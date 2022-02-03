using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BehaviorTree/Action/TeamAction/TeamTankMode")]
public class TeamTankMode : Action
{
    public Mode m_ModeToSet;
    public TeamTankIndexTargetListVariable m_TeamTankTarget;
    public TankModeListVariable m_TankMode;
    public override void AddAITank(int teamIndex, int tankIndex = -1)
    {

    }

    public override Status BHUpdate(bool debugMode, int teamIndex, int tankIndex = -1)
    {
        m_TankMode.m_Values[m_TeamTankTarget.m_Values[teamIndex]] = m_ModeToSet;
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
