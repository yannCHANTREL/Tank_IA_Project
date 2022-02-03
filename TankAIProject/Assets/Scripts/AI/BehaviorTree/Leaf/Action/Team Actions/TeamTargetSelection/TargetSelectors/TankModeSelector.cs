using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BehaviorTree/Action/TeamAction/TargetSelector/TankModeSelector")]
public class TankModeSelector : TargetSelector
{
    public Mode m_ModeToTest;
    public TankModeListVariable m_TankModeListVariable;
    public override bool Test(int tankIndex)
    {
        return m_TankModeListVariable.m_Values[tankIndex] == m_ModeToTest;
    }
}
