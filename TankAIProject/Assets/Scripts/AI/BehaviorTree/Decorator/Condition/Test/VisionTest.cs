using Complete;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BehaviorTree/Decorator/ConditionTest/VisionTest")]
public class VisionTest : ConditionTest
{
    public PointVariable m_Point;
    public Vector3ListVariable m_TargetPos;
    public PositionType m_PositionType;
    public bool m_TestStatic;
    public bool m_TestAlly;
    public bool m_TestEnemy;
    public TeamList m_TeamList;

    public enum PositionType { point, tankTarget}

    public override bool Test(int teamIndex, int tankIndex = -1)
    {
        Vector3 testPos = Vector3.zero;
        if (m_PositionType == PositionType.point) { testPos = m_Point.m_CenterPos; }
        else if (m_PositionType == PositionType.tankTarget)
        {
            testPos = m_TargetPos.m_Values[tankIndex];
        }

        foreach (var tankManager in m_TeamList.m_Teams[teamIndex].m_TeamTank)
        {
            GameObject tank = tankManager.m_Instance;
            if (tank.GetComponent<TankIndexManager>().m_TankIndex == tankIndex)
            {
                TankDetectCollider.collisionFeedback collisionFeedback = tank.GetComponent<TankDetectCollider>().DirectionnalSensing(testPos);
                return !((collisionFeedback.componentStaticDetected && m_TestStatic) || (collisionFeedback.AllyDetected && m_TestAlly) || (collisionFeedback.EnemyDetected && m_TestEnemy));
            }
        }
        return false;
    }
}
