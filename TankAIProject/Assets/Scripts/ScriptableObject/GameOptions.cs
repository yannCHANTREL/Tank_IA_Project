using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameOption")]
public class GameOptions : ScriptableObject
{
    public enum AISearchAlgo
    {
        Dijkstra, AStar, Navmesh
    }

    public enum AIDifficulty
    {
        Normal, Hard
    }

    public enum Mode
    {
        PlayerVSAI, AIVSAI
    }

    public AISearchAlgo m_SearchAlgo;
    public AIDifficulty m_AIDifficulty;
    public Mode m_Mode;
    [Range(1, 3)] 
    public int m_NbPlayer;
}
