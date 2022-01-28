using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Transition/Capture/No Team")]
public class NoTeam : TransitionBase
{
    public override bool IsValid(StateMachineManager stateMachineManager)
    {
        CaptureData data = (CaptureData)stateMachineManager.m_Data;
        return data.GetNumberTeamOnPoint() == 0;
    }
}
