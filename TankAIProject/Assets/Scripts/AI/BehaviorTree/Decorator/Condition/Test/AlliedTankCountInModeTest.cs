using Complete;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BehaviorTree/Decorator/ConditionTest/AlliedTankCountInModeTest")]
public class AlliedTankCountInModeTest : ConditionTest
{
    public Mode m_TankModeToTest;
    public TestPolicy m_TestPolicy;
    public TeamList m_TeamList;
    public TankModeListVariable m_TankMode;
    public enum TestPolicy { zero, one, all }
    public override bool Test(int teamIndex, int tankIndex = -1)
    {
        int validedCount = 0;
        List<TankManager> tankManagers = m_TeamList.m_Teams[teamIndex].m_TeamTank;
        int teamTankCount = tankManagers.Count;
        foreach (var tankManager in tankManagers)
        {
            bool stepTest = m_TankMode.m_Values[tankManager.m_Instance.GetComponent<TankIndexManager>().m_TankIndex] == m_TankModeToTest;

            if (stepTest)
            {
                if (m_TestPolicy == TestPolicy.one) { return true; }
                else { validedCount++; }
            }
        }
        if ((m_TestPolicy == TestPolicy.all && validedCount == teamTankCount) || (m_TestPolicy == TestPolicy.zero && validedCount == 0)) { return true; }
        else { return false; }
    }
}
