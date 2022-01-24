using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEnemySensing : MonoBehaviour
{
    public GameObject m_TankTarget;
    public Vector3ListVariable m_TankTargetPos;
    public Vector3ListVariable m_TankTargetLastPos;
    public Vector3ListVariable m_TankTargetSpeed;
    public Vector3ListVariable m_TankTargetLastSpeed;
    public Vector3ListVariable m_TankTargetEstimatedPos;
    public FloatListVariable m_TankTargetTimeToReachEstimatedPos;
    public TankIndexManager m_TankIndexManager;

    public float m_ShellSpeed = 20;
    public bool m_StraightEstimation;
    public int m_MaxEstimationStep = 1000;

    public void FixedUpdate()
    {
        if (m_TankTarget)
        {
            Vector3 tankTargetEstimatedPos;
            float dt = Time.fixedDeltaTime;
            Vector3 tankTargetLastPos = m_TankTargetPos.m_Values[m_TankIndexManager.m_TankIndex];
            Vector3 tankTargetPos = m_TankTarget.transform.position;
            Vector3 tankTargetDeltaPos = tankTargetPos - tankTargetLastPos;
            Vector3 tankTargetLastSpeed = m_TankTargetSpeed.m_Values[m_TankIndexManager.m_TankIndex];
            Vector3 tankTargetSpeed = tankTargetDeltaPos / dt;
            Vector3 tankPos = transform.position;
            Vector3 tankTargetDistance = tankTargetPos - tankPos;
            float tankTargetVelocityMagnitude = tankTargetSpeed.magnitude;
            float tankDistanceMagnitude = tankTargetDistance.magnitude;
            float tankTargetAngleDir = Vector3.SignedAngle(tankTargetDistance, tankTargetSpeed, Vector3.up);
            Vector3 tankTargetDir = Vector3.Dot(tankTargetSpeed, m_TankTarget.transform.forward) >= 0 ? m_TankTarget.transform.forward : -m_TankTarget.transform.forward;
            //float tankTargetLastAngleForward = Vector3.SignedAngle(tankTargetSpeed, tankTargetDir, Vector3.up);
            float tankTargetLastAngleForward = Vector3.SignedAngle(tankTargetLastSpeed, tankTargetSpeed, Vector3.up);
            float tankTargetTimeToReachEstimatedPos;

            if (m_StraightEstimation)
            {
                float squareSpeedDiff = Mathf.Pow(m_ShellSpeed, 2f) - Mathf.Pow(tankTargetVelocityMagnitude, 2f);
                float squareTargetDistance = Mathf.Pow(tankDistanceMagnitude, 2f);
                float speedDistanceCosAngle = tankTargetVelocityMagnitude * tankDistanceMagnitude * Mathf.Cos(tankTargetAngleDir);
                float speedDistanceCosAngleDivSquareSpeedDiff = speedDistanceCosAngle / squareSpeedDiff;
                float shellTravelTime = squareSpeedDiff == 0 ? squareTargetDistance / 2f / speedDistanceCosAngle : Mathf.Sqrt(Mathf.Pow(speedDistanceCosAngleDivSquareSpeedDiff, 2f) + squareTargetDistance / squareSpeedDiff) - speedDistanceCosAngleDivSquareSpeedDiff;
                tankTargetEstimatedPos = tankTargetPos + tankTargetSpeed * shellTravelTime;
                tankTargetTimeToReachEstimatedPos = shellTravelTime;
            }
            else
            {
                float currentTankTargetEstimatedError;
                float currentTankTargetTravelTime = 0;
                float shellTravelTime;
                float bestTankTargetTimeEstimatedError = float.MaxValue;
                Vector3 tankTargetStepEstimatedDeltaPos = tankTargetDeltaPos;
                tankTargetEstimatedPos = tankTargetPos;

                for (int i = 0; i < m_MaxEstimationStep; i++)
                {
                    tankTargetStepEstimatedDeltaPos = Quaternion.AngleAxis(tankTargetLastAngleForward, Vector3.up) * tankTargetStepEstimatedDeltaPos;
                    Debug.DrawLine(tankTargetEstimatedPos + new Vector3(0f,1f,0f), tankTargetEstimatedPos + tankTargetStepEstimatedDeltaPos + new Vector3(0f,1f,0f), Color.magenta);
                    tankTargetEstimatedPos += tankTargetStepEstimatedDeltaPos;
                    currentTankTargetTravelTime += dt;
                    shellTravelTime = (tankTargetEstimatedPos - tankPos).magnitude / m_ShellSpeed;
                    currentTankTargetEstimatedError = Mathf.Abs(currentTankTargetTravelTime - shellTravelTime);
                    
                    if (currentTankTargetEstimatedError < bestTankTargetTimeEstimatedError)
                    {
                        bestTankTargetTimeEstimatedError = currentTankTargetEstimatedError;
                    }
                    else { break; }
                }
                
                tankTargetTimeToReachEstimatedPos = currentTankTargetTravelTime - dt;
            }

            m_TankTargetLastPos.m_Values[m_TankIndexManager.m_TankIndex] = tankTargetLastPos;
            m_TankTargetPos.m_Values[m_TankIndexManager.m_TankIndex] = tankTargetPos;
            m_TankTargetLastSpeed.m_Values[m_TankIndexManager.m_TankIndex] = tankTargetLastSpeed;
            m_TankTargetSpeed.m_Values[m_TankIndexManager.m_TankIndex] = tankTargetSpeed;
            m_TankTargetEstimatedPos.m_Values[m_TankIndexManager.m_TankIndex] = tankTargetEstimatedPos;
            m_TankTargetTimeToReachEstimatedPos.m_Values[m_TankIndexManager.m_TankIndex] = tankTargetTimeToReachEstimatedPos;
        }
    }
}
