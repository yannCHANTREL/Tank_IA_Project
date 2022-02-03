using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BehaviorTree/Action/TeamAction/TeamTargetSelection")]
public class TeamTargetSelection : Action
{
    public enum Policy { and, or };
    public enum SelectionPolicy { lowest, highest }

    public bool m_StrictSelection;
    public TeamTankIndexTargetListVariable m_TeamTankIndexTarget;
    public TeamList m_TeamList;
    public List<SortingData> m_SortingDatas;
    public List<SelectionData> m_SelectionDatas;
    public Policy m_CompositionPolicy;


    [System.Serializable]
    public struct SelectionData { public TargetSelector m_TargetSelector; public bool m_Signs; }

    [System.Serializable]
    public struct SortingData { public TargetSorter m_TargetSorter; public SelectionPolicy m_SelectionPolicy; }
    public override void AddAITank(int teamIndex, int tankIndex = -1)
    {

    }

    public override Status BHUpdate(bool debugMode, int teamIndex, int tankIndex = -1)
    {
        int strictlySelectedTankIndex = -1;
        int selectedTankIndex = -1;

        //float strictScore = m_SelectionPolicy == SelectionPolicy.lowest ? float.MaxValue : float.MinValue;
        //float score = m_SelectionPolicy == SelectionPolicy.lowest ? float.MaxValue : float.MinValue;

        List<float> strictScores = new List<float>();
        List<float> scores = new List<float>();
        List<float> stepScores = new List<float>();

        int SortingDataCount = m_SortingDatas.Count;
        for (int i = 0; i < SortingDataCount; i++)
        {
            float initialScore = m_SortingDatas[i].m_SelectionPolicy == SelectionPolicy.lowest ? float.MaxValue : float.MinValue;
            strictScores.Add(initialScore);
            scores.Add(initialScore);
            stepScores.Add(0);
        }

        foreach (var tankManager in m_TeamList.m_Teams[teamIndex].m_TeamTank)
        {
            int stepTankIndex = tankManager.m_Instance.GetComponent<TankIndexManager>().m_TankIndex;
            for (int i = 0; i < SortingDataCount; i++)
            {
                stepScores[i] = m_SortingDatas[i].m_TargetSorter.ScoreCalculation(stepTankIndex);
            }

            //float stepScore = m_TargetSorter.ScoreCalculation(stepTankIndex);

            if (debugMode)
            {
                Debug.Log("New tank : " + ListToString(stepScores));
            }


            for (int i = 0; i < SortingDataCount; i++)
            {
                SelectionPolicy selectionPolicy = m_SortingDatas[i].m_SelectionPolicy;
                bool lowestPolicy = selectionPolicy == SelectionPolicy.lowest;
                bool highestPolicy = selectionPolicy == SelectionPolicy.highest;
                bool selectionTest = Test(stepTankIndex);
                if (selectionTest)
                {
                    if (((lowestPolicy && stepScores[i] < strictScores[i]) || (highestPolicy && stepScores[i] > strictScores[i])))
                    {
                        if (debugMode) Debug.Log("Valided as strictly best tank");
                        CopyList(ref strictScores, stepScores);
                        strictlySelectedTankIndex = stepTankIndex;
                        if ((lowestPolicy && stepScores[i] < scores[i]) || (highestPolicy && stepScores[i] > scores[i]))
                        {
                            if (debugMode) Debug.Log("Valided as best tank");
                            CopyList(ref scores, stepScores);
                            selectedTankIndex = stepTankIndex;
                        }
                        break;
                    }
                    else if (stepScores[i] > strictScores[i])
                    {
                        break;
                    }
                }
                else
                {
                    if ((lowestPolicy && stepScores[i] < scores[i]) || (highestPolicy && stepScores[i] > scores[i]))
                    {
                        CopyList(ref scores, stepScores);
                        selectedTankIndex = stepTankIndex;
                        break;
                    }
                    else if ((lowestPolicy && stepScores[i] > scores[i]) || (highestPolicy && stepScores[i] < scores[i]))
                    {
                        break;
                    }
                }
            }


            /*if ((m_SelectionPolicy == SelectionPolicy.lowest && stepScore < strictScore) || (m_SelectionPolicy == SelectionPolicy.highest && stepScore > strictScore))
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
            }*/
        }
        if (debugMode) Debug.Log("Strictly detected : " + strictlySelectedTankIndex + " | Detected :" + selectedTankIndex);
        if (selectedTankIndex == -1 || (m_StrictSelection && strictlySelectedTankIndex == -1)) return Status.failure;
        else
        {
            m_TeamTankIndexTarget.m_Values[teamIndex] = !m_StrictSelection && strictlySelectedTankIndex == -1 ? selectedTankIndex : strictlySelectedTankIndex;
            return Status.success;
        }
    }

    public bool Test(int tankIndex)
    {
        bool isAnd = m_CompositionPolicy == Policy.and;
        bool isOr = m_CompositionPolicy == Policy.or;
        for (int i = 0; i < m_SelectionDatas.Count; i++)
        {
            bool test = m_SelectionDatas[i].m_Signs == m_SelectionDatas[i].m_TargetSelector.Test(tankIndex);
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

    public string ListToString(List<float> list)
    {
        string outString = "";
        foreach (var val in list)
        {
            outString += val + " | ";
        }
        return outString;
    }

    public void CopyList(ref List<float> toList, List<float> fromList)
    {
        for (int i = 0; i < fromList.Count; i++)
        {
            toList[i] = fromList[i];
        }
    }
}
