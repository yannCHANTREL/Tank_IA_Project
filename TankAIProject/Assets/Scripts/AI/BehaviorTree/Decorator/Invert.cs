using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BehaviorTree/Decorator/Invert")]
public class Invert : Decorator
{
    public override void OnInitialize()
    {

    }

    public override Status BHUpdate(int teamIndex, int tankIndex = 0)
    {
        Status status = m_Child.Tick(teamIndex, tankIndex);
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
