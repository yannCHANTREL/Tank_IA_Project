using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class AStarManager : SearchAlgorithm
{
    private VirtualGrid m_ClassGrid;
    private Dictionary<Vector2Int, Node> m_Nodes;
    private AStarSearch m_AStar;

    #region LaunchSinceEditor

    private AStarEditor m_Editor;

    public void InitializationCoordinates(AStarEditor editor)
    {
        m_Editor = editor;
        m_ClassGrid = editor.m_ClassGrid;
    }
    
    public IEnumerator LaunchThreadWithAStar()
    {
        // Launch one thread each second, or when the previous is finish
        while (true)
        {
            Thread t = new Thread(ImplementedAStarEditor);
            float temp = Time.realtimeSinceStartup;
            t.Start();
                
            // wait end execution thread
            // OR 1 second difference between now and the start of the thread
            while(t.IsAlive || (Time.realtimeSinceStartup - temp) < 1.0f)
            {
                yield return null;
            }
            //Debug.Log("time execution thread : " + (Time.realtimeSinceStartup - temp));
        }
    } 
    
    private void ImplementedAStarEditor()
    {
        // try move since A point to B point
        Vector2Int start = m_ClassGrid.GetIndexByWorldPosition(m_Editor.m_Start);
        Vector2Int end = m_ClassGrid.GetIndexByWorldPosition(m_Editor.m_End);
        if (m_Nodes.ContainsKey(start) && m_Nodes.ContainsKey(end))
        {
            Path path = m_AStar.GetShortestPath(m_Nodes[start], m_Nodes[end]);
            m_ClassGrid.DrawAStarPath(m_Nodes, m_AStar, path);
        }
        else 
        {
            if (!m_Nodes.ContainsKey(start))
            {
                Debug.Log("Error AStar start incorrect");
            }
            if (!m_Nodes.ContainsKey(end))
            {
                Debug.Log("Error AStar end incorrect");
            }
        }
    }

    #endregion

    public override void Initialization(VirtualGrid grid)
    {
        m_ClassGrid = grid;
        // preparation AStar features
        PreparationForAStarFeatures();
    }

    public void PreparationForAStarFeatures()
    {
        m_Nodes = new Dictionary<Vector2Int,Node>();
        
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
        m_AStar = new AStarSearch(new SquareGrid(m_Nodes.Count));
    }

    public override Path LaunchSearch(Vector3 posStart, Vector3 posEnd)
    {
        Vector2Int indexStart = m_ClassGrid.GetIndexByWorldPosition(m_ClassGrid.Vector3ToVector2(posStart));
        Vector2Int indexEnd = m_ClassGrid.GetIndexByWorldPosition(m_ClassGrid.Vector3ToVector2(posEnd));
        
        if (m_Nodes.ContainsKey(indexStart) && m_Nodes.ContainsKey(indexEnd))
        {
            return m_AStar.GetShortestPath(m_Nodes[indexStart], m_Nodes[indexEnd]);
        }
        
        if (!m_Nodes.ContainsKey(indexStart))
        {
            Debug.Log("Error AStar LaunchSearch start incorrect");
        }
        if (!m_Nodes.ContainsKey(indexEnd))
        {
            Debug.Log("Error AStar LaunchSearch end incorrect");
        }
        return null;
    }
    
    public override Dictionary<Vector2Int, Node> GetListNode()
    {
        return m_Nodes;
    }
    
    public override void SetArrayForNavMesh(Vector3[] navMeshPath)
    {
    }
}
