using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FiniteStateMachine<T>
{
    private T _owner;
    private State _currentState;
    private State _prevState;
    private Dictionary<string, List<Transition>> _transitions = new Dictionary<string, List<Transition>>();
    private List<Transition> _currentTransitions = new List<Transition>();
    private bool _debugMode;
    private string _username;
    public FiniteStateMachine(T owner, string username = "", bool debugMode = false)
    {
        _owner = owner;
        _username = username;
        _debugMode = debugMode;
    }

    public State PrevState => _prevState;

    public void Tik()
    {
        State nextState = GetNextState();
        if(nextState != null)
            SetState(nextState);

        if(_currentState != null)
            _currentState.Tik();
    }

    public void SetState(State state)
    {
        if(state == _currentState)
            return;
        
        _currentState?.Exit();
        if(_debugMode)
            Debug.Log($"{_username} => Changing State FROM:{_currentState?.Name} --> TO:{state.Name}");
        _prevState = _currentState;
        _currentState = state;

        _transitions.TryGetValue(_currentState.Name, out _currentTransitions);

        _currentState.Enter();
    }

    public void AddTransition(State fromState, State toState, Func<bool> transitionCondition)
    {
        if (_transitions.TryGetValue(fromState.Name, out var stateTransitions) == false)
        {
            stateTransitions = new List<Transition>();
            _transitions[fromState.Name] = stateTransitions;
        }

        stateTransitions.Add(new Transition(toState, transitionCondition));

    }

    private State GetNextState()
    {
        if(_currentTransitions == null)
            Debug.LogError($"{_username} => Current State {_currentState.Name} has NO transitions!");

        foreach (Transition transition in _currentTransitions)
        {
            if (transition.Condition())
                return transition.NextState;
        }

        return null;
    }
}
