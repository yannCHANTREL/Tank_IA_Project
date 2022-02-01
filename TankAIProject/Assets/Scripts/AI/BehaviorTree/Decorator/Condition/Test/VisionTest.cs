using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionTest : ConditionTest
{
    public PointVariable m_Point;
    public Vector3ListVariable m_TargetPos;
    public PositionType m_PositionType;

    public enum PositionType { point, tankTarget}

    public override bool Test(int teamIndex, int tankIndex = 0)
    {
        /*Vector3 testPos = Vector3.zero;
        if (m_PositionType == PositionType.point) { testPos = m_Point.m_CenterPos; }
        else if (m_PositionType == PositionType.tankTarget) { testPos = m_TargetPos.m_Values[tankIndex]; }
        else return false;*/

        return true;
    }
}
