using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BehaviorTree/Composite/Selector")]
public class Selector : Composite
{
    public override void OnInitialize()
    {

    }

    public override Status BHUpdate(bool debugMode, int teamIndex, int tankIndex = -1)
    {
        foreach (var child in m_Children)
        {
            Status status = child.Tick(debugMode, teamIndex, tankIndex);
            if (status != Status.failure) { return status; }
        }
        return Status.failure;
    }

    public override void OnTerminate(Status status)
    {

    }
}