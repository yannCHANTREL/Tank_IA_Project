using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/TeamTankIndexTargetList")]
public class TeamTankIndexTargetListVariable : ListVariable
{
    public List<int> m_Values;
    public override void IncrementTankSize()
    {
        
    }

    public override void IncrementTeamSize()
    {
        m_Values.Add(0);
    }

    public override void Reset()
    {
        m_Values = new List<int>();
    }
}
