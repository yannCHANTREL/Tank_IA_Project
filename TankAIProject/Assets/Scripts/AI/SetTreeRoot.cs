using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTreeRoot : MonoBehaviour
{
   [SerializeField] 
    private GameOptions m_GameOptions;
    
    [SerializeField] 
    private BehaviorTree m_TankBehaviorTreeHDA;
    
    [SerializeField] 
    private BehaviorTree m_TankBehaviorTreeHAD;
    
    [SerializeField] 
    private BehaviorTree m_TankBehaviorTreeDHA;
    
    // Start is called before the first frame update
    void Start()
    {
        BehaviorTreeComponent behaviorTreeComponent = gameObject.GetComponent<BehaviorTreeComponent>();

        for (int i = 0; i < behaviorTreeComponent.m_TeamBehaviorTrees.m_Value.Count; i++)
        {
            switch (m_GameOptions.m_BehaviorTree)
            {
                case GameOptions.BehaviorTreeEnum.HealthDefenseAttack:
                    behaviorTreeComponent.m_TeamBehaviorTrees.m_Value[i] = m_TankBehaviorTreeHDA;
                    break;
                case GameOptions.BehaviorTreeEnum.HealthAttackDefense:
                    behaviorTreeComponent.m_TeamBehaviorTrees.m_Value[i] = m_TankBehaviorTreeHAD;
                    break;
                case GameOptions.BehaviorTreeEnum.DefenseHealthAttack:
                    behaviorTreeComponent.m_TeamBehaviorTrees.m_Value[i] = m_TankBehaviorTreeDHA;
                    break;
            }
            
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
