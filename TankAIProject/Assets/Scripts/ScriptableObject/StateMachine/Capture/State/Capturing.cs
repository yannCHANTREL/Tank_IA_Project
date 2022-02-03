using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/State/Capture/Capturing")]
public class Capturing : StateBase
{
    
    public override void OnEnter(StateMachineManager stateMachineManager)
    { }

    public override void OnExit(StateMachineManager stateMachineManager)
    { }

    protected override void Execute(StateMachineManager stateMachineManager)
    {
        CaptureData data = (CaptureData)stateMachineManager.m_Data;
        
        if (data.m_OldTeamCapturing == data.m_CurrentTeamCapturing)
        {
            data.m_Value += (Time.deltaTime * 100) / data.m_FullCaptureDuration;
        }
        else
        {
            data.m_Value -= (Time.deltaTime * 100) / (data.m_FullCaptureDuration / 2);
        }
        
        data.UpdateSlider();
      
        if (data.m_Value < 0)
        {
            data.m_Value = 0;
            data.m_OldTeamCapturing = data.m_CurrentTeamCapturing;
            data.ChangeFillColor();
        }
    }
}
