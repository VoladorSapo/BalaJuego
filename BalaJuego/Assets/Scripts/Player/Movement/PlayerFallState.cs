using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    bool comeFromJumpUp;
    public PlayerFallState(PlayerMove player) : base(player)
    {
    }
    public override void SetUp(string Data)
    {
        base.SetUp(Data);
        
            comeFromJumpUp = Data == "JumpUp";
        

    }
    public override void OnEnter()
    {
        base.OnEnter();
        rb2d.gravityScale = player.fallGravity * player.timeMagnitude;
        //AnimatorExtensions.UpdateAnimatorBool(player.Animators, "isGround", false);
        player.anim.SetBool("isGround", false);

    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (comeFromJumpUp)
        {
            Debug.Log("JumpCut");
            player.calcVelocity.y *= player.jumpCutProportion;
            comeFromJumpUp = false;
        }
        float useAccel = (Mathf.Abs(player.calcVelocity.x) == 0 || Mathf.Sign(player.calcVelocity.x) == player.MoveX) ? player.acceleration : player.turnDecceleration;
        player.calcVelocity.x = Mathf.MoveTowards(player.calcVelocity.x, player.MoveX * player.maxSpeed * player.timeMagnitude, useAccel * Time.fixedDeltaTime * player.timeMagnitude);
        if (player.calcVelocity.y < -player.maxFallVelocity)
        {
            player.calcVelocity.y = -player.maxFallVelocity;
        }
       // Debug.Log("FallVelocity"+player.calcVelocity.y);
        rb2d.linearVelocity = player.calcVelocity;
    }
}
