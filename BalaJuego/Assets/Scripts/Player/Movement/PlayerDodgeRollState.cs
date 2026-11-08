using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class PlayerDodgeRollState : PlayerBaseState
{
    int dodgeRollDirection;
    float useAcell;
    float objspeed;

    //1 acel, 2 maxspeed, 3 decel
    int rollPhase;
    float timeInCurrentPhase;
    Vector3 phasesTimes;
    public PlayerDodgeRollState(PlayerMove player) : base(player)
    {
    }
    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("START ROLL");
        player.PlayAnimation("roll");
        player.playerShoot.setCanShoot(false);

        dodgeRollDirection = player.MoveX == 0 ? -(int)player.transform.localScale.x : player.MoveX;
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
        rb2d.gravityScale = player.fallGravity * player.timeMagnitude;
        timeInCurrentPhase = 0;
        Vector3 proportions = new Vector3(player.rollAccelerationTimeProportion, player.rollMaxSpeedTimeProportion, player.rollDeccelerationTimeProportion);
        float sum = proportions.x + proportions.y + proportions.z;
        proportions = proportions / sum;
        phasesTimes = proportions * player.rollTime;        
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        //float realobjspeed = objspeed * player.timeMagnitude;
        //player.calcVelocity.x = Mathf.MoveTowards(player.calcVelocity.x, realobjspeed, useAcell * Time.fixedDeltaTime * player.timeMagnitude);
        //rb2d.linearVelocity = player.calcVelocity;

        Vector2 tangent = new Vector2(player.hitGround.normal.y, -player.hitGround.normal.x);
        if(tangent.x == 0)
        {
            tangent.x = 1;
        }
        Debug.Log("tangent" + tangent);
        float realobjspeed = objspeed * player.timeMagnitude;
        float currentvelocity;
        if (tangent.x > 0.01f)
        {
            currentvelocity = player.calcVelocity.x / tangent.x;
        }
        else
        {
            currentvelocity = player.calcVelocity.x;
        }
        currentvelocity = Mathf.MoveTowards(currentvelocity, realobjspeed, useAcell * Time.fixedDeltaTime * player.timeMagnitude);
      //  Debug.Log($"RollPhasePre{currentvelocity} {realobjspeed} {useAcell} {useAcell * Time.fixedDeltaTime * player.timeMagnitude}");

        player.calcVelocity.x = currentvelocity * tangent.x;
        player.calcVelocity.y = currentvelocity * tangent.y;
        rb2d.linearVelocity = player.calcVelocity;

        switch (rollPhase)
        {
            case 1:
           //     Debug.Log($"RollPhase {rollPhase} {timeInCurrentPhase} {phasesTimes[rollPhase - 1]}");
                timeInCurrentPhase += Time.fixedDeltaTime;
                if (timeInCurrentPhase >= phasesTimes[rollPhase-1])
                {
                    if (player.DEBUG_dodgewhenwalkspeedismax)
                    {
                        player.playerLife.setInvincibility(true);
                        if (player.DEBUG_canDodgeHeavy)
                            player.setGoThroughEnemies(true);
                    }
                    timeInCurrentPhase = 0;
                    rollPhase = 2;
                //    Debug.Log($"RollPhaseChange: {rollPhase}");

                }
                break;
            case 2:
            //    Debug.Log($"RollPhase {rollPhase} {timeInCurrentPhase} {phasesTimes[rollPhase - 1]}");

                timeInCurrentPhase += Time.fixedDeltaTime;
                if (timeInCurrentPhase >= phasesTimes[rollPhase - 1])
                {
                    rollPhase = 3;
                    timeInCurrentPhase = 0;
                    //useAcell = dodgeRollDirection * player.rollAcceleration;
                    if (player.DEBUG_endOnWalkSpeed)
                    {
                        objspeed = player.maxSpeed * dodgeRollDirection;
                    }
                    else
                    {
                        objspeed = 0;
                    }
             //       Debug.Log($"RollPhaseChange: {rollPhase}");

                }
                break;
            case 3:
          //      Debug.Log($"RollPhase {rollPhase} {timeInCurrentPhase} {phasesTimes[rollPhase - 1]}");

                timeInCurrentPhase += Time.fixedDeltaTime;
                if ((timeInCurrentPhase >= phasesTimes[rollPhase - 1]))
                {
                    rollPhase = 4;
                 //   Debug.Log($"RollPhaseChange: {rollPhase}");

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
        player.playerShoot.setCanShoot(true);

    }
}
