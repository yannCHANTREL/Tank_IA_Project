using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Virtual Grid")]
public class VirtualGrid : ScriptableObject
{

    public Vector3 m_GridTransformPosition;
    public LayerMask m_UnwalkableLayerMask;
    public int m_GridWorldSize;
    public int m_NbNode;
    public BoxCollider m_PrefabTankCollider;

    private int[,] m_Grid;
    private float m_NodeDiameter;
    private float m_NodeRadius;
    private int m_GridSize;
    private Vector3 m_WorldBottomLeft;
    private float m_tankDiameter;

    public void CreateGrid()
    {
        m_NodeDiameter = (float) m_GridWorldSize / m_NbNode;
        m_NodeRadius = m_NodeDiameter / 2;

        m_GridSize = Mathf.RoundToInt(m_GridWorldSize / m_NodeDiameter);

        m_WorldBottomLeft = m_GridTransformPosition - Vector3.right * m_GridWorldSize / 2 - Vector3.forward * m_GridWorldSize / 2;
        m_tankDiameter = Mathf.Sqrt(Mathf.Pow(m_PrefabTankCollider.size.x, 2) + Mathf.Pow(m_PrefabTankCollider.size.z, 2));
        
        m_Grid = new int[m_GridSize, m_GridSize];
       
        for (int x = 0; x < m_GridSize; x++)
        {
            for (int y = 0; y < m_GridSize; y++)
            {
                Vector3 worldPoint = GetWorldPositionByIndex(x, y);
                
                bool walkable = !Physics.CheckSphere(worldPoint, m_tankDiameter - m_NodeRadius, m_UnwalkableLayerMask);
                
                m_Grid[x, y] = (walkable ? 0 : -1);
                // Verif char à faire
            }
        }
    }

    public Vector3 GetWorldPositionByIndex(int x, int y)
    {
        return m_WorldBottomLeft + Vector3.right * (x * m_NodeDiameter + m_NodeRadius)
                                 + Vector3.forward * (y * m_NodeDiameter + m_NodeRadius);
    }

    public Vector2Int GetIndexByWorldPosition(Vector2 worldPosition)
    {
        return new Vector2Int();
    }

    public void DrawGizmos()
    {
        Gizmos.DrawWireCube(m_GridTransformPosition, new Vector3(m_GridWorldSize, 1, m_GridWorldSize));

        if (m_Grid != null)
        {
            for (int x = 0; x < m_GridSize; x++)
            {
                for (int y = 0; y < m_GridSize; y++)
                {
                    int n = m_Grid[x, y];
                    Vector3 worldPoint = GetWorldPositionByIndex(x, y);
                    
                    switch (n)
                    {
                        case -1:
                            Gizmos.color = Color.red;
                            break;
                        case 0:
                            Gizmos.color = Color.white;
                            break;
                        case 1:
                            Gizmos.color = Color.cyan;
                            break;
                    }

                    Gizmos.DrawCube(worldPoint, Vector3.one * (m_NodeDiameter - 0.1f));
                }
            }
        }
    }
    
    public int[,] grid
    {
        get
        {
            return m_Grid;
        }
    }
    
    public int gridSize
    {
        get
        {
            return m_GridSize;
        }
    }
}
