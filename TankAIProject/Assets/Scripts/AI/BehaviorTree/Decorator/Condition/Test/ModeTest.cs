using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BehaviorTree/Decorator/ConditionTest/ModeTest")]
public class ModeTest : ConditionTest
{
    public Mode m_ModeToTest;
    public TankModeListVariable m_TankMode;
    public override bool Test(int teamIndex, int tankIndex = -1)
    {
        if (m_TankMode.m_Values[tankIndex] == m_ModeToTest) return true;
        return false;
    }
}
