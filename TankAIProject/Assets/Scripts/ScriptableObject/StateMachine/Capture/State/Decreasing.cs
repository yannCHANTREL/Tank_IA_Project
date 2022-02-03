using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/State/Capture/Decrease")]
public class Decreasing : StateBase
{
    public float m_DecreaseSpeed;
    
    public override void OnEnter(StateMachineManager stateMachineManager)
    { }

    public override void OnExit(StateMachineManager stateMachineManager)
    { }

    protected override void Execute(StateMachineManager stateMachineManager)
    {
        CaptureData data = (CaptureData)stateMachineManager.m_Data;
        
        if (data.m_Value <= 0) return;
        
        data.m_Value -= Time.deltaTime * 100 / m_DecreaseSpeed;
        data.UpdateSlider();
    }
}
