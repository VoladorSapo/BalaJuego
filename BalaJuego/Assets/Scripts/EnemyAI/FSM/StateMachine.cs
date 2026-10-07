using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StateMachine
{
    StateNode current;
    Dictionary<Type, StateNode> nodes = new();
    HashSet<ITransition> anyTransitions = new();
    IState defaultState;
    public void Update()
    {
        var transition = GetTransition();
        if (transition != null)
        {
            ChangeState(transition.To,transition.Data);
        }
        //Debug.Log(current?.State?.ToString());
        current?.State?.Update();
        if (current?.State?.ShouldEnd() == true)
        {
            endState();
        }
    }

    

    public void FixedUpdate()
    {
        current?.State?.FixedUpdate();
    }

    public void setDefaultState(IState state)
    {
        defaultState = state;
    }
    public void restart()
    {
        if(defaultState == null)
        {
            throw new Exception("Default State not Set exception");
        }
        SetState(defaultState);
    }
    public void SetState(IState state, string Data = "")
    {
        if (current != null)
        {
            ChangeState((IState)state, Data);
        }
        else
        {
            current = nodes[state.GetType()];
            current.State.SetUp(Data);
            current.State.OnEnter();
        }

    }
    public void ForceSetState(IState state,string Data = "")
    {
        GetOrAddNode(state);
        if (current != null)
        {
            ChangeState((IState)state, Data);
        }
        else
        {
            current = nodes[state.GetType()];
            current.State.SetUp(Data);
            current.State.OnEnter();
        }
    }
    public void ForceEndState(IState state)
    {
        Debug.Log("try end state"+state.GetType()+ current.State.GetType());
        if (state.GetType().Equals(current.State.GetType()))
        {
            Debug.Log("end state");
            endState();
        }
    }
    private void endState()
    {
       var transition = GetEndTransition();
        if (transition != null)
        {
            ChangeState(transition.To,transition.Data);
        }
    }
    void ChangeState(IState state,string Data = "")
    {
        if (state == current.State) return;

        
        Action trueEndState = () => {
            var previousState = current.State;
            var nextState = nodes[state.GetType()].State;
            previousState?.OnExit();
            if (nextState != null)
            {
                Debug.Log(Data);
                nextState.SetUp(Data);
            }
            nextState?.OnEnter();
            current = nodes[state.GetType()];
        };
        if (current.State.hasPreExitAction())
        {
            current.State.preExit(trueEndState);
        }
        else
        {
            trueEndState.Invoke();
        }

    }

    ITransition GetTransition()
    {
        foreach (var transition in anyTransitions)
            if (transition.Condition.Evaluate())
                return transition;
        if (current != null && current.Transitions != null)
        {
            foreach (var transition in current.Transitions)
                if (transition.Condition.Evaluate())
                    return transition;
        }
        return null;
    }
    private ITransition GetEndTransition()
    {
        if (current != null && current.EndTransitions != null)
        {
            foreach (var transition in current.EndTransitions)
                if (transition.Condition.Evaluate())
                    return transition;
        }
        return null;
    }

    public void AddTransition(IState from, IState to, IPredicate condition, string Data = "")
    {
        GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition,Data);
    }
    public void AddEndTransition(IState from, IState to, IPredicate condition, string Data = "")
    {
        GetOrAddNode(from).AddEndTransition(GetOrAddNode(to).State, condition, Data);
    }

    public void AddAnyTransition(IState to, IPredicate condition, string Data = "")
    {
        anyTransitions.Add(new Transition(GetOrAddNode(to).State, condition, Data));
    }

    StateNode GetOrAddNode(IState state)
    {
        var node = nodes.GetValueOrDefault(state.GetType());

        if (node == null)
        {
            node = new StateNode(state);
            nodes.Add(state.GetType(), node);
        }

        return node;
    }
    public IState currentState()
    {
        if (current == null)
            return null;
        return current.State;
    }
}

class StateNode
{
    public IState State { get; }
    public HashSet<ITransition> Transitions { get; }
    public HashSet<ITransition> EndTransitions { get; }

    public StateNode(IState _state)
    {
        State = _state;
        Transitions = new HashSet<ITransition>();
        EndTransitions = new HashSet<ITransition>();
    }

    public void AddTransition(IState state, IPredicate condition, string Data = "")
    {
        Transitions.Add(new Transition(state, condition,Data));
    }
    public void AddEndTransition(IState state, IPredicate condition, string Data = "")
    {
        EndTransitions.Add(new Transition(state, condition,Data));
    }
}
