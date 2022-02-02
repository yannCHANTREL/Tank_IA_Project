using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    public bool m_PathFound;
    public Vector3 m_TargetPos;
    public float m_RadiusTolerance = 1;

    [SerializeField]
    private NavigationManager m_NavManager;
        
    private List<Vector3> m_ListWayPoints;

    private void Update()
    {
        
    }

    public async void SearchPath(Vector3 posStart, Vector3 posEnd)
    {
        m_TargetPos = posEnd;
        m_PathFound = false;
        m_ListWayPoints = await m_NavManager.FindPath(posStart, posEnd);
        m_PathFound = true;
    }

    public void UpdatePath(Vector3 currentPos) 
    {
        double distance;
        for (int i = 0; i < m_ListWayPoints.Count; i++)
        {
            distance = Math.Sqrt(Math.Pow((m_ListWayPoints[i].x - currentPos.x), 2f) +
                                        Math.Pow((m_ListWayPoints[i].y - currentPos.y), 2f));
            if (distance < m_RadiusTolerance)
            {
                m_ListWayPoints.RemoveRange(0,i+1);
            }
        }
        if (m_ListWayPoints.Count == 0)
        {
            m_TargetPos = Vector3.zero;
            m_PathFound = false;
        }
    }

    public Vector3 GetActualWaypoint()
    {
        if (m_ListWayPoints != null && m_ListWayPoints.Count > 0) return m_ListWayPoints[0];
        return Vector3.zero;
    }
}
