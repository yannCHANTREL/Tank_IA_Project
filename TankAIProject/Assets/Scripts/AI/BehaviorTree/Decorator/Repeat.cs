using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BehaviorTree/Decorator/Repeat")]
public class Repeat : Decorator
{
    public int m_Limit;
    public override void OnInitialize()
    {

    }

    public override Status BHUpdate(int teamIndex, int tankIndex = 0)
    {
        for (int i = 0; i < m_Limit; i++)
        {
            m_Child.Tick(teamIndex, tankIndex);
            if (m_Child.m_Status == Status.running) i--;
            if (m_Child.m_Status == Status.failure) return Status.failure;
        }
        return Status.success;
    }

    public override void OnTerminate(Status status)
    {

    }
}