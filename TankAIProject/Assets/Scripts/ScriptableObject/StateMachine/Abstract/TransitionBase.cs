using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TransitionBase : ScriptableObject
{
    public StateBase m_NextStateBase;
    
    public abstract bool IsValid(StateMachineManager stateMachineManager);
}
