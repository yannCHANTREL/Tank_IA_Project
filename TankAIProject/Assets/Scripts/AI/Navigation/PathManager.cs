using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    [SerializeField]
    private NavigationManager m_NavManager;

    [SerializeField]
    private VirtualGrid m_Grid;
        
    private List<Vector3> m_ListWayPoints;

    public async void SearchPath(Vector3 posStart, Vector3 posEnd)
    {
        m_ListWayPoints = await m_NavManager.FindPath(posStart, posEnd);
    }

    public void UpdatePath(Vector3 currentPos) 
    {
        double radius = m_Grid.nodeDiameter / 2;
        double distance;
        for (int i = 0; i < m_ListWayPoints.Count; i++)
        {
            distance = Math.Sqrt(Math.Pow((m_ListWayPoints[i].x - currentPos.x), 2f) +
                                        Math.Pow((m_ListWayPoints[i].y - currentPos.y), 2f));
            if (distance < radius)
            {
                m_ListWayPoints.RemoveRange(0,i+1);
            }
        }
    }

    public Vector3 GetActualWaypoint()
    {
        if (m_ListWayPoints != null && m_ListWayPoints.Count > 0) return m_ListWayPoints[0];
        return Vector3.zero;
    }
}
