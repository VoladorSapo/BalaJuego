using UnityEngine;

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
