using UnityEngine;

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
