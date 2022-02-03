using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BehaviorTree/Action/TeamAction/TeamMode")]
public class TeamMode : Action
{
    public Mode m_ModeToSet;
    public TeamList m_TeamList;
    public TankModeListVariable m_TankMode;
    public override void AddAITank(int teamIndex, int tankIndex = -1)
    {

    }

    public override Status BHUpdate(bool debugMode, int teamIndex, int tankIndex = -1)
    {
        List<Mode> tankModes = m_TankMode.m_Values;
        foreach (var tankManager in m_TeamList.m_Teams[teamIndex].m_TeamTank)
        {
            tankModes[tankManager.m_Instance.GetComponent<TankIndexManager>().m_TankIndex] = m_ModeToSet;
        }
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
