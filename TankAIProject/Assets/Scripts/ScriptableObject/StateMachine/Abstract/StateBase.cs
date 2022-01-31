using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBase : ScriptableObject
{
    public TransitionBase[] m_Transitions;
    
    
    public abstract void OnEnter(StateMachineManager stateMachineManager);
    public abstract void OnExit(StateMachineManager stateMachineManager);
    protected abstract void Execute(StateMachineManager stateMachineManager);
    
    
    public void StateUpdate(StateMachineManager stateMachineManager)
    {
        Execute(stateMachineManager);
        CheckTransitions(stateMachineManager);
    }
    
    private void CheckTransitions(StateMachineManager stateMachineManager)
    {
        foreach (TransitionBase transition in m_Transitions)
        {
            if (!transition.IsValid(stateMachineManager)) continue;
            
            stateMachineManager.TransitionTo(transition.m_NextStateBase);
            break;
        }
    }
}
