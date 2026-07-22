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
public class PlayerDodgeRollState : PlayerBaseState
{
    int dodgeRollDirection;
    public PlayerDodgeRollState(PlayerMove player) : base(player)
    {
    }
    public override void OnEnter()
    {
        base.OnEnter();
        dodgeRollDirection=player.MoveX;
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        float useAccel = dodgeRollDirection * player.rollAcceleration;
        player.calcVelocity.x = Mathf.MoveTowards(player.calcVelocity.x, player.MoveX * player.maxRollSpeed * player.timeMagnitude, useAccel * Time.fixedDeltaTime * player.timeMagnitude);
        rb2d.velocity = player.calcVelocity;
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
