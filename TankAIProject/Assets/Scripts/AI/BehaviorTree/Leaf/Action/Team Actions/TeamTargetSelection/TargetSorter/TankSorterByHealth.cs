using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BehaviorTree/Action/TeamAction/TargetSorter/TankSorterByHealth")]
public class TankSorterByHealth : TargetSorter
{
    public FloatListVariable m_TankHealth;
    public override float ScoreCalculation(int tankIndex)
    {
        return m_TankHealth.m_Values[tankIndex];
    }
}
