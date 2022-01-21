using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEnemySensing : MonoBehaviour
{
    public GameObject m_TankTarget;
    public Vector3ListVariable m_FireTarget;
    public TankIndexManager m_TankIndexManager;

    public void Update()
    {
        if (m_TankTarget) { m_FireTarget.m_Values[m_TankIndexManager.m_TankIndex] = m_TankTarget.transform.position; }
    }
}
