using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class NavigationManager : MonoBehaviour
{
    public DijkstraManager m_DijkstraManager;
    public AStarManager m_AStarManager;
    public VirtualGrid m_ClassGrid;
    
    private List<SearchAlgorithm> m_ListAlgorithm;
    private int m_AlgorithmMode;
    
    private Path m_Path;
    private List<Vector3> m_FinalPath; 
    
    public void Start()
    {
        m_ListAlgorithm = new List<SearchAlgorithm>();
        m_ListAlgorithm.Add(m_DijkstraManager);
        m_ListAlgorithm.Add(m_AStarManager);
        m_Path = null;
        m_FinalPath = null;
    }

    public void ChooseAAlgorithmMode(int index)
    {
        m_ListAlgorithm[index].Initialization(m_ClassGrid);
        m_AlgorithmMode = index;
    }

    public IEnumerator LaunchAlgorithmSearch(Vector3 posStart, Vector3 posEnd)
    {
        Vector2Int indexStart = m_ClassGrid.GetIndexByWorldPosition(m_ClassGrid.Vector3ToVector2(posStart));
        Vector2Int indexEnd = m_ClassGrid.GetIndexByWorldPosition(m_ClassGrid.Vector3ToVector2(posEnd));

        Thread thread = new Thread(() => m_ListAlgorithm[m_AlgorithmMode].LaunchSearch(indexStart, indexEnd, this));
        float temp = Time.realtimeSinceStartup;
        thread.Start();

        // wait end execution thread
        // OR 1 second difference between now and the start of the thread
        while(thread.IsAlive || (Time.realtimeSinceStartup - temp) < 1.0f)
        {
            yield return null;
        }

        List<Node> path = CalculFinalPath();
        m_FinalPath = new List<Vector3>();
        foreach (var node in path)
        {
            m_FinalPath.Add(new Vector3(node.position.x, 0, node.position.y));
        }
        
        yield return 1;
    }

    public List<Node> CalculFinalPath()
    {
        List<Node> entryPath = path.nodes;
        int size = entryPath.Count;

        if (path == null || size < 2)
        {
            return null;
        }

        float nodeDiameter = m_ClassGrid.nodeDiameter;
        float nodeHalfDiagonale = (nodeDiameter * Mathf.Sqrt(2)) / 2;
            
        List<Node> finalNodes = new List<Node>();
        Node nodeStart = entryPath[0];
        Node current = entryPath[1];
        
        Node nodeEnd = null;
        Vector3 line = Vector3.zero;
        List<Node> nodes = null;

        for (int i = 2; i < size; i++)
        {
            nodeEnd = entryPath[i];
            line = nodeEnd.position - nodeStart.position;
            nodes = FindNodeNeedVerification(nodeStart, nodeEnd);
            
            if (nodes.Count == 0)
            {
                current = nodeEnd; // not blocker => not save this case
            }
            else
            {
                bool canPass = true;
                foreach (var node in nodes)
                {
                    float distanceBtwNodeAndLine = CalcDistPointLine(node.position, nodeStart.position, nodeEnd.position);
                    if (distanceBtwNodeAndLine <= nodeHalfDiagonale && node.stateNode == -1)
                    {
                        finalNodes.Add(current); // blocker => save this case
                        nodeStart = current;
                        current = nodeEnd;
                        canPass = false;
                        break;
                    }
                }
                if (canPass)
                {
                    current = nodeEnd; // not blocker => not save this case
                }
            }
        }
        
        return finalNodes;
    }

    private List<Node> FindNodeNeedVerification(Node nodeStart, Node nodeEnd)
    {
        Dictionary<Vector2Int, Node> nodes = m_ListAlgorithm[m_AlgorithmMode].GetListNode();
        List<Node> ret = new List<Node>();

        foreach (var node in nodes)
        {
            if (node.Key.x >= nodeStart.position.x && node.Key.x <= nodeEnd.position.x && node.Key.y >= nodeStart.position.y && node.Key.y <= nodeEnd.position.y && !path.nodes.Contains(node.Value))
            {
                ret.Add(node.Value);
            }
        }
        
        return ret;
    }

    // between A and BC
    Vector3 NearestPointFromLine(Vector3 A, Vector3 B, Vector3 C)
    {
        return B + Vector3.Project(A - B, C - B);
    }

    float CalcDistPointLine(Vector3 A, Vector3 B, Vector3 C)
    {
        return (A - NearestPointFromLine(A, B, C)).magnitude;
    }
    
    public Path path
    {
        get
        {
            return m_Path;
        }
        set
        {
            m_Path = value;
        }
    }
    
    public List<Vector3> finalPath
    {
        get
        {
            return m_FinalPath;
        }
    }
}
