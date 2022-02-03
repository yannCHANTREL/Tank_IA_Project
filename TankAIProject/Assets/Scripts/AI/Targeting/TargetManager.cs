using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public GameObjectListVariable m_TargetTank;
    public GameObjectListVariable m_LastTargetTank;
    public TargetCountListVariable m_TargetCount;
    public SensedTankListVariable m_SensedTank;
    public TankIndexManager m_TankIndexManager;

    private void Update()
    {
        if (!(m_SensedTank && m_TargetTank && m_TargetCount && m_TankIndexManager)) return;

        m_LastTargetTank.m_Values = m_TargetTank.m_Values;

        int tankIndex = m_TankIndexManager.m_TankIndex;
        int teamIndex = m_TankIndexManager.m_TeamIndex;
        Vector3 tankPos = transform.position;
        RemoveTarget(tankIndex, teamIndex);


        List<GameObject> tankList = m_SensedTank.m_AttackingTanks[tankIndex].m_List;
        /*if (tankList.Count > 0)
        {
            SetNewTarget(GetNearestTankFromList(tankList, tankPos), tankIndex, teamIndex);
            return;
        }*/
        tankList = m_SensedTank.m_EnemyTanksOnCapturePoint[tankIndex].m_List;
        if (tankList.Count > 0)
        {
            SetNewTarget(GetNearestTankFromList(tankList, tankPos), tankIndex, teamIndex);
            return;
        }
        tankList = GetNotTargetedEnemyTankList(teamIndex);
        if (tankList.Count > 0)
        {
            SetNewTarget(GetNearestTankFromList(tankList, tankPos), tankIndex, teamIndex);
            return;
        }
        tankList = m_SensedTank.m_TeamSensedEnemies[teamIndex].m_List;
        if (tankList.Count > 0)
        {
            SetNewTarget(GetNearestTankFromList(tankList, tankPos), tankIndex, teamIndex);
            return;
        }
    }

    private void RemoveTarget(int tankIndex, int teamIndex)
    {
        GameObject tankTarget = m_TargetTank.m_Values[tankIndex];
        if (tankTarget)
        {
            m_TargetCount.m_Values[tankTarget.GetComponent<TankIndexManager>().m_TankIndex].m_List[teamIndex]--;
            m_TargetTank.m_Values[tankIndex] = null;
        }
    }

    private GameObject GetNearestTankFromList(List<GameObject> tankList, Vector3 tankPos)
    {
        GameObject nearestTank = null;
        float distance = float.MaxValue;
        foreach (var tank in tankList)
        {
            float stepDistance = (tankPos - tank.transform.position).magnitude;
            if (stepDistance < distance)
            {
                distance = stepDistance;
                nearestTank = tank;
            }
        }
        return nearestTank;
    }

    private void SetNewTarget(GameObject target, int tankIndex, int teamIndex)
    {
        m_TargetTank.m_Values[tankIndex] = target;
        m_TargetCount.m_Values[target.GetComponent<TankIndexManager>().m_TankIndex].m_List[teamIndex]++;
    }

    private List<GameObject> GetNotTargetedEnemyTankList(int teamIndex)
    {
        List<GameObject> tankList = new List<GameObject>();
        foreach(GameObject tank in m_SensedTank.m_TeamSensedEnemies[teamIndex].m_List)
        {
            if (m_TargetCount.m_Values[tank.GetComponent<TankIndexManager>().m_TankIndex].m_List[teamIndex] == 0)
            {
                tankList.Add(tank);
            }
        }

        return tankList;
    }
}
