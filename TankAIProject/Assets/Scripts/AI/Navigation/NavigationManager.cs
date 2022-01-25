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
    
    public void Start()
    {
        m_ListAlgorithm = new List<SearchAlgorithm>();
        m_ListAlgorithm.Add(m_DijkstraManager);
        m_ListAlgorithm.Add(m_AStarManager);
        m_Path = null;
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
        yield return null;
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
}
