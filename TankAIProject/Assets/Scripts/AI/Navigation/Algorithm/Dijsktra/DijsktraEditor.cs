using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DijsktraEditor : MonoBehaviour
{
    public VirtualGrid m_ClassGrid;
    public Vector2Int m_Start;
    public Vector2Int m_End;
    public bool m_Activate;
    
    void Start()
    {
        if (m_Activate)
        {
            DijkstraManager dijkstraManager = gameObject.AddComponent(typeof(DijkstraManager)) as DijkstraManager;
            if (dijkstraManager != null)
            {
                dijkstraManager.InitializationCoordinates(this);
            
                // preparation Dijsktra features
                dijkstraManager.PreparationForDijsktraFeatures();

                // Algorithm of research of the shortest path (Dijsktra)
                StartCoroutine(dijkstraManager.LaunchThreadWithDijsktra());
            }
        }
    }
}
