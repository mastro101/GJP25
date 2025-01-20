using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    State currentState;
    State[] states;

    protected void Awake()
    {
        states = gameObject.GetComponentsInChildren<State>();
        currentState = states[0];
    }

    private void Start()
    {
        currentState.OnEnter();
    }

    public void ChangeState(State _s, bool hardReset)
    {
        if (currentState == null || _s == null)
            return;

        if (_s == currentState && !hardReset)
            return;

        _s.OnExit();
        currentState = _s;
        currentState.OnEnter();
    }

    protected void Update()
    {
        currentState.OnTick();
    }

    protected void FixedUpdate()
    {
        currentState.OnFixedTick();
    }
}
