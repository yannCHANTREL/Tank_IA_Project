using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct IntList
{
    public List<int> m_List;

    public IntList(List<int> list)
    {
        m_List = list;
    }

    public IntList(int listSize)
    {
        m_List = new List<int>(listSize);
        for (int i = 0; i < listSize; i++) { m_List.Add(0); }
    }


    public void Reset()
    {
        m_List.Clear();
    }
}

[CreateAssetMenu(menuName = "Variables/TargetCountList")]
public class TargetCountListVariable : ListVariable
{
    public List<IntList> m_Values;
    private int teamSize;

    public override void IncrementTankSize()
    {
        m_Values.Add(new IntList(teamSize));
    }

    public override void IncrementTeamSize()
    {
        teamSize++;
        foreach (var tankTargetCountList in m_Values)
        {
            tankTargetCountList.m_List.Add(0);
        }
    }

    public override void Reset()
    {
        teamSize = 0;
        m_Values = new List<IntList>();
    }
}
