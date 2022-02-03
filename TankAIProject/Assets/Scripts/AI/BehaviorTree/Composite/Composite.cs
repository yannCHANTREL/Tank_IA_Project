using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Composite : Behavior
{
    public List<Behavior> m_Children;

    public override void AddAITank(int teamIndex, int tankIndex = -1)
    {
        foreach (var child in m_Children)
        {
            child.AddAITank(teamIndex, tankIndex);
        }
    }
    public override void RemoveAITank(int teamIndex, int tankIndex = -1)
    {
        foreach (var child in m_Children)
        {
            child.RemoveAITank(teamIndex, tankIndex);
        }
    }
}
