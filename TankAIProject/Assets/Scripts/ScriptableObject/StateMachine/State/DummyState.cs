using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/State/Dummy")]
public class DummyState : StateBase
{
    public string m_StateName;
    
    public override void OnEnter(StateMachineManager stateMachineManager)
    {
        Debug.Log($"Enter state {name}");
    }

    public override void OnExit(StateMachineManager stateMachineManager)
    {
        Debug.Log($"Exit state {name}");
    }

    protected override void Execute(StateMachineManager stateMachineManager)
    {
        Debug.Log($"Execute state {name}");
    }
}
