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
        AnimatorExtensions.UpdateAnimatorBool(player.Animators, "isRunning", false);
        AnimatorExtensions.UpdateAnimatorBool(player.Animators, "isGround", true);
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
        rb2d.linearVelocity = player.calcVelocity;
    }
}
//public class PlayerEndDodgeRollState : PlayerBaseState
//{
//    int dodgeRollDirection;
//    public PlayerEndDodgeRollState(PlayerMove player) : base(player)
//    {
//    }
//    public override void OnEnter()
//    {
//        base.OnEnter();
//        dodgeRollDirection = player.MoveX;
//    }
//}
