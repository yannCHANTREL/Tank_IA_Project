using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameOption")]
public class GameOptions : ScriptableObject
{
    public enum AISearchAlgo
    {
        Dijsktra, AStar, Navmesh
    }

    public enum AIDifficulty
    {
        Facile, Normal, Difficile
    }

    public enum Mode
    {
        PVP, PVE
    }

    public AISearchAlgo m_SearchAlgo;
    public AIDifficulty m_AIDifficulty;
    public Mode m_Mode;
}
