using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BehaviorTree/Action/TeamAction/TargetSelector/AttackedTankSelector")]
public class AttackedTankSelector : TargetSelector
{
    public SensedTankListVariable m_SensedTank;
    public override bool Test(int tankIndex)
    {
        return m_SensedTank.m_AttackingTanks[tankIndex].m_List.Count > 0; 
    }
}
