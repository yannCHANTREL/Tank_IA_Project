using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarEditor : MonoBehaviour
{
    public VirtualGrid m_ClassGrid;
    public Vector2Int m_Start;
    public Vector2Int m_End;
    public bool m_Activate;
    
    void Start()
    {
        if (m_Activate)
        {
            AStarManager aStarManager = gameObject.AddComponent(typeof(AStarManager)) as AStarManager;
            if (aStarManager != null)
            {
                aStarManager.InitializationCoordinates(this);
            
                // preparation Dijsktra features
                aStarManager.PreparationForAStarFeatures();

                // Algorithm of research of the shortest path (Dijsktra)
                StartCoroutine(aStarManager.LaunchThreadWithAStar());
            }
        }
    }
}
