using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Composite : Behavior
{
    public List<Behavior> m_Children;

    public override void AddAITank(int tankIndex)
    {
        foreach (var child in m_Children)
        {
            child.AddAITank(tankIndex);
        }
    }
    public override void RemoveAITank(int tankIndex)
    {
        foreach (var child in m_Children)
        {
            child.RemoveAITank(tankIndex);
        }
    }
}
