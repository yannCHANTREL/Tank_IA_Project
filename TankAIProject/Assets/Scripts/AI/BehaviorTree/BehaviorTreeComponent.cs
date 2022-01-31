using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTreeComponent : MonoBehaviour
{
    public BehaviorTree m_BehaviorTree;
    public TankIndexManager m_TankIndexManager;
    
    void Update()
    {
        m_BehaviorTree.Tick(m_TankIndexManager.m_TankIndex);
    }
}
