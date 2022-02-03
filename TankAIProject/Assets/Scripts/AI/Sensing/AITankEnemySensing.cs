using System.Collections;
using System.Collections.Generic;
using Complete;
using UnityEditor;
using UnityEngine;

public class AITankEnemySensing : MonoBehaviour
{
    public TeamList m_TeamList;
    public SensedTankListVariable m_SensedTank;
    public TankIndexManager m_TankIndexManager;

    public float m_DetectionRange = 20;

    private Transform m_Transform;
    private int m_TankIndex;
    private int m_TeamIndex;

    void Start()
    {
        if (!m_TankIndexManager) return;
        m_TankIndex = m_TankIndexManager.m_TankIndex;
        m_TeamIndex = m_TankIndexManager.m_TeamIndex;
        m_Transform = transform;
    }

    private void OnDisable()
    {
        m_SensedTank.m_TankSensedEnemies[m_TankIndex].Reset();
    }

    void Update()
    {
        m_SensedTank.m_TankSensedEnemies[m_TankIndex].Reset();
        
        for (int i = 0; i < m_TeamList.m_Teams.Length; i++)
        {
            if (i != m_TeamIndex)
            {
                List<TankManager> teamTanks = m_TeamList.m_Teams[i].m_TeamTank;
                for (int j = 0; j < teamTanks.Count; j++)
                {
                    GameObject otherTank = teamTanks[j].m_Instance;
                    float distToTank = (otherTank.transform.position - m_Transform.position).magnitude;


                    if (distToTank <= m_DetectionRange && otherTank.activeInHierarchy)
                    {
                        m_SensedTank.m_TankSensedEnemies[m_TankIndex].m_List.Add(otherTank);
                    }
                }
            }
        }
    }
}
