using System.Collections;
using System.Collections.Generic;
using Complete;
using UnityEngine;

[CreateAssetMenu(menuName = "Team Tank List")]
public class TeamTankList : ScriptableObject
{
    public int m_TeamNumber;
    public Color m_TeamColor;
    
    public List<TankManager> m_TeamTank;
}
