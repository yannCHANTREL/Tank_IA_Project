using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BehaviorTree/Decorator/ConditionTest/HealthSpawnedOrSoonSpawningTest")]
public class HealthSpawnedOrSpawningSoonTest : ConditionTest
{
    [SerializeField] private GameObjectVariable m_HealthObject;
    [SerializeField] private FloatVariable m_TimeBeforeHealthRespawn;
    [SerializeField] private float m_MaxRemainingTimeBeforeSpawn = 3f;
    
    public override bool Test(int teamIndex, int tankIndex = -1)
    {
        return m_HealthObject.m_Value != null || m_TimeBeforeHealthRespawn.m_Value < m_MaxRemainingTimeBeforeSpawn;
    }
}
