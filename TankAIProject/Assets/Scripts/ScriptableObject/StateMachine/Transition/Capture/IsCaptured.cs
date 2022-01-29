using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Transition/Capture/Is Captured")]
public class IsCaptured : TransitionBase
{
    public override bool IsValid(StateMachineManager stateMachineManager)
    {
        CaptureData data = (CaptureData)stateMachineManager.m_Data;
        return data.m_Value >= 100;
    }
}
