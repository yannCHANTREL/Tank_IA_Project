using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Transition/Capture/Is Capturing")]
public class IsCapturing : TransitionBase
{
    public override bool IsValid(StateMachineManager stateMachineManager)
    {
        CaptureData data = (CaptureData)stateMachineManager.m_Data;
        return data.GetTeamCapturingNumber() != 0;
    }
}
