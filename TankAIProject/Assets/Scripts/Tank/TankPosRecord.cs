using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPosRecord : MonoBehaviour
{
    public Vector3ListVariable m_TankPos;
    public TankIndexManager m_TankIndexManager;
    private int m_TankIndex;

    private void Start()
    {
        m_TankIndex = m_TankIndexManager.m_TankIndex;
    }

    void Update()
    {
        m_TankPos.m_Values[m_TankIndex] = transform.position;
    }
}
