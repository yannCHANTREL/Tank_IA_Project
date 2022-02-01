using Complete;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackSensing : MonoBehaviour
{
    public TeamList m_TeamList;
    public SensedTankListVariable m_SensedTank;
    public TankIndexManager m_TankIndexManager;
    public PointVariable m_CapturePoint;

    public float m_AttackMaxAngle = 20;
    public float m_AttackMaxDistance = 20;

    private int m_TankIndex;

    private void Start()
    {
        m_TankIndex = m_TankIndexManager.m_TankIndex;
    }

    void Update()
    {
        AttackSensing();
    }

    private void AttackSensing()
    {
        Vector3 tankPos = transform.position;
        Vector3 CapturePos = m_CapturePoint.m_CenterPos;

        Team[] teams = m_TeamList.m_Teams;
        for (int i = 0; i < teams.Length; i++)
        {
            if (i != m_TankIndex)
            {
                m_SensedTank.m_AttackingTanks[m_TankIndex] = new List<GameObject>();
                m_SensedTank.m_EnemyTanksOnCapturePoint[m_TankIndex] = new List<GameObject>();
                List<TankManager> teamTankManagers = teams[i].m_TeamTank;
                foreach (var tankManager in teamTankManagers)
                {
                    Transform otherTankTransform = tankManager.m_Instance.transform;

                    Vector3 distanceToTank = tankPos - otherTankTransform.position;
                    if (distanceToTank.magnitude < m_AttackMaxDistance && Vector3.Angle(otherTankTransform.forward, distanceToTank) < m_AttackMaxAngle)
                    {
                        m_SensedTank.m_AttackingTanks[m_TankIndex].Add(tankManager.m_Instance);
                    }

                    Vector3 distanceToCapturePoint = CapturePos - otherTankTransform.position;
                    if (distanceToCapturePoint.magnitude < m_CapturePoint.m_Radius)
                    {
                        m_SensedTank.m_EnemyTanksOnCapturePoint[m_TankIndex].Add(tankManager.m_Instance);
                    }
                }
            }
        }
    }
}
