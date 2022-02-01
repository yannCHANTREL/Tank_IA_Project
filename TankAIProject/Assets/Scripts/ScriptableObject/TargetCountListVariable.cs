using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/TargetCountList")]
public class TargetCountListVariable : GenericListVariable<List<int>>
{
    public override void IncrementTankSize()
    {
        if (m_Values.Count == 0) { m_Values.Add(new List<int>(1)); }
        else
        {
            m_Values.Add(new List<int>(m_Values[0].Count));
        }
    }

    public override void IncrementTeamSize()
    {
        foreach (var tankTargetCountList in m_Values)
        {
            tankTargetCountList.Add(0);
        }
    }
}
