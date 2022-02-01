using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BehaviorTree/Composite/Sequence")]
public class Sequence : Composite
{
    public override void OnInitialize()
    {

    }

    public override Status BHUpdate(int teamIndex, int tankIndex = 0)
    {
        foreach(var child in m_Children)
        {
            Status status = child.Tick(teamIndex, tankIndex);
            if (status != Status.success) { return status; }
        }
        return Status.success;
    }

    public override void OnTerminate(Status status)
    {
        
    }
}
