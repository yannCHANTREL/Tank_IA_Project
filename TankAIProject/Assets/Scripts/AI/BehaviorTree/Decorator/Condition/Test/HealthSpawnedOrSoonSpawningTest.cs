using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BehaviorTree/Decorator/ConditionTest/HealthSpawnedOrSoonSpawningTest")]
public class HealthSpawnedOrSoonSpawningTest : ConditionTest
{
    [SerializeField] private GameobjectVariable m_HealthObject;
    [SerializeField] private floatVariable m_TimeBeforeHealthRepawn;
    [SerializeField] private float m_TimeRemainingNeededForSearchObject = 3f;
    
    public override bool Test(int teamIndex, int tankIndex = 0)
    {
        return m_HealthObject.m_Gameobject != null || m_TimeBeforeHealthRepawn.m_float < m_TimeRemainingNeededForSearchObject;
    }
}
