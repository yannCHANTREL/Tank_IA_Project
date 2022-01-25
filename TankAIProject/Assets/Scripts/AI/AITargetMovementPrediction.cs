using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITargetMovementPrediction : MonoBehaviour
{
    public GameObject m_TankTarget;
    public Vector3ListVariable m_TankTargetPos;
    public Vector3ListVariable m_TankTargetLastPos;
    public Vector3ListVariable m_TankTargetSpeed;
    public Vector3ListVariable m_TankTargetLastSpeed;
    public Vector3ListVariable m_TankTargetEstimatedPos;
    public FloatListVariable m_TankTargetTimeToReachEstimatedPos;
    public FloatListVariable m_TankTargetAngularSpeed;
    public TankIndexManager m_TankIndexManager;

    public float m_ShellSpeed = 20;
    public float m_MaxStepAngle = 3.6f;
    public float m_MaxStepDist = 12;
    public int m_MaxEstimationStep = 1000;

    private Transform m_Transform;

    public void Awake()
    {
        m_Transform = transform;
    }

    public void FixedUpdate()
    {
        if (m_TankTarget) TrajectoryEstimationB(); 
    }

    public void TrajectoryEstimationB()
    {
        Vector3 tankTargetEstimatedPos;
        float dt = Time.fixedDeltaTime;
        Vector3 tankTargetLastPos = m_TankTargetPos.m_Values[m_TankIndexManager.m_TankIndex];
        Vector3 tankTargetPos = m_TankTarget.transform.position;
        Vector3 tankTargetDeltaPos = tankTargetPos - tankTargetLastPos;
        Vector3 tankTargetPenultimateSpeed = m_TankTargetLastSpeed.m_Values[m_TankIndexManager.m_TankIndex];
        Vector3 tankTargetLastSpeed = m_TankTargetSpeed.m_Values[m_TankIndexManager.m_TankIndex];
        Vector3 tankTargetSpeed = tankTargetDeltaPos / dt;
        float tankTargetAngleDir = Vector3.SignedAngle(tankTargetLastSpeed, tankTargetSpeed, Vector3.up);
        float tankTargetLastAngleDir = Vector3.SignedAngle(tankTargetPenultimateSpeed, tankTargetLastSpeed, Vector3.up);
        float tankTargetTimeToReachEstimatedPos;
        Vector3 tankPos = m_Transform.position;

        float stepDist = tankTargetDeltaPos.magnitude;
        float lastStepDist = tankTargetLastSpeed.magnitude * dt;
        float currentTankTargetEstimatedError;
        float currentTankTargetTravelTime = 0;
        float shellTravelTime;
        float bestTankTargetTimeEstimatedError = float.MaxValue;
        Vector3 tankTargetInitialDir = tankTargetDeltaPos.normalized;
        tankTargetEstimatedPos = tankTargetPos;
        float stepDeltaAngle = tankTargetAngleDir;
        float lastStepDeltaAngle = tankTargetLastAngleDir;
        float stepAngle = 0;
        float temp;

        for (int i = 0; i < m_MaxEstimationStep; i++)
        {
            temp = stepDeltaAngle;
            stepDeltaAngle = Mathf.Clamp(1.5f * stepDeltaAngle - 0.5f * lastStepDeltaAngle, -m_MaxStepAngle, m_MaxStepAngle);
            lastStepDeltaAngle = temp;
            stepAngle += stepDeltaAngle;
            tankTargetInitialDir = Quaternion.AngleAxis(stepDeltaAngle, Vector3.up) * tankTargetInitialDir;

            temp = stepDist;
            stepDist = Mathf.Clamp(1.5f * stepDist - 0.5f * lastStepDist, -m_MaxStepDist, m_MaxStepDist);
            lastStepDist = temp;

            Debug.DrawLine(tankTargetEstimatedPos + new Vector3(0f, 1f, 0f), tankTargetEstimatedPos + Quaternion.AngleAxis(stepDeltaAngle, Vector3.up) * tankTargetInitialDir * stepDist + new Vector3(0f, 1f, 0f), Color.magenta);
            tankTargetEstimatedPos += Quaternion.AngleAxis(stepDeltaAngle, Vector3.up) * tankTargetInitialDir * stepDist;
            currentTankTargetTravelTime += dt;
            shellTravelTime = (tankTargetEstimatedPos - tankPos).magnitude / m_ShellSpeed;
            currentTankTargetEstimatedError = Mathf.Abs(currentTankTargetTravelTime - shellTravelTime);

            if (currentTankTargetEstimatedError < bestTankTargetTimeEstimatedError)
            {
                bestTankTargetTimeEstimatedError = currentTankTargetEstimatedError;
            }
            else break;
        }

        tankTargetTimeToReachEstimatedPos = currentTankTargetTravelTime - dt;

        m_TankTargetLastPos.m_Values[m_TankIndexManager.m_TankIndex] = tankTargetLastPos;
        m_TankTargetPos.m_Values[m_TankIndexManager.m_TankIndex] = tankTargetPos;
        m_TankTargetSpeed.m_Values[m_TankIndexManager.m_TankIndex] = tankTargetSpeed;
        m_TankTargetLastSpeed.m_Values[m_TankIndexManager.m_TankIndex] = tankTargetLastSpeed;
        m_TankTargetEstimatedPos.m_Values[m_TankIndexManager.m_TankIndex] = tankTargetEstimatedPos;
        m_TankTargetTimeToReachEstimatedPos.m_Values[m_TankIndexManager.m_TankIndex] = tankTargetTimeToReachEstimatedPos;
    }
}
