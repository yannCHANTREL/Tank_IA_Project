using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShoot : MonoBehaviour
{
    public Vector3ListVariable m_TargetPos;
    public Vector3ListVariable m_TargetEstimatedPos;
    public FloatListVariable m_TargetTimeToReachEstimatedPos;
    public bool m_PredictTargetMovement = true;
    public float m_FireRange = 13;
    public float m_AngularTolerance = 5f;
    public float m_RadiusTolerance = 0.5f;
    public float m_ShellSpeed = 20;
    public TankIndexManager m_TankIndexManager;
    public TankEvent m_FireCommand;

    // Update is called once per frame
    void Update()
    {
        Fire();
    }

    public void Fire()
    {
        Vector3 tankPos = transform.position;
        Vector3 tankForward = transform.forward;
        Vector3 targetPos = m_PredictTargetMovement ? m_TargetEstimatedPos.m_Values[m_TankIndexManager.m_TankIndex] : m_TargetPos.m_Values[m_TankIndexManager.m_TankIndex];
        Vector3 distance = targetPos - tankPos;
        float angleToTarget = Vector3.Angle(distance, tankForward);
        
        if (distance.magnitude < m_FireRange + m_RadiusTolerance && angleToTarget < m_AngularTolerance && (distance - tankForward * m_ShellSpeed * m_TargetTimeToReachEstimatedPos.m_Values[m_TankIndexManager.m_TankIndex]).magnitude < m_RadiusTolerance)
        {
            m_FireCommand.Raise(m_TankIndexManager.m_TankIndex);
        }
    }
}
