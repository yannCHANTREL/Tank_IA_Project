using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BehaviorTree/Composite/Parallel")]
public class Parallel : Composite
{
    public enum Policy { RequireOne, RequireAll };

    public Policy m_SuccessPolicy;
    public Policy m_FailurePolicy;

    public override void OnInitialize()
    {

    }

    public override Status BHUpdate(int teamIndex, int tankIndex = 0)
    {
        int successCount = 0;
        int FailureCount = 0;
        foreach (var child in m_Children)
        {   
            if (child.m_Status == Status.running) child.Tick(teamIndex, tankIndex);
            if (child.m_Status == Status.success)
            {
                successCount++;
                if (m_SuccessPolicy == Policy.RequireOne) return Status.success;
            }
            if (child.m_Status == Status.failure)
            {
                FailureCount++;
                if (m_SuccessPolicy == Policy.RequireOne) return Status.failure;
            }
        }

        int size = m_Children.Count;
        if (m_FailurePolicy == Policy.RequireAll && FailureCount == size) return Status.failure;
        if (m_SuccessPolicy == Policy.RequireAll && successCount == size) return Status.success;
        return Status.running;
    }

    public override void OnTerminate(Status status)
    {
        foreach (var child in m_Children)
        {
            if (child.m_Status == Status.running) child.OnTerminate(status);
        }
    }
}