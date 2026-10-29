using System;
using UnityEngine;

public abstract class BaseEnemyState : IState
{
    protected AEnemyBehaviour enemy;
    protected string idCode;
    protected bool generatedCode = false;
    /// <summary>
    /// Data that can be sent every time the state is reached, run before OnEnter. Allows for the same state to be reused in different configurations, as the data is stored in the transition.
    /// For example, a state that only plays an animation and ends when it finishes could be used for multiple actions
    /// </summary>
    /// <param name="Data"></param>
   
        
    protected virtual string generateIdCode()
    {
        return this.GetType().ToString();
    }
    public virtual void SetUp(string Data = "")
    {
      
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
    /// <summary>
    /// Checks if the end condition for the state has been reached
    /// </summary>
    /// <returns></returns>
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



    public virtual bool hasPreExitAction()
    {
        return false;
    }

    public virtual void preExit(Action action)
    {
    }

    public string GetIdCode()
    {
        if (!generatedCode)
        {
            generatedCode = true;
           idCode = generateIdCode();   
        }
        return idCode;
    }
}
