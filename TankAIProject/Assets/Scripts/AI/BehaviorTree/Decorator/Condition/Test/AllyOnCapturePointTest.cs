using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BehaviorTree/Decorator/ConditionTest/AllyOnCapturePointTest")]
public class AllyOnCapturePointTest : ConditionTest
{
    public TeamList m_TeamList;
    public PointVariable m_CapturePoint;
    public override bool Test(int teamIndex, int tankIndex = -1)
    {
        foreach (var alliedTankManager in m_TeamList.m_Teams[teamIndex].m_TeamTank)
        {
            GameObject alliedTank = alliedTankManager.m_Instance;
            if (alliedTank.GetComponent<TankIndexManager>().m_TankIndex != tankIndex && (alliedTank.transform.position - m_CapturePoint.m_CenterPos).magnitude < m_CapturePoint.m_Radius) return true;
        }
        return false;
    }
}
