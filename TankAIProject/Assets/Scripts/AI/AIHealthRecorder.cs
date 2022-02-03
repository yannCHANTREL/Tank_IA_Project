using Complete;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealthRecorder : MonoBehaviour
{
    public FloatListVariable m_TankHealth;
    public TeamList m_TeamList;

    void Update()
    {
        foreach (var team in m_TeamList.m_Teams)
        {
            foreach (var tankManager in team.m_TeamTank)
            {
                GameObject tank = tankManager.m_Instance;
                m_TankHealth.m_Values[tank.GetComponent<TankIndexManager>().m_TankIndex] = tank.GetComponent<TankHealth>().m_CurrentHealth;
            }
        }
    }
}
