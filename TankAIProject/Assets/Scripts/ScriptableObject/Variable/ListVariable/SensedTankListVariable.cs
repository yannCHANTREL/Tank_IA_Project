using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/SensedTankList")]
public class SensedTankListVariable : ListVariable
{
    public List<List<GameObject>> m_TankSensedEnemies;
    public List<List<GameObject>> m_TeamSensedEnemies;
    public List<List<GameObject>> m_AttackingTanks;
    public List<List<GameObject>> m_EnemyTanksOnCapturePoint;
    public TeamList m_TeamList;

    public override void Reset()
    {
        m_TankSensedEnemies = new List<List<GameObject>>();
        m_TeamSensedEnemies = new List<List<GameObject>>();
        m_AttackingTanks = new List<List<GameObject>>();
        m_EnemyTanksOnCapturePoint = new List<List<GameObject>>();
    }

    public override void IncrementTankSize()
    {
        m_TankSensedEnemies.Add(new List<GameObject>());
        m_AttackingTanks.Add(new List<GameObject>());
        m_EnemyTanksOnCapturePoint.Add(new List<GameObject>());
    }
    
    public override void IncrementTeamSize()
    {
        m_TeamSensedEnemies.Add(new List<GameObject>());
    }
}