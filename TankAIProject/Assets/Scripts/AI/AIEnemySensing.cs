using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEnemySensing : MonoBehaviour
{
    public GameObject m_TankTarget;
    public Vector3ListVariable m_TankTargetPos;
    public Vector3ListVariable m_TankTargetSpeed;
    public Vector3ListVariable m_TankTargetEstimatedPos;
    public Vector3ListVariable m_TankTargetLastPos;
    public TankIndexManager m_TankIndexManager;

    public float m_ShellSpeed = 20;

    public void FixedUpdate()
    {
        if (m_TankTarget)
        {
            Vector3 tankTargetLastPos = m_TankTargetPos.m_Values[m_TankIndexManager.m_TankIndex];
            Vector3 tankTargetPos = m_TankTarget.transform.position;
            Vector3 tankTargetVelocity = (tankTargetPos - tankTargetLastPos) / Time.fixedDeltaTime;
            Vector3 tankTargetDistance = tankTargetPos - transform.position;
            float tankTargetVelocityMagnitude = tankTargetVelocity.magnitude;
            float tankTargetDistanceMagnitude = tankTargetDistance.magnitude;
            float tankTargetAngleDir = Vector3.Angle(tankTargetDistance, tankTargetVelocity);

            float squareSpeedDiff = Mathf.Pow(m_ShellSpeed, 2f) - Mathf.Pow(tankTargetVelocityMagnitude, 2f);
            float squareTargetDistance = Mathf.Pow(tankTargetDistanceMagnitude, 2f);
            float speedDistanceCosAngle = tankTargetVelocityMagnitude * tankTargetDistanceMagnitude * Mathf.Cos(tankTargetAngleDir);
            float speedDistanceCosAngleDivSquareSpeedDiff = speedDistanceCosAngle / squareSpeedDiff;
            float shellTravelTime = squareSpeedDiff == 0 ? squareTargetDistance / 2f / speedDistanceCosAngle : Mathf.Sqrt(Mathf.Pow(speedDistanceCosAngleDivSquareSpeedDiff, 2f) + squareTargetDistance / squareSpeedDiff) - speedDistanceCosAngleDivSquareSpeedDiff;
            Vector3 tankTargetEstimatedPos = tankTargetPos + tankTargetVelocity * shellTravelTime;

            m_TankTargetPos.m_Values[m_TankIndexManager.m_TankIndex] = tankTargetPos;
            m_TankTargetSpeed.m_Values[m_TankIndexManager.m_TankIndex] = tankTargetVelocity;
            m_TankTargetEstimatedPos.m_Values[m_TankIndexManager.m_TankIndex] = tankTargetEstimatedPos;
            m_TankTargetLastPos.m_Values[m_TankIndexManager.m_TankIndex] = tankTargetLastPos;
        }
    }
}
