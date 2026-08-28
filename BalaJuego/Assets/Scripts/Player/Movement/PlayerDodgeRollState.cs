using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class PlayerDodgeRollState : PlayerBaseState
{
    int dodgeRollDirection;
    float useAcell;
    float objspeed;

    //1 acel, 2 maxspeed, 3 decel
    int rollPhase;
    float timeInMaxSpeed;
    public PlayerDodgeRollState(PlayerMove player) : base(player)
    {
    }
    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("START ROLL");
        player.PlayAnimation("roll");
        dodgeRollDirection = player.MoveX;
        if (!player.DEBUG_dodgewhenwalkspeedismax)
        {
            player.playerLife.setInvincibility(true);
            if (player.DEBUG_canDodgeHeavy)
                player.setGoThroughEnemies(true);
        }
        useAcell = player.rollAcceleration;
        objspeed = player.maxRollSpeed * dodgeRollDirection;
        rollPhase = 1;
        Debug.Log($"RollPhaseChange: {rollPhase}");
        rb2d.gravityScale = player.fallGravity;
        timeInMaxSpeed = 0;
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        float realobjspeed = objspeed * player.timeMagnitude;
        player.calcVelocity.x = Mathf.MoveTowards(player.calcVelocity.x, realobjspeed, useAcell * Time.fixedDeltaTime * player.timeMagnitude);
        rb2d.velocity = player.calcVelocity;
        switch (rollPhase)
        {
            case 1:
                Debug.Log($"RollPhase{player.calcVelocity.x} {realobjspeed}");
                if (Mathf.Abs(player.calcVelocity.x) >= Mathf.Abs(realobjspeed))
                {
                    if (player.DEBUG_dodgewhenwalkspeedismax)
                    {
                        player.playerLife.setInvincibility(true);
                        if (player.DEBUG_canDodgeHeavy)
                            player.setGoThroughEnemies(true);
                    }
                    rollPhase = 2;
                    Debug.Log($"RollPhaseChange: {rollPhase}");

                }
                break;
            case 2:
                timeInMaxSpeed += Time.fixedDeltaTime;
                if (timeInMaxSpeed > player.rollMaxSpeedTime)
                {
                    rollPhase = 3;
                    //useAcell = dodgeRollDirection * player.rollAcceleration;
                    if (player.DEBUG_endOnWalkSpeed)
                    {
                        objspeed = player.maxSpeed * dodgeRollDirection;
                    }
                    else
                    {
                        objspeed = 0;
                    }
                    Debug.Log($"RollPhaseChange: {rollPhase}");

                }
                break;
            case 3:
                if (Mathf.Abs(player.calcVelocity.x) <= Mathf.Abs(realobjspeed))
                {
                    rollPhase = 4;
                    Debug.Log($"RollPhaseChange: {rollPhase}");

                }
                break;
            default: break;
        }
    }
    public override bool ShouldEnd()
    {
        return rollPhase == 4;
    }
    public override void OnExit()
    {
        base.OnExit();
        player.playerLife.setInvincibility(false);
        player.setGoThroughEnemies(false);

    }
}
