using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DijkstraManager : MonoBehaviour
{
    public VirtualGrid m_ClassGrid;             // Reference Grid
    public Vector2Int m_start;
    public Vector2Int m_end;
    
    private Dictionary<Vector2Int, Node> m_Nodes;
    private Graph m_Graph;
    
    // Start is called before the first frame update
    void Start()
    {
        // preparation Dijsktra features
        PreparationForDijsktraFeatures();
        
        // Algorithm of research of the shortest path (Dijsktra)
        StartCoroutine(LaunchThreadWithDijsktra());
    }
    
    private void PreparationForDijsktraFeatures()
    {
        m_Nodes = new Dictionary<Vector2Int, Node>();
        
        // create all nodes
        for (int i = 0; i < m_ClassGrid.gridSize; i++)
        {
            for (int j = 0; j < m_ClassGrid.gridSize; j++)
            {
                if (m_ClassGrid.grid[i,j] == 0)
                {
                    Vector2 vect2 = m_ClassGrid.GetVector2WorldPositionByIndex(new Vector2Int(i, j));
                    Node node = new Node(m_ClassGrid.grid[i,j], vect2);
                    m_Nodes.Add(new Vector2Int(i,j),node);
                }
            }
        }

        // create all neighbors for each nodes
        foreach (var nodeTarget in m_Nodes)
        {
            int posGridI = nodeTarget.Key.x;
            int posGridJ = nodeTarget.Key.y;

            Node neighbors1, neighbors2, neighbors3, neighbors4;
            if (m_Nodes.TryGetValue(new Vector2Int(posGridI, posGridJ - 1), out neighbors1))
            {
                nodeTarget.Value.AddNeighbors(neighbors1);
            }
            if (m_Nodes.TryGetValue(new Vector2Int(posGridI + 1, posGridJ), out neighbors2))
            {
                nodeTarget.Value.AddNeighbors(neighbors2);
            }
            if (m_Nodes.TryGetValue(new Vector2Int(posGridI, posGridJ + 1), out neighbors3))
            {
                nodeTarget.Value.AddNeighbors(neighbors3);
            }
            if (m_Nodes.TryGetValue(new Vector2Int(posGridI - 1, posGridJ), out neighbors4))
            {
                nodeTarget.Value.AddNeighbors(neighbors4);
            }
        }

        // create graph with the dictionnary of nodes
        m_Graph = new Graph(m_Nodes);
    }
    
    private IEnumerator LaunchThreadWithDijsktra()
    {
        // Launch one thread each second, or when the previous is finish
        while (true)
        {
            Thread t = new Thread(ImplementedDijsktra);
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
    
    private void ImplementedDijsktra()
    {
        // try move since A point to B point
        Vector2Int start = m_ClassGrid.GetIndexByWorldPosition(m_start);
        Vector2Int end = m_ClassGrid.GetIndexByWorldPosition(m_end);
        if (m_Nodes.ContainsKey(start) && m_Nodes.ContainsKey(end))
        {
            Path m_Path = m_Graph.GetShortestPath (m_Nodes[start], m_Nodes[end]);
            //Debug.Log("Length = " + m_Path.length);
            m_ClassGrid.DrawDijkstraPath(m_Path);
        }
        else 
        {
            if (!m_Nodes.ContainsKey(start))
            {
                Debug.Log("Error start incorrect");
            }
            if (!m_Nodes.ContainsKey(end))
            {
                Debug.Log("Error end incorrect");
            }
        }
    }
}
