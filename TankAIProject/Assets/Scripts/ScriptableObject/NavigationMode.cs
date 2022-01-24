using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Navigation Mode")]
public class NavigationMode : ScriptableObject
{
    public DijkstraManager m_Dijsktra;
    public AStarManager m_AStarManager;
    public int m_NavigationMode;
}
