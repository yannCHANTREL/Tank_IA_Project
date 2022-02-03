using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTreeComponent : MonoBehaviour
{
    public BehaviorTree m_TankBehaviorTree;
    
    public TeamBehaviorTreeListVariable m_TeamBehaviorTrees;
    public TankIndexManager m_TankIndexManager;
    public TreeType m_TreeType;
    public bool m_DebugMode;

    private int tankIndex;
    private int teamIndex;

    public enum TreeType { tank, team}

    private void Start()
    {
        if (m_TankIndexManager)
        {
            tankIndex = m_TankIndexManager.m_TankIndex;
            teamIndex = m_TankIndexManager.m_TeamIndex;
        }
    }

    void Update()
    {
        if (m_TreeType == TreeType.team && m_TeamBehaviorTrees)
        {
            for (int i = 0; i < m_TeamBehaviorTrees.m_Value.Count; i++)
            {
                BehaviorTree teamBehaviorTree = m_TeamBehaviorTrees.m_Value[i];
                if (teamBehaviorTree) { teamBehaviorTree.Tick(m_DebugMode, i); }
            }
        }
        else if (m_TreeType == TreeType.tank && m_TankIndexManager)
        {
            m_TankBehaviorTree.Tick(m_DebugMode, teamIndex, tankIndex);
        }
    }
}
