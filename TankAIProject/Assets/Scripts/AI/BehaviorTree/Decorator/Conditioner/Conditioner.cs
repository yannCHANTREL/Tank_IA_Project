using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BehaviorTree/Decorator/Conditioner")]
public class Conditioner : Decorator
{
    public Condition m_condition;
    public override void OnInitialize()
    {

    }

    public override Status BHUpdate(int tankIndex)
    {
        if (m_condition.Test(tankIndex))
        {
            return m_Child.Tick(tankIndex);
        }
        else
        {
            return Status.failure;
        }
    }

    public override void OnTerminate(Status status)
    {

    }
}
