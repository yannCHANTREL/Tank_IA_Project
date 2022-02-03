using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BehaviorTree/Decorator/ConditionTest/SelfOnPointTest")]
public class SelfOnPointTest : ConditionTest
{
    public PointVariable m_Point;
    public Vector3ListVariable m_TankPos;
    public override bool Test(int teamIndex, int tankIndex = -1)
    {
        return (m_TankPos.m_Values[tankIndex] - m_Point.m_CenterPos).magnitude < m_Point.m_Radius;
    }
}
