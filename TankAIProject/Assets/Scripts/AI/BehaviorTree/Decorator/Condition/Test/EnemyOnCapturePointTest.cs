using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BehaviorTree/Decorator/ConditionTest/EnemyOnCapturePointTest")]
public class EnemyOnCapturePointTest : ConditionTest
{
    public SensedTankListVariable m_SensedTank;
    public PointVariable m_CapturePoint;
    public override bool Test(int teamIndex, int tankIndex = -1)
    {
        foreach (var enemyTank in m_SensedTank.m_TeamSensedEnemies[teamIndex].m_List)
        {
            if ((enemyTank.transform.position - m_CapturePoint.m_CenterPos).magnitude < m_CapturePoint.m_Radius) return true;
        }
        return false;
    }
}
