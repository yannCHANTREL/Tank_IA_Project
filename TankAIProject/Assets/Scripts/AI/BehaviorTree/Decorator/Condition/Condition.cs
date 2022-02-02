using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BehaviorTree/Decorator/Condition")]
public class Condition : Decorator
{
    public List<TestData> m_Tests;
    public enum Policy { And, Or };

    public Policy m_CompositionPolicy;

    [System.Serializable]
    public struct TestData { public ConditionTest m_ConditionTests; public bool m_Signs; }
    public override void OnInitialize()
    {

    }

    public override Status BHUpdate(int teamIndex, int tankIndex = 0)
    {
        if (Test(teamIndex, tankIndex))
        {
            return m_Child.Tick(teamIndex, tankIndex);
        }
        else
        {
            return Status.failure;
        }
    }

    public override void OnTerminate(Status status)
    {

    }

    public bool Test(int teamIndex, int tankIndex = 0)
    {
        bool isAnd = m_CompositionPolicy == Policy.And;
        for (int i = 0; i < m_Tests.Count; i++)
        {
            bool test = m_Tests[i].m_Signs == m_Tests[i].m_ConditionTests.Test(teamIndex, tankIndex);
            if (isAnd && !test) { return false; }
            if (!isAnd && test) { return true; }
        }
        return isAnd;
    }
}
