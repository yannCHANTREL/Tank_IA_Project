using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BehaviorTree/Decorator/Condition")]
public class Condition : Decorator
{
    public List<ConditionTest> m_ConditionTests;
    public List<bool> m_Signs;
    public enum Policy { And, Or };

    public Policy m_CompositionPolicy;

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
        if (m_ConditionTests.Count != m_Signs.Count) return false;

        bool isAnd = m_CompositionPolicy == Policy.And;
        for (int i = 0; i < m_ConditionTests.Count; i++)
        {
            bool test = m_Signs[i] == m_ConditionTests[i].Test(teamIndex, tankIndex);
            if (isAnd && !test) { return false; }
            if (!isAnd && test) { return true; }
        }
        return isAnd;
    }
}
