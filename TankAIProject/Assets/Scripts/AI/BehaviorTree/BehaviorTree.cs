using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BehaviorTree/BehaviorTree")]
public class BehaviorTree : ScriptableObject
{
    public Behavior m_Root;

    public void Tick(bool debugMode, int teamIndex, int tankIndex = -1)
    {
        m_Root.Tick(debugMode, teamIndex, tankIndex);
    }

    public void AddAITank(int teamIndex, int tankIndex = -1)
    {
        m_Root.AddAITank(teamIndex, tankIndex);
    }
    public void RemoveAITank(int teamIndex, int tankIndex = -1)
    {
        m_Root.RemoveAITank(teamIndex, tankIndex);
    }
}
