using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Decorator : Behavior
{
    public Behavior m_Child;

    public override void AddAITank(int teamIndex, int tankIndex = -1)
    {
        m_Child.AddAITank(teamIndex, tankIndex);
    }
    public override void RemoveAITank(int teamIndex, int tankIndex = -1)
    {
        m_Child.RemoveAITank(teamIndex, tankIndex);
    }
}
