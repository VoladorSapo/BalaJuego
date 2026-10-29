using System;
using UnityEngine;

/// <summary>
/// States that consists of an animation and optional endAnimation. The logic for ending the animation can 
/// </summary>
public class EnemyPlayAnimationState: BaseEnemyState
{
    protected string animName { get; set; }
    bool animationEnded;

    public EnemyPlayAnimationState(AEnemyBehaviour _enemy, string AnimName)
    {
        enemy = _enemy;
        this.animName = AnimName;
    }
    public override void OnEnter()
    {
        animationEnded = false;
        Debug.Log(animName);
        enemy.playAnimation(animName, () => animationEnded = true);
    }
    public override void OnExit()
    {
        Debug.Log("end" + animName);
    }

    public override bool hasPreExitAction()
    {
        return enemy.checkHasAnimation(animName + "End");
    }
    public override void preExit(Action action)
    {
        Debug.Log("preExit");
        enemy.playAnimation(animName + "End", action);
    }
    protected override string generateIdCode()
    {
        return this.GetType().ToString()+animName;
    }
    public override bool ShouldEnd()
    {
        return animationEnded;
    }
}
