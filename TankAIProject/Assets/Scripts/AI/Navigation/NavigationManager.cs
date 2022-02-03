using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class NavigationManager : MonoBehaviour
{
    public DijkstraManager m_DijkstraManager;
    public AStarManager m_AStarManager;
    public NavMeshManager m_NavMeshManager;
    public VirtualGrid m_ClassGrid;
    
    private List<SearchAlgorithm> m_ListAlgorithm;
    public int m_AlgorithmMode = 1;

    public void Awake()
    {
        m_ListAlgorithm = new List<SearchAlgorithm>();
        m_ListAlgorithm.Add(m_DijkstraManager);
        m_ListAlgorithm.Add(m_AStarManager);
        m_ListAlgorithm.Add(m_NavMeshManager);
    }

    public void InitializationForEditor(DijkstraManager dijkstraManager, AStarManager aStarManager, VirtualGrid classGrid)
    {
        m_ClassGrid = classGrid;
        m_DijkstraManager = dijkstraManager;
        m_AStarManager = aStarManager;
        
        m_ListAlgorithm = new List<SearchAlgorithm>();
        m_ListAlgorithm.Add(m_DijkstraManager);
        m_ListAlgorithm.Add(m_AStarManager);
    }

    public void ChooseAAlgorithmMode(int index)
    {
        m_ListAlgorithm[index].Initialization(m_ClassGrid);
        m_AlgorithmMode = index;
    }

    

    public async Task<List<Vector3>> FindPath(Vector3 posStart, Vector3 posEnd)
    {
        if (m_AlgorithmMode == 2)
        {
            NavMeshPath navMeshPath = new NavMeshPath();
            NavMesh.CalculatePath(posStart, posEnd, NavMesh.AllAreas, navMeshPath);
            Vector3[] arrayPosition = navMeshPath.corners;
            m_ListAlgorithm[2].SetArrayForNavMesh(arrayPosition);
        }
        
        List<Vector3> ret = new List<Vector3>();
        
        Tuple<List<Node>, List<Node>> globalPath = await LaunchAlgorithmSearch(posStart, posEnd);

        if (globalPath != null)
        {
            List<Node> results = globalPath.Item2;

            foreach (var result in results)
            {
                ret.Add(new Vector3(result.position.x, 0, result.position.y));
            }
        }

        return ret;
    }

    public async Task<Tuple<List<Node>,List<Node>>> LaunchAlgorithmSearch(Vector3 posStart, Vector3 posEnd)
    {
        // Get the path
        Path thread = await Task.Run(() => m_ListAlgorithm[m_AlgorithmMode].LaunchSearch(posStart, posEnd));
        if (thread == null) return null;
        List<Node> entryPath = thread.nodes;
        
        // Get the final path (clean)
        List<Node> finalPath = CalculFinalPath(entryPath);
        Tuple<List<Node>, List<Node>> ret = new Tuple<List<Node>, List<Node>>(entryPath,finalPath);
        return ret;
    }

    public List<Node> CalculFinalPath(List<Node> entryPath)
    {
        List<Node> ret = new List<Node>();
        int size = entryPath.Count;

        if (entryPath == null || size < 2)
        {
            return ret;
        }

        Node current = entryPath[1];
        if (size == 2)
        {
            ret.Add(current);
            return ret;
        }

        float nodeDiameter = m_ClassGrid.nodeDiameter;
        float nodeHalfDiagonale = (nodeDiameter * Mathf.Sqrt(2)) / 2;
            
        Node nodeStart = entryPath[0];
        Node nodeEnd;
        List<Node> nodesHasCheck;

        for (int i = 2; i < size; i++)
        {
            nodeEnd = entryPath[i];
            
            if (i + 1 == size)
            {
                ret.Add(nodeEnd); // last case => save this case
                break;
            }
            
            nodesHasCheck = FindNodesNeedVerification(nodeStart, nodeEnd, entryPath);
            if (nodesHasCheck.Count == 0)
            {
                current = nodeEnd; // not blocker => not save this case
            } 
            else
            {
                bool canPass = true;
                foreach (var node in nodesHasCheck)
                {
                    float distanceBtwNodeAndLine = CalcDistPointLine(node.position, nodeStart.position, nodeEnd.position);
                    if (distanceBtwNodeAndLine <= nodeHalfDiagonale && node.stateNode == -1)
                    {
                        ret.Add(current); // blocker => save this case
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
        
        return ret;
    }

    private List<Node> FindNodesNeedVerification(Node nodeStart, Node nodeEnd, List<Node> entryPath)
    {
        Dictionary<Vector2Int, Node> nodes = m_ListAlgorithm[m_AlgorithmMode].GetListNode();
        List<Node> ret = new List<Node>();

        float startX = nodeStart.position.x;
        float startY = nodeStart.position.y;
        float endX = nodeEnd.position.x;
        float endY = nodeEnd.position.y;

        float nodeX, nodeY;
        
        foreach (var node in nodes)
        {
            nodeX = node.Value.position.x;
            nodeY = node.Value.position.y;
            if (((nodeX >= startX && nodeX <= endX) || (nodeX >= endX && nodeX <= startX)) && ((nodeY >= startY && nodeY <= endY) || (nodeY >= endY && nodeY <= startY)) && !entryPath.Contains(node.Value))
            {
                ret.Add(node.Value);
            }
        }
        
        return ret;
    }

    // between A and BC
    private Vector3 NearestPointFromLine(Vector3 a, Vector3 b, Vector3 c)
    {
        return b + Vector3.Project(a - b, c - b);
    }

    private float CalcDistPointLine(Vector3 a, Vector3 b, Vector3 c)
    {
        return (a - NearestPointFromLine(a, b, c)).magnitude;
    }

    public string DisplayAlgorithmChoose()
    {
        if (m_AlgorithmMode == 0) return "DijkstraManager";
        if (m_AlgorithmMode == 1) return "AStarManager";
        if (m_AlgorithmMode == 2) return "NavMeshManager";
        return "Nothing";
    }
}
