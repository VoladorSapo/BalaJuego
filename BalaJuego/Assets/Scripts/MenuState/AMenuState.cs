using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class AMenuState : IState
{

    protected MenuController menu;

    public AMenuState(MenuController menu)
    { this.menu = menu; }
    public abstract void FixedUpdate();

    public abstract void OnEnter();

    public abstract void OnExit();

    public abstract void Update();

    public abstract void HandleButton(string i);

    public virtual bool ShouldEnd()
    {
        return false;
    }

    public void SetUp(string Data)
    {
    }

    public bool hasPreExitAction()
    {
        return false;
    }

    public void preExit(Action action)
    {
    }
}
