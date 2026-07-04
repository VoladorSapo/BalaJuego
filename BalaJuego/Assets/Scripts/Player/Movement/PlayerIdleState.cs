using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerMove player) : base(player)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        player.UpdateAnimatorBool("isRunning", false);
        player.UpdateAnimatorBool("isGround", true);
        musicManager.Instance.StopWalking();
    }
    public override void Update()
    {
        base.Update();
        player.beOnGround();

    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        player.calcVelocity.x = Mathf.MoveTowards(player.calcVelocity.x, 0, player.groundDecceleration * Time.fixedDeltaTime);
        rb2d.velocity = player.calcVelocity;
    }
}
public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerMove player) : base(player)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        player.UpdateAnimatorBool("isRunning", true);
        player.UpdateAnimatorBool("isGround", true);
        musicManager.Instance.StartWalking();
    }
    public override void Update()
    {
        base.Update();
        player.beOnGround();

    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        //dustWalk.gameObject.SetActive(true);
        //if (onGround) dustWalk.Play();
        float useAccel = (Mathf.Abs(player.calcVelocity.x) == 0 || Mathf.Sign(player.calcVelocity.x) == player.MoveX) ?   player.acceleration : player.turnDecceleration;
        player.calcVelocity.x = Mathf.MoveTowards(player.calcVelocity.x, player.MoveX * player.maxSpeed * player.timeMagnitude, useAccel * Time.fixedDeltaTime * player.timeMagnitude);
        rb2d.velocity = player.calcVelocity;
    }
    public override void OnExit()
    {
        base.OnExit();
        musicManager.Instance.StopWalking();

    }
}
public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerMove player) : base(player)
    {
    }
    public override void OnEnter()
    {
        base.OnEnter();

        //dustJump.gameObject.SetActive(true);
        //dustJump.Play();
        rb2d.gravityScale = player.normalGravity;
        musicManager.Instance.PlayJump();
        rb2d.gravityScale = player.normalGravity;
        rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
        rb2d.AddForce(Vector2.up * player.jumpForce, ForceMode2D.Impulse);
        Debug.Log("addforce");
        player.coyoteTimeCurrent = player.jumpBufferTimeCurrent = 0;
        player.UpdateAnimatorBool("isGround", false);

    }
    public override void Update()
    {
        base.Update();
      
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        float useAccel = (Mathf.Abs(player.calcVelocity.x) == 0 || Mathf.Sign(player.calcVelocity.x) == player.MoveX) ? player.acceleration : player.turnDecceleration;
        player.calcVelocity.x = Mathf.MoveTowards(player.calcVelocity.x, player.MoveX * player.maxSpeed * player.timeMagnitude, useAccel * Time.fixedDeltaTime * player.timeMagnitude);
        rb2d.velocity = player.calcVelocity;
    }
}
public class PlayerFallState : PlayerBaseState
{
    public PlayerFallState(PlayerMove player) : base(player)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter(); 
        rb2d.gravityScale = player.fallGravity;
        player.UpdateAnimatorBool("isGround", false);


    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        float useAccel = (Mathf.Abs(player.calcVelocity.x) == 0 || Mathf.Sign(player.calcVelocity.x) == player.MoveX) ? player.acceleration : player.turnDecceleration;
        player.calcVelocity.x = Mathf.MoveTowards(player.calcVelocity.x, player.MoveX * player.maxSpeed * player.timeMagnitude, useAccel * Time.fixedDeltaTime * player.timeMagnitude);
        if (player.calcVelocity.y < -player.maxFallVelocity)
        {
            player.calcVelocity.y = -player.maxFallVelocity;
        }
        Debug.Log("FallVelocity"+player.calcVelocity.y);
        rb2d.velocity = player.calcVelocity;
    }
}
public class PlayerStartDodgeRollState : PlayerBaseState
{
    int dodgeRollDirection;
    public PlayerStartDodgeRollState(PlayerMove player) : base(player)
    {
    }
    public override void OnEnter()
    {
        base.OnEnter();
        dodgeRollDirection=player.MoveX;
    }
}
public class PlayerEndDodgeRollState : PlayerBaseState
{
    int dodgeRollDirection;
    public PlayerEndDodgeRollState(PlayerMove player) : base(player)
    {
    }
    public override void OnEnter()
    {
        base.OnEnter();
        dodgeRollDirection = player.MoveX;
    }
}
public class PlayerCrouchedState : PlayerBaseState
{
    public PlayerCrouchedState(PlayerMove player) : base(player)
    {
        
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
public abstract class PlayerBaseState: IState
{
    protected PlayerMove player;
    protected Rigidbody2D rb2d;
    public PlayerBaseState(PlayerMove player)
    {
        this.player = player;
    }

    public virtual void OnEnter()
    {
        rb2d = player.GetComponent<Rigidbody2D>();
    }
    public virtual void Update()
    {
        player.MoveX = (int)player.playerInput.Move;
    }
    public virtual void FixedUpdate()
    {
        player.calcVelocity = rb2d.velocity;
    }
    public virtual void OnExit()
    {

    }
}