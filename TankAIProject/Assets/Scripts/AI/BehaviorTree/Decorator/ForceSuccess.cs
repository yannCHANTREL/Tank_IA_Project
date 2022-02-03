using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BehaviorTree/Decorator/ForceSuccess")]
public class ForceSuccess : Decorator
{
    public override void OnInitialize()
    {

    }

    public override Status BHUpdate(bool debugMode, int teamIndex, int tankIndex = -1)
    {
        Status status = m_Child.Tick(debugMode, teamIndex, tankIndex);
        if (status == Status.failure) return Status.success;
        return status;
    }

    public override void OnTerminate(Status status)
    {

    }
}
