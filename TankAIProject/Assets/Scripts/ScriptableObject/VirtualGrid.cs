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
    public bool m_ActivateGizmos;

    private int[,] m_Grid;
    private float m_NodeDiameter;
    private float m_NodeRadius;
    private int m_GridSize;
    private Vector3 m_WorldBottomLeft3DPos;
    private Vector2 m_WorldBottomLeft;
    private float m_TankDiameter;

    // Dijkstra needs
    private Path m_DijsktraPath;
    
    // AStar needs
    private Dictionary<Vector2Int,Node> m_ListLocation;
    private AStarSearch m_AStar;
    private Path m_AStarPath;
    
    // NavigationManager needs
    private bool m_DisplayNavigationPath;
    private List<Node> m_EntryPath;
    private List<Node> m_FinalPath;

    public void CreateGrid()
    {
        m_DisplayNavigationPath = false;
        m_DijsktraPath = null;
        m_NodeDiameter = (float) m_GridWorldSize / m_NbNode;
        m_NodeRadius = m_NodeDiameter / 2;

        m_GridSize = Mathf.RoundToInt(m_GridWorldSize / m_NodeDiameter);

        m_WorldBottomLeft3DPos = m_GridTransformPosition - Vector3.right * m_GridWorldSize / 2 - Vector3.forward * m_GridWorldSize / 2;
        m_WorldBottomLeft = new Vector2(m_WorldBottomLeft3DPos.x, m_WorldBottomLeft3DPos.z);
        m_TankDiameter = Mathf.Sqrt(Mathf.Pow(m_PrefabTankCollider.size.x, 2) + Mathf.Pow(m_PrefabTankCollider.size.z, 2));
        
        m_Grid = new int[m_GridSize, m_GridSize];
       
        for (int x = 0; x < m_GridSize; x++)
        {
            for (int y = 0; y < m_GridSize; y++)
            {
                Vector3 worldPoint = GetVector3WorldPositionByIndex(new Vector2Int(x, y));
                
                bool walkable = !Physics.CheckSphere(worldPoint, m_TankDiameter - m_NodeRadius, m_UnwalkableLayerMask);
                
                m_Grid[x, y] = (walkable ? 0 : -1);
            }
        }
    }

    public Vector2 GetVector2WorldPositionByIndex(Vector2Int gridPos)
    {
        return Vector3ToVector2(GetVector3WorldPositionByIndex(gridPos));
    }

    public Vector2 Vector3ToVector2(Vector3 vect3)
    {
        return new Vector2(vect3.x, vect3.z);
    }
    
    public Vector3 Vector2ToVector3(Vector2 vect2)
    {
        return new Vector3(vect2.x, 0, vect2.y);
    }
    
    public Vector3 GetVector3WorldPositionByIndex(Vector2Int gridPos)
    {
        return m_WorldBottomLeft3DPos + Vector3.right * (gridPos.x * m_NodeDiameter + m_NodeRadius)
                                 + Vector3.forward * (gridPos.y * m_NodeDiameter + m_NodeRadius);
    }

    public Vector2Int GetIndexByWorldPosition(Vector2 worldPosition)
    {
        float posX = (worldPosition.x - m_WorldBottomLeft.x) / (Vector3.right.x * m_NodeDiameter) -
                     m_NodeRadius / m_NodeDiameter;
        float posY = (worldPosition.y - m_WorldBottomLeft.y) / (Vector3.forward.z * m_NodeDiameter) -
                     m_NodeRadius / m_NodeDiameter;
        
        return new Vector2Int(Mathf.RoundToInt(posX),Mathf.RoundToInt(posY));
    }

    public void DrawGizmos()
    {
        if (m_ActivateGizmos)
        {
            Gizmos.DrawWireCube(m_GridTransformPosition, new Vector3(m_GridWorldSize, 1, m_GridWorldSize));

            if (m_Grid != null)
            {
                for (int x = 0; x < m_GridSize; x++)
                {
                    for (int y = 0; y < m_GridSize; y++)
                    {
                        int n = m_Grid[x, y];
                        Vector3 worldPoint = GetVector3WorldPositionByIndex(new Vector2Int(x, y));
                        
                        switch (n)
                        {
                            case -1:
                                Gizmos.color = Color.red;
                                break;
                            case 0:
                                Gizmos.color = Color.white;
                                break;
                        }

                        Gizmos.DrawCube(worldPoint, Vector3.one * (m_NodeDiameter - 0.1f));
                    }
                }

                if (m_DijsktraPath != null)
                {
                    DrawDijsktraPathChoose();
                }
                
                if (m_AStarPath != null && m_AStar != null && m_ListLocation != null)
                {
                    DrawAStarPathChoose();
                }

                if (m_DisplayNavigationPath)
                {
                    DrawNavigationPathChoose();
                }
            }
        }
    }

    public void DrawDijkstraPath(Path path)
    {
        m_DijsktraPath = path;
    }
    
    public void DrawAStarPath(Dictionary<Vector2Int,Node> listLocation, AStarSearch aStar, Path path)
    {
        m_ListLocation = listLocation;
        m_AStar = aStar;
        m_AStarPath = path;
    }
    
    public void DrawNavigationPath(List<Node> entryPath, List<Node> finalPath)
    {
        m_EntryPath = entryPath;
        m_FinalPath = finalPath;
        m_DisplayNavigationPath = true;
    }
    
    public void DrawNavigationPathChoose()
    {
        if (m_EntryPath != null)
        {
            foreach (var node in m_EntryPath)
            {
                Vector3 worldPoint = Vector2ToVector3(node.position);
                Gizmos.color = Color.green;
                Gizmos.DrawCube(worldPoint, Vector3.one * (m_NodeDiameter - 0.1f));
            }
        }

        if (m_FinalPath != null)
        {
            foreach (var node in m_FinalPath)
            {
                Vector3 worldPoint = Vector2ToVector3(node.position);
                Gizmos.color = Color.magenta;
                Gizmos.DrawCube(worldPoint, Vector3.one * (m_NodeDiameter - 0.1f));
            }
        }
    }

    public void DrawAStarPathChoose()
    {
        List<Node> listNodes = m_AStarPath.nodes;
        
        // Display the grid in black and all the location go through to find the path
        Node ptr = null;
        foreach (var location in m_ListLocation)
        {
            if (m_AStar.m_CameFrom != null && m_AStar.m_CameFrom.TryGetValue(location.Value, out ptr))
            {
                Vector3 worldPoint = Vector2ToVector3(location.Value.position);
                Gizmos.color = Color.gray;
                Gizmos.DrawCube(worldPoint, Vector3.one * (m_NodeDiameter - 0.1f));
            }
        }
        
        // Display the path choose
        foreach (var location in listNodes)
        {
            Vector3 worldPoint = Vector2ToVector3(location.position);
            Gizmos.color = Color.cyan;
            Gizmos.DrawCube(worldPoint, Vector3.one * (m_NodeDiameter - 0.1f));
        }
    }
    
    public void DrawDijsktraPathChoose()
    {
        List<Node> listNodes = m_DijsktraPath.nodes;
        foreach (var node in listNodes)
        {
            Vector3 worldPoint = Vector2ToVector3(node.position);
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(worldPoint, Vector3.one * (m_NodeDiameter - 0.1f));
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

    public float nodeDiameter
    {
        get
        {
            return m_NodeDiameter;
        }
    }
}
