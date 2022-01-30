using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTree : ScriptableObject
{
    public Behavior m_Root;

    public void Tick(int tankIndex)
    {
        m_Root.Tick(tankIndex);
    }

    public void AddAITank(int tankIndex)
    {
        m_Root.AddAITank(tankIndex);
    }
    public void RemoveAITank(int tankIndex)
    {
        m_Root.RemoveAITank(tankIndex);
    }
}
