using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShoot : MonoBehaviour
{
    
    public Vector3 m_TargetPos;
    public float m_FireRange = 13;
    public float m_AngularTolerance = 5f;
    public float m_RadiusTolerance = 0.5f;
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
        Vector3 distance = m_TargetPos - tankPos;
        float angleToTarget = Vector3.Angle(distance, tankForward);
        
        if (distance.magnitude < m_FireRange + m_RadiusTolerance && angleToTarget < m_AngularTolerance)
        {
            print(distance.magnitude + "    " + angleToTarget);
            m_FireCommand.Raise(m_TankIndexManager.m_TankIndex);
        }
    }
}
