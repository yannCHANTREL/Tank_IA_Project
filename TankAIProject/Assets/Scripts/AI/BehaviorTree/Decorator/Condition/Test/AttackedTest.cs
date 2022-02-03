using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BehaviorTree/Decorator/ConditionTest/AttackedTest")]
public class AttackedTest : ConditionTest
{
    public SensedTankListVariable m_SensedTank;
    public override bool Test(int teamIndex, int tankIndex = -1)
    {
        if (!m_SensedTank) return false;

        return m_SensedTank.m_AttackingTanks[tankIndex].m_List.Count > 0;
    }
}
