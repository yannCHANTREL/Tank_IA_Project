using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BehaviorTree/Decorator/Invert")]
public class Invert : Decorator
{
    public override void OnInitialize()
    {

    }

    public override Status BHUpdate(bool debugMode, int teamIndex, int tankIndex = -1)
    {
        Status status = m_Child.Tick(debugMode, teamIndex, tankIndex);
        switch (status)
        {
            case Status.success:
                return Status.failure;
            case Status.failure:
                return Status.success;
            default:
                return status;
        }
    }

    public override void OnTerminate(Status status)
    {

    }
}
