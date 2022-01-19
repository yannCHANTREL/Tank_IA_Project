using System.Collections;
using System.Collections.Generic;
using Complete;
using UnityEngine;

[CreateAssetMenu(menuName = "Tank List")]
public class TeamList : ScriptableObject
{
    public List<TeamTankList> m_Teams;

    public void AddTank(TeamTankList tank)
    {
        m_Teams.Add(tank);
    }
}
