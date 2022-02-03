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

    public bool m_DebugMode = false;

    private int m_TankIndex;
    private int m_TeamIndex;

    private void Start()
    {
        m_TankIndex = m_TankIndexManager.m_TankIndex;
        m_TeamIndex = m_TankIndexManager.m_TeamIndex;
    }

    void Update()
    {
        AttackSensing();
    }

    private void AttackSensing()
    {
        Vector3 tankPos = transform.position;
        Vector3 capturePos = m_CapturePoint.m_CenterPos;

        /*Team[] teams = m_TeamList.m_Teams;
        for (int i = 0; i < teams.Length; i++)
        {
            if (i != m_TeamIndex)
            {
                m_SensedTank.m_AttackingTanks[m_TankIndex].Reset();
                m_SensedTank.m_EnemyTanksOnCapturePoint[m_TankIndex].Reset();
                List<TankManager> teamTankManagers = teams[i].m_TeamTank;
                foreach (var tankManager in teamTankManagers)
                {
                    Transform otherTankTransform = tankManager.m_Instance.transform;
                    Vector3 otherTankForward = otherTankTransform.forward;
                    Vector3 otherPosition = otherTankTransform.position;

                    Color debugColor = Color.green;

                    Vector3 distanceToTank = tankPos - otherPosition;
                    if (distanceToTank.magnitude < m_AttackMaxDistance && Vector3.Angle(otherTankForward, distanceToTank) < m_AttackMaxAngle)
                    {
                        debugColor = Color.red;
                        m_SensedTank.m_AttackingTanks[m_TankIndex].m_List.Add(tankManager.m_Instance);
                    }

                    if (m_DebugMode) { DisplayDetectionBounds(otherTankForward, otherPosition, debugColor); }

                    Vector3 distanceToCapturePoint = capturePos - otherPosition;
                    if (distanceToCapturePoint.magnitude < m_CapturePoint.m_Radius)
                    {
                        m_SensedTank.m_EnemyTanksOnCapturePoint[m_TankIndex].m_List.Add(tankManager.m_Instance);
                    }
                }
            }
        }*/

        m_SensedTank.m_AttackingTanks[m_TankIndex].Reset();
        m_SensedTank.m_EnemyTanksOnCapturePoint[m_TankIndex].Reset();
        foreach (var tank in m_SensedTank.m_TeamSensedEnemies[m_TeamIndex].m_List)
        {
            Transform otherTankTransform = tank.transform;
            Vector3 otherTankForward = otherTankTransform.forward;
            Vector3 otherPosition = otherTankTransform.position;

            Color debugColor = Color.green;

            Vector3 distanceToTank = tankPos - otherPosition;
            if (distanceToTank.magnitude < m_AttackMaxDistance && Vector3.Angle(otherTankForward, distanceToTank) < m_AttackMaxAngle)
            {
                debugColor = Color.red;
                m_SensedTank.m_AttackingTanks[m_TankIndex].m_List.Add(tank);
            }

            if (m_DebugMode) { DisplayDetectionBounds(otherTankForward, otherPosition, debugColor); }

            Vector3 distanceToCapturePoint = capturePos - otherPosition;
            if (distanceToCapturePoint.magnitude < m_CapturePoint.m_Radius)
            {
                m_SensedTank.m_EnemyTanksOnCapturePoint[m_TankIndex].m_List.Add(tank);
            }
        } 
    }

    private void DisplayDetectionBounds(Vector3 otherTankForward, Vector3 otherPosition, Color debugColor)
    {
        Vector3 scaledForward = otherTankForward * m_AttackMaxDistance;
        Vector3 Axis1 = Quaternion.AngleAxis(m_AttackMaxAngle, Vector3.up) * scaledForward;
        Vector3 Axis2 = Quaternion.AngleAxis(-m_AttackMaxAngle, Vector3.up) * scaledForward;
        Debug.DrawLine(otherPosition, otherPosition + Axis1, debugColor);
        Debug.DrawLine(otherPosition, otherPosition + Axis2, debugColor);
        Debug.DrawLine(otherPosition + Axis1, otherPosition + scaledForward, debugColor);
        Debug.DrawLine(otherPosition + scaledForward, otherPosition + Axis2, debugColor);
    }
}
