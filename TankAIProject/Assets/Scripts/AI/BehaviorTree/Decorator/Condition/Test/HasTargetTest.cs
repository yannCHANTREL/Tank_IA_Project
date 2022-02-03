using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BehaviorTree/Decorator/ConditionTest/HasTargetTest")]
public class HasTargetTest : ConditionTest
{
    public GameObjectListVariable m_TargetTank;
    public override bool Test(int teamIndex, int tankIndex = -1)
    {
        if (m_TargetTank.m_Values[tankIndex])
        {
            return true;
        }
        return false;
    }
}