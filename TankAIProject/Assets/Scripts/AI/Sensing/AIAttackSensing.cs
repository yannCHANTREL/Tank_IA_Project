using Complete;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackSensing : MonoBehaviour
{
    public TeamList m_TeamList;
    public SensedTankListVariable m_SensedTank;
    public TankIndexManager m_TankIndexManager;

    public float m_AttackMaxAngle = 20;
    public float m_AttackMaxDistance = 20;

    void Update()
    {
        AttackSensing();
    }

    private void AttackSensing()
    {
        Vector3 pos = transform.position;

        Team[] teams = m_TeamList.m_Teams;
        for (int i = 0; i < teams.Length; i++)
        {
            if (i != m_TankIndexManager.m_TeamIndex)
            {
                List<TankManager> teamTankManagers = teams[i].m_TeamTank;
                foreach (var tankManager in teamTankManagers)
                {
                    Transform otherTankTransform = tankManager.m_Instance.transform;
                    Vector3 distance = pos - otherTankTransform.position;
                    if (distance.magnitude < m_AttackMaxDistance && Vector3.Angle(otherTankTransform.forward, distance) < m_AttackMaxAngle)
                    {
                        m_SensedTank.m_AttackingTanks[m_TankIndexManager.m_TankIndex].Add(tankManager.m_Instance);
                    }
                }
            }
        }
    }
}
