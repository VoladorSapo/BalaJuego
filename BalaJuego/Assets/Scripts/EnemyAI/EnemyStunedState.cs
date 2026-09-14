using System;
using Unity.VisualScripting.FullSerializer;
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
        Debug.Log(animName);
        enemy.playAnimation(animName, () => Debug.Log("FINISHANIMATION"));
    }
    public override void OnExit()
    {
        Debug.Log("end"+animName);
        enemy.stunedCollider.gameObject.SetActive(false);
    }

    public override bool hasPreExitAction()
    {
        return enemy.checkHasAnimation(animName+"End");
    }
    public override void preExit(Action action)
    {
        Debug.Log("preExit");
        enemy.playAnimation(animName + "End",action);
    }
    protected override string DefaultAnimName()
    {
        return "Stun";
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
