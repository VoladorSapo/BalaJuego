using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StateMachine
{
    StateNode current;
    Dictionary<Type, StateNode> nodes = new();
    HashSet<ITransition> anyTransitions = new();
    Type defaultState;
    public void Update()
    {
        var transition = GetTransition();
        if (transition != null)
        {
            ChangeState(transition.To);
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
        defaultState = state.GetType();
    }
    public void restart()
    {
        if(defaultState == null)
        {
            throw new Exception("Default State not Set exception");
        }
        SetState(defaultState);
    }
    private void SetState(Type type)
    {
        current = nodes[type];
        current.State?.OnEnter();
    }
    public void SetState(IState state)
    {
        current = nodes[state.GetType()];
        current.State?.OnEnter();
        
    }
    public void ForceSetState(IState state)
    {
        GetOrAddNode(state);
        current = nodes[state.GetType()];
        current.State?.OnEnter();
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
            ChangeState(transition.To);
        }
    }
    void ChangeState(IState state)
    {
        if (state == current.State) return;

        var previousState = current.State;
        var nextState = nodes[state.GetType()].State;

        previousState?.OnExit();
        nextState?.OnEnter();
        current = nodes[state.GetType()];
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

    public void AddTransition(IState from, IState to, IPredicate condition)
    {
        GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition);
    }
    public void AddEndTransition(IState from, IState to, IPredicate condition)
    {
        GetOrAddNode(from).AddEndTransition(GetOrAddNode(to).State, condition);
    }

    public void AddAnyTransition(IState to, IPredicate condition)
    {
        anyTransitions.Add(new Transition(GetOrAddNode(to).State, condition));
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

    public void AddTransition(IState state, IPredicate condition)
    {
        Transitions.Add(new Transition(state, condition));
    }
    public void AddEndTransition(IState state, IPredicate condition)
    {
        EndTransitions.Add(new Transition(state, condition));
    }
}
