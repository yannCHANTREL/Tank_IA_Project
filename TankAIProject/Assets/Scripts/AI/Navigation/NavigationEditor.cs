using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class NavigationEditor : MonoBehaviour
{
    public VirtualGrid m_ClassGrid;
    public Vector3 m_Start;
    public Vector3 m_End;
    public bool m_Activate;

    private bool m_Finish;
    
    void Start()
    {
        m_Finish = false;
        if (m_Activate)
        {
            DijkstraManager dijkstraManager = gameObject.AddComponent(typeof(DijkstraManager)) as DijkstraManager;
            AStarManager aStarManager = gameObject.AddComponent(typeof(AStarManager)) as AStarManager;
            NavigationManager navigationManager = gameObject.AddComponent(typeof(NavigationManager)) as NavigationManager;
            if (navigationManager != null)
            {
                // Initialize
                navigationManager.InitializationForEditor(dijkstraManager, aStarManager, m_ClassGrid);
                
                // Choose and initialize Dijsktra
                navigationManager.ChooseAAlgorithmMode(0);

                StartCoroutine(LaunchInBoucle(navigationManager));
            }
        }
    }

    private IEnumerator LaunchInBoucle(NavigationManager navigationManager)
    {
       while (true)
       {
           LaunchOnce(navigationManager);
           yield return new WaitUntil(() => m_Finish);
       }
    }
    
    private async void LaunchOnce(NavigationManager navigationManager)
    {
        m_Finish = false;
        // Launch search
        Tuple<List<Node>, List<Node>> globalPath = await navigationManager.LaunchAlgorithmSearch(m_Start, m_End);

        // Display the result
        m_ClassGrid.DrawNavigationPath(globalPath.Item1, globalPath.Item2);
        m_Finish = true;
    }
}
