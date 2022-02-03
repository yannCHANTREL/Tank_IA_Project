using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshManager : SearchAlgorithm
{
    private VirtualGrid m_ClassGrid;
    private Dictionary<Vector2Int, Node> m_Nodes;
    
    private Vector3[] m_NavMeshPath;

    public override void Initialization(VirtualGrid grid)
    {
        m_ClassGrid = grid;
        m_Nodes = new Dictionary<Vector2Int, Node>();
        PreparationSearch();
    }
    
    public override void SetArrayForNavMesh(Vector3[] navMeshPath)
    {
        m_NavMeshPath = navMeshPath;
    }

    public void PreparationSearch()
    {
        // create nodes
        for (int i = 0; i < m_ClassGrid.gridSize; i++)
        {
            for (int j = 0; j < m_ClassGrid.gridSize; j++)
            {
                Node location = new Node(m_ClassGrid.grid[i, j], m_ClassGrid.GetVector2WorldPositionByIndex(new Vector2Int(i, j)));
                m_Nodes.Add(new Vector2Int(i, j), location);
            }
        }
        
        // create all neighbors for each nodes
        foreach (var node in m_Nodes)
        {
            int posX = node.Key.x;
            int posY = node.Key.y;

            Node neighbors1, neighbors2, neighbors3, neighbors4;
            if (m_Nodes.TryGetValue(new Vector2Int(posX, posY - 1), out neighbors1))
            {
                node.Value.AddNeighbors(neighbors1);
            }
            if (m_Nodes.TryGetValue(new Vector2Int(posX + 1, posY), out neighbors2))
            {
                node.Value.AddNeighbors(neighbors2);
            }
            if (m_Nodes.TryGetValue(new Vector2Int(posX, posY + 1), out neighbors3))
            {
                node.Value.AddNeighbors(neighbors3);
            }
            if (m_Nodes.TryGetValue(new Vector2Int(posX - 1, posY), out neighbors4))
            {
                node.Value.AddNeighbors(neighbors4);
            }
        }
    }

    public override Path LaunchSearch(Vector3 posStart, Vector3 posEnd)
    {
        Path path = new Path();
        Vector2Int index;
        foreach (var aPosition in m_NavMeshPath)
        {
            index = m_ClassGrid.GetIndexByWorldPosition(m_ClassGrid.Vector3ToVector2(aPosition));
            path.nodes.Add(m_Nodes[index]);
        }
        
        return path;
    }
    
    public override Dictionary<Vector2Int, Node> GetListNode()
    {
        return m_Nodes;
    }
}
