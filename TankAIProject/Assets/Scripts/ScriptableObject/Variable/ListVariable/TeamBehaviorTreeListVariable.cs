using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/TeamBehaviorTreeList")]
public class TeamBehaviorTreeListVariable : ListVariable
{
    public List<BehaviorTree> m_Value;

    public override void Reset()
    {
        m_Value = new List<BehaviorTree>();
    }

    public override void IncrementTankSize()
    {

    }

    public override void IncrementTeamSize()
    {
        m_Value.Add(null);
    }
}