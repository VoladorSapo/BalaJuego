using System;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class EnemyStunedState: EnemyPlayAnimationState
{
    public EnemyStunedState(AEnemyBehaviour _enemy): base(_enemy,"Stun")
    {
        enemy = _enemy;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        enemy.stunedCollider.gameObject.SetActive(true);
      
    }
    public override void OnExit()
    {
        base.OnExit();
        enemy.stunedCollider.gameObject.SetActive(false);
    }

    public override bool ShouldEnd()
    {
        return false;
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
        Debug.Log(enemy);
        enemy.detectorManager.detectorDictionary["wall"].gameObject.SetActive(false);

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
