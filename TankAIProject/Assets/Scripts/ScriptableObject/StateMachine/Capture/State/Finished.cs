using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/State/Capture/Finished")]
public class Finished : StateBase
{
    public override void OnEnter(StateMachineManager stateMachineManager)
    {
        CaptureData data = (CaptureData)stateMachineManager.m_Data;
        data.m_IsGameFinished = false;
    }

    public override void OnExit(StateMachineManager stateMachineManager)
    { }

    protected override void Execute(StateMachineManager stateMachineManager)
    { }
}
