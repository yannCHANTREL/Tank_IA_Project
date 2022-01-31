using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Transition/Capture/Multiple Team")]
public class MultipleTeam : TransitionBase
{
    public override bool IsValid(StateMachineManager stateMachineManager)
    {
        CaptureData data = (CaptureData)stateMachineManager.m_Data;
        return data.GetNumberTeamOnPoint() > 1;
    }
}
