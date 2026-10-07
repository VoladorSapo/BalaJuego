using UnityEngine;
public class EnemyIdleState : BaseEnemyState
{
    public EnemyIdleState(AEnemyBehaviour _enemy)
    {
        enemy = _enemy;
    }

    public override void OnEnter()
    { 
        Debug.Log("start Idle");
        enemy.playAnimation("Idle");
    }
    public override void Update()
    {
    }
}


