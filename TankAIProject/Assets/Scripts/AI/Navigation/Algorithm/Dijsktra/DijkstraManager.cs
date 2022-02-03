using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class DijkstraManager : SearchAlgorithm
{
    private VirtualGrid m_ClassGrid;
    private Dictionary<Vector2Int, Node> m_Nodes;
    private Graph m_Graph;

    #region LaunchSinceEditor
    
    private DijsktraEditor m_Editor;

    public void InitializationCoordinates(DijsktraEditor editor)
    {
        m_Editor = editor;
        m_ClassGrid = editor.m_ClassGrid;
    }

    public IEnumerator LaunchThreadWithDijsktra()
    {
        // Launch one thread each second, or when the previous is finish
        while (true)
        {
            Thread t = new Thread(ImplementedDijsktraEditor);
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
    
    private void ImplementedDijsktraEditor()
    {
        // try move since A point to B point
        Vector2Int start = m_ClassGrid.GetIndexByWorldPosition(m_Editor.m_Start);
        Vector2Int end = m_ClassGrid.GetIndexByWorldPosition(m_Editor.m_End);
        if (m_Nodes.ContainsKey(start) && m_Nodes.ContainsKey(end))
        {
            Path path = m_Graph.GetShortestPath (m_Nodes[start], m_Nodes[end]);
            m_ClassGrid.DrawDijkstraPath(path);
        }
        else 
        {
            if (!m_Nodes.ContainsKey(start))
            {
                Debug.Log("Error Dijsktra start incorrect");
            }
            if (!m_Nodes.ContainsKey(end))
            {
                Debug.Log("Error Dijsktra end incorrect");
            }
        }
    }

    #endregion

    public override void Initialization(VirtualGrid grid)
    {
        m_ClassGrid = grid;
        // preparation Dijsktra features
        PreparationForDijsktraFeatures();
    }
    
    public void PreparationForDijsktraFeatures()
    {
        m_Nodes = new Dictionary<Vector2Int, Node>();
        
        // create all nodes
        for (int i = 0; i < m_ClassGrid.gridSize; i++)
        {
            for (int j = 0; j < m_ClassGrid.gridSize; j++)
            {
                Node node = new Node(m_ClassGrid.grid[i,j], m_ClassGrid.GetVector2WorldPositionByIndex(new Vector2Int(i, j)));
                m_Nodes.Add(new Vector2Int(i,j),node);
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

    public override Path LaunchSearch(Vector3 posStart, Vector3 posEnd)
    {
        Vector2Int indexStart = m_ClassGrid.GetIndexByWorldPosition(m_ClassGrid.Vector3ToVector2(posStart));
        Vector2Int indexEnd = m_ClassGrid.GetIndexByWorldPosition(m_ClassGrid.Vector3ToVector2(posEnd));
        
        if (m_Nodes.ContainsKey(indexStart) && m_Nodes.ContainsKey(indexEnd))
        {
            return m_Graph.GetShortestPath (m_Nodes[indexStart], m_Nodes[indexEnd]);
        }
        
        if (!m_Nodes.ContainsKey(indexStart))
        {
            Debug.Log("Error Dijsktra LaunchSearch start incorrect");
        }
        if (!m_Nodes.ContainsKey(indexEnd))
        {
            Debug.Log("Error Dijsktra LaunchSearch end incorrect");
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
