using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AStarManager : AlgorithmSearch
{
    public bool m_Activate;
    
    private Dictionary<Vector2Int, Node> m_Nodes;
    private AStarSearch m_AStar;
    
    void Start()
    {
        if (m_Activate)
        {
            // preparation AStar features
            PreparationForAStarFeatures();

            // Algorithm of research of the shortest path (AStar)
            StartCoroutine(LaunchThreadWithAStar());
        }
    }
    
    public override void Initialization()
    {
        // preparation AStar features
        PreparationForAStarFeatures();
    }

    private void PreparationForAStarFeatures()
    {
        m_Nodes = new Dictionary<Vector2Int,Node>();
        
        // create locations
        for (int i = 0; i < m_ClassGrid.gridSize; i++)
        {
            for (int j = 0; j < m_ClassGrid.gridSize; j++)
            {
                Node location = new Node(m_ClassGrid.grid[i, j], m_ClassGrid.GetVector2WorldPositionByIndex(new Vector2Int(i, j)));
                m_Nodes.Add(new Vector2Int(i, j), location);
            }
        }
        
        // add state walls and forest and several neighbors
        foreach (var location in m_Nodes)
        {
            int posX = location.Key.x;
            int posY = location.Key.y;

            Node neighbors1, neighbors2, neighbors3, neighbors4;
            if (m_Nodes.TryGetValue(new Vector2Int(posX, posY - 1), out neighbors1))
            {
                location.Value.AddNeighbors(neighbors1);
            }
            if (m_Nodes.TryGetValue(new Vector2Int(posX + 1, posY), out neighbors2))
            {
                location.Value.AddNeighbors(neighbors2);
            }
            if (m_Nodes.TryGetValue(new Vector2Int(posX, posY + 1), out neighbors3))
            {
                location.Value.AddNeighbors(neighbors3);
            }
            if (m_Nodes.TryGetValue(new Vector2Int(posX - 1, posY), out neighbors4))
            {
                location.Value.AddNeighbors(neighbors4);
            }
        }
        m_AStar = new AStarSearch(new SquareGrid(m_Nodes.Count));
    }
    
    private IEnumerator LaunchThreadWithAStar()
    {
        // Launch one thread each second, or when the previous is finish
        while (true)
        {
            Thread t = new Thread(ImplementedAStarEditor);
            var temp = Time.realtimeSinceStartup;
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
        Vector2Int start = m_ClassGrid.GetIndexByWorldPosition(m_Start);
        Vector2Int end = m_ClassGrid.GetIndexByWorldPosition(m_End);
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

    public override void LaunchSearch(Vector2Int indexStart, Vector2Int indexEnd, NavigationManager navigationManager)
    {
        if (m_Nodes.ContainsKey(indexStart) && m_Nodes.ContainsKey(indexEnd))
        {
            navigationManager.path = m_AStar.GetShortestPath(m_Nodes[indexStart], m_Nodes[indexEnd]);
        }
        else 
        {
            if (!m_Nodes.ContainsKey(indexStart))
            {
                Debug.Log("Error AStar LaunchSearch start incorrect");
            }
            if (!m_Nodes.ContainsKey(indexEnd))
            {
                Debug.Log("Error AStar LaunchSearch end incorrect");
            }
        }
    }
}
