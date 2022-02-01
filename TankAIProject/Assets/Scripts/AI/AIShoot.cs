using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShoot : MonoBehaviour
{
    public BoolListVariable m_FireMode;
    public Vector3ListVariable m_TargetEstimatedPos;
    public FloatListVariable m_TargetTimeToReachEstimatedPos;
    public float m_FireRange = 13;
    public float m_AngularTolerance = 5f;
    public float m_RadiusTolerance = 0.5f;
    public float m_ShellSpeed = 20;
    public TankIndexManager m_TankIndexManager;
    public TankEvent m_FireCommand;

   
    void Update()
    {
        int tankIndex = m_TankIndexManager.m_TankIndex;
        if (m_FireMode.m_Values[tankIndex]) { Fire(tankIndex); }
    }

    public void Fire(int tankIndex)
    {
        Vector3 tankPos = transform.position;
        Vector3 tankForward = transform.forward;
        Vector3 targetPos = m_TargetEstimatedPos.m_Values[tankIndex];
        Vector3 distance = targetPos - tankPos;
        float angleToTarget = Vector3.Angle(distance, tankForward);
        Debug.DrawLine(tankPos + new Vector3(0f,1f,0f), new Vector3(0f,1f,0f) + tankPos + tankForward * m_ShellSpeed * m_TargetTimeToReachEstimatedPos.m_Values[tankIndex], Color.blue);
        if (distance.magnitude < m_FireRange + m_RadiusTolerance && angleToTarget < m_AngularTolerance && (distance - tankForward * m_ShellSpeed * m_TargetTimeToReachEstimatedPos.m_Values[tankIndex]).magnitude < m_RadiusTolerance)
        {
            m_FireCommand.Raise(tankIndex);
        }
    }
}
