using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineManager : MonoBehaviour
{
    public StateBase m_DefaultStateBase;
    
    private StateBase m_CurrentStateBase;

    private void Start()
    {
        m_CurrentStateBase = m_DefaultStateBase;
        m_CurrentStateBase.OnEnter(this);
    }

    private void Update()
    {
        m_CurrentStateBase.StateUpdate(this);
    }

    public void TransitionTo(StateBase stateBase)
    {
        m_CurrentStateBase.OnExit(this);
        m_CurrentStateBase = stateBase;
        m_CurrentStateBase.OnEnter(this);
    }
}
