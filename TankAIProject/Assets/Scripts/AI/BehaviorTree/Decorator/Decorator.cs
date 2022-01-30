using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Decorator : Behavior
{
    public Behavior m_Child;

    public override void AddAITank(int tankIndex)
    {
        m_Child.AddAITank(tankIndex);
    }
    public override void RemoveAITank(int tankIndex)
    {
        m_Child.RemoveAITank(tankIndex);
    }
}
