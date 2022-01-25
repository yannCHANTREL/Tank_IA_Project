using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Transition/Wait")]

public class TransitionWait : TransitionBase
{
    public float m_WaitTime;

    private float m_StartTime;
    private bool m_FirstTime = true;
    
    public override bool IsValid(StateMachineManager stateMachineManager)
    {
        if (m_FirstTime)
        {
            m_FirstTime = false;
            m_StartTime = Time.time;
        }
        
        return Time.time - m_StartTime >= m_WaitTime;
    }
}
