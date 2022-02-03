using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct GameObjectList
{
    public List<GameObject> m_List;

    public GameObjectList(List<GameObject> list)
    {
        m_List = list;
    }
    public void Reset()
    {
        m_List.Clear();
    }
}

[CreateAssetMenu(menuName = "Variables/SensedTankList")]
public class SensedTankListVariable : ListVariable
{
    public List<GameObjectList> m_TankSensedEnemies;
    public List<GameObjectList> m_TeamSensedEnemies;
    public List<GameObjectList> m_AttackingTanks;
    public List<GameObjectList> m_EnemyTanksOnCapturePoint;
    public TeamList m_TeamList;

    public override void Reset()
    {
        m_TankSensedEnemies = new List<GameObjectList>();
        m_TeamSensedEnemies = new List<GameObjectList>();
        m_AttackingTanks = new List<GameObjectList>();
        m_EnemyTanksOnCapturePoint = new List<GameObjectList>();
    }

    public override void IncrementTankSize()
    {
        m_TankSensedEnemies.Add(new GameObjectList(new List<GameObject>()));
        m_AttackingTanks.Add(new GameObjectList(new List<GameObject>()));
        m_EnemyTanksOnCapturePoint.Add(new GameObjectList(new List<GameObject>()));
    }
    
    public override void IncrementTeamSize()
    {
        m_TeamSensedEnemies.Add(new GameObjectList(new List<GameObject>()));
    }
}