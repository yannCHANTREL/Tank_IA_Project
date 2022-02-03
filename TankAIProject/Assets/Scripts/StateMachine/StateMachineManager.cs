using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineManager : MonoBehaviour
{
    public StateBase m_DefaultState;
    public DataBase m_Data;
    
    private StateBase m_CurrentState;

    private void Start()
    {
        m_CurrentState = m_DefaultState;
        m_CurrentState.OnEnter(this);
    }

    private void Update()
    {
        m_CurrentState.StateUpdate(this);
    }

    public void TransitionTo(StateBase stateBase)
    {
        m_CurrentState.OnExit(this);
        m_CurrentState = stateBase;
        m_CurrentState.OnEnter(this);
    }
}
