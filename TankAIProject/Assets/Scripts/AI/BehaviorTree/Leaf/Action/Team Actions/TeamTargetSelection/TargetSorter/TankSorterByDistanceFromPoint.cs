using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BehaviorTree/Action/TeamAction/TargetSorter/TankSorterByDistanceFromPoint")]
public class TankSorterByDistanceFromPoint : TargetSorter
{
    public PointVariable m_Point;
    public Vector3ListVariable m_TankPos;
    public override float ScoreCalculation(int tankIndex)
    {
        return (m_Point.m_CenterPos - m_TankPos.m_Values[tankIndex]).magnitude;
    }
}
