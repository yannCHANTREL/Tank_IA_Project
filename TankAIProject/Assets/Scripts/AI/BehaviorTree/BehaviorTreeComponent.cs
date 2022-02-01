using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTreeComponent : MonoBehaviour
{
    public BehaviorTree m_BehaviorTree;
    public TankIndexManager m_TankIndexManager;
    public int TeamIndex;
    public bool m_IsTeamTree;
    
    void Update()
    {
        if (m_IsTeamTree) m_BehaviorTree.Tick(TeamIndex);
        else m_BehaviorTree.Tick(m_TankIndexManager.m_TeamIndex, m_TankIndexManager.m_TankIndex);
    }
}
