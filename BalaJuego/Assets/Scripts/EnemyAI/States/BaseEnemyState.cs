using System;
using UnityEngine;

public abstract class BaseEnemyState : IState
{
    protected AEnemyBehaviour enemy;
    protected string animName;

    public virtual void SetUp(string Data)
    {
        if (string.IsNullOrEmpty(Data))
        {
            animName = DefaultAnimName();
        }
        else
        {
            animName = Data;
        }
    }
    public virtual void OnEnter()
    {

    }
    public virtual void Update()
    {

    }
    public virtual void FixedUpdate()
    {

    }
    public virtual void OnExit()
    {

    }
    public virtual bool ShouldEnd()
    {
        return false;
    }

    /// <summary>
    /// Debug message only if DebugOn is activated in EnemyBehaviour
    /// </summary>
    /// <param name="msg"></param>
    protected void LogDebug(string msg)
    {
        if (enemy.DebugOn)
        {
            Debug.Log($"FSM DEBUG ENEMY {enemy.name}: {msg}");
        }
    }
    protected void LogValue(string name,string val)
    {
        if (enemy.DebugOn)
        {
            Debug.Log($"FSM DEBUG ENEMY {enemy.name} {name}: {val}");
        }
    }


    protected virtual string DefaultAnimName() => "";

    public virtual bool hasPreExitAction()
    {
        return false;
    }

    public virtual void preExit(Action action)
    {
    }
}
