
using System;

public interface IState
{
    void OnEnter();
    void Update();
    bool ShouldEnd();
    void FixedUpdate();
    void OnExit();

    void SetUp(string Data);

    bool hasPreExitAction();

    void preExit(Action action);
}