using System.Collections;
using System.Collections.Generic;
using Complete;
using UnityEngine;

public class AITeamEnemySensing : MonoBehaviour
{
    public SensedTankListVariable m_SensedTank;
    public TeamList m_TeamList;
    
    void Update()
    {
        List<List<GameObject>> tankSensedEnemiesList = m_SensedTank.m_TankSensedEnemies;
        List<List<GameObject>> teamSensedEnemiesList = m_SensedTank.m_TeamSensedEnemies;
        for (int i = 0; i < teamSensedEnemiesList.Count; i++)
        {
            teamSensedEnemiesList[i] = new List<GameObject>();
        }
        for (int i = 0; i < m_TeamList.m_Teams.Length; i++)
        {
            List<TankManager> team = m_TeamList.m_Teams[i].m_TeamTank;
            foreach (var tankManager in team)
            {
                List<GameObject> tankSensedEnemies = tankSensedEnemiesList[tankManager.m_Instance.GetComponent<TankIndexManager>().m_TankIndex];
                List<GameObject> teamSensedEnemies = teamSensedEnemiesList[i];
                for (int k = 0; k < tankSensedEnemies.Count; k++)
                {
                    GameObject sensedEnemy = tankSensedEnemies[k];
                    
                    if (!teamSensedEnemies.Contains(sensedEnemy))
                    {
                        teamSensedEnemies.Add(sensedEnemy);
                    }
                }
            }
        }
    }
}
