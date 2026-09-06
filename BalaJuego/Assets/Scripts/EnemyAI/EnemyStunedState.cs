using System;
using UnityEngine;

public class EnemyStunedState: BaseEnemyState
{

    public EnemyStunedState(AEnemyBehaviour _enemy)
    {
        enemy = _enemy;
    }

    public override void OnEnter()
    {
        enemy.stunedCollider.gameObject.SetActive(true);
        enemy.anim.Play("enemyStun");
    }
    
    public override void OnExit()
    {
        Debug.Log("end stun");
        enemy.stunedCollider.gameObject.SetActive(false);
      
    }
}

public class HeavyStunedState : EnemyStunedState
{
    public HeavyStunedState(AEnemyBehaviour _enemy) : base(_enemy)
    {
        enemy = _enemy;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        musicManager.Instance.StopHeavyWalking();
        enemy.GetComponentInChildren<WallDetector>().gameObject.SetActive(false);

    }
}

public class EnemyDeadState: BaseEnemyState
{
    public EnemyDeadState(AEnemyBehaviour _enemy)
    {
        enemy = _enemy;
    }

    public override void OnEnter()
    {
      
    }

    public override void OnExit()
    {
       

    }
}
