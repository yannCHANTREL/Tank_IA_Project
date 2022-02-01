using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BehaviorTree/Action/TeamAction/TeamTargetSelection")]
public class TeamTargetSelection : Action
{
    public enum SelectionPolicy { lowest, highest }
    public bool m_StrictSelection;
    public SelectionPolicy m_SelectionPolicy;
    public TeamTankIndexTargetList m_TeamTankIndexTarget;
    public TargetSorter m_TargetSorter;
    public TargetSelector m_TargetSelector;
    public bool m_TestSign;
    public TeamList m_TeamList;

    public override void AddAITank(int teamIndex, int tankIndex = 0)
    {

    }

    public override Status BHUpdate(int teamIndex, int tankIndex = 0)
    {
        int strictlySelectedTankIndex = -1;
        int selectedTankIndex = -1;

        float strictScore = m_SelectionPolicy == SelectionPolicy.lowest ? float.MaxValue : float.MinValue;
        float score = m_SelectionPolicy == SelectionPolicy.lowest ? float.MaxValue : float.MinValue;
        foreach (var tankManager in m_TeamList.m_Teams[teamIndex].m_TeamTank)
        {
            int stepTankIndex = tankManager.m_Instance.GetComponent<TankIndexManager>().m_TankIndex;
            float stepScore = m_TargetSorter.ScoreCalculation(stepTankIndex);

            if ((m_SelectionPolicy == SelectionPolicy.lowest && stepScore < strictScore) || (m_SelectionPolicy == SelectionPolicy.highest && stepScore > strictScore))
            {
                if (m_TargetSelector.Test(stepTankIndex) == m_TestSign)
                {
                    strictScore = stepScore;
                    strictlySelectedTankIndex = stepTankIndex;
                }
                score = stepScore;
                selectedTankIndex = stepTankIndex;
            }
            else if ((m_SelectionPolicy == SelectionPolicy.lowest && stepScore < score) || (m_SelectionPolicy == SelectionPolicy.highest && stepScore > score))
            {
                score = stepScore;
                selectedTankIndex = stepTankIndex;
            }
        }
        if (selectedTankIndex == -1) return Status.failure;
        else
        {
            m_TeamTankIndexTarget.m_Values[teamIndex] = strictlySelectedTankIndex != -1 ? strictlySelectedTankIndex : selectedTankIndex;
            return Status.success;
        }
    }

    public override void OnInitialize()
    {

    }

    public override void OnTerminate(Status status)
    {

    }

    public override void RemoveAITank(int teamIndex, int tankIndex = 0)
    {

    }
}
