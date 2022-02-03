using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BehaviorTree/Action/TankAction/FireMode")]
public class FireMode : Action
{
    public bool m_Fire;
    public BoolListVariable m_FireMode;
    public override void AddAITank(int teamIndex, int tankIndex = -1)
    {

    }

    public override Status BHUpdate(bool debugMode, int teamIndex, int tankIndex = -1)
    {
        m_FireMode.m_Values[tankIndex] = m_Fire;
        return Status.success;
    }

    public override void OnInitialize()
    {

    }

    public override void OnTerminate(Status status)
    {

    }

    public override void RemoveAITank(int teamIndex, int tankIndex = -1)
    {

    }
}
