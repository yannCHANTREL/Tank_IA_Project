using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameOption")]
public class GameOptions : ScriptableObject
{
    public enum AIDifficulty
    {
        Normal, Hard
    }
    public enum AISearchAlgo
    {
        Dijkstra, AStar, Navmesh
    }

    public enum Mode
    {
        PlayerVSAI, AIVSAI
    }

    public enum BehaviorTreeEnum
    {
        HAD, HDA, DHA
    }

    public AISearchAlgo m_SearchAlgo;
    public AIDifficulty m_AIDifficulty;
    public BehaviorTreeEnum m_BehaviorTree;
    public Mode m_Mode;
    [Range(1, 3)] 
    public int m_NbPlayer;
}
