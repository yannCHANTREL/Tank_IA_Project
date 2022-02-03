using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BehaviorTree/Action/TeamAction/TeamTargetSelection")]
public class TeamTargetSelection : Action
{
    public enum Policy { and, or };
    public enum SelectionPolicy { lowest, highest }
    public bool m_StrictSelection;
    public SelectionPolicy m_SelectionPolicy;
    public TeamTankIndexTargetListVariable m_TeamTankIndexTarget;
    public TargetSorter m_TargetSorter;
    public TeamList m_TeamList;
    public List<TestData> m_Tests;
    public Policy m_CompositionPolicy;


    [System.Serializable]
    public struct TestData { public TargetSelector m_TargetSelector; public bool m_Signs; }
    public override void AddAITank(int teamIndex, int tankIndex = -1)
    {

    }

    public override Status BHUpdate(bool debugMode, int teamIndex, int tankIndex = -1)
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
                if (Test(stepTankIndex))
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
        if (debugMode) Debug.Log("Strictly detected : " + strictlySelectedTankIndex + " | Detected :" + selectedTankIndex);
        if (selectedTankIndex == -1 || (m_StrictSelection && strictlySelectedTankIndex == -1)) return Status.failure;
        else
        {
            m_TeamTankIndexTarget.m_Values[teamIndex] = m_StrictSelection ? strictlySelectedTankIndex : selectedTankIndex;
            return Status.success;
        }
    }

    public bool Test(int tankIndex)
    {
        bool isAnd = m_CompositionPolicy == Policy.and;
        bool isOr = m_CompositionPolicy == Policy.or;
        for (int i = 0; i < m_Tests.Count; i++)
        {
            bool test = m_Tests[i].m_Signs == m_Tests[i].m_TargetSelector.Test(tankIndex);
            if (isAnd && !test) { return false; }
            if (isOr && test) { return true; }
        }
        return isAnd;
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
