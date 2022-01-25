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

    void Start()
    {
        m_Transform = transform;
    }
    
    void Update()
    {
        m_SensedTank.m_TankSensedEnemies[m_TankIndexManager.m_TankIndex] = new List<GameObject>();
        for (int i = 0; i < m_TeamList.m_Teams.Length; i++)
        {
            if (i != m_TankIndexManager.m_TeamIndex)
            {
                List<TankManager> teamTanks = m_TeamList.m_Teams[i].m_TeamTank;
                for (int j = 0; j < teamTanks.Count; j++)
                {
                    GameObject otherTank = teamTanks[j].m_Instance;
                    float distToTank = (otherTank.transform.position - m_Transform.position).magnitude;
                    if (distToTank <= m_DetectionRange)
                    {
                        m_SensedTank.m_TankSensedEnemies[m_TankIndexManager.m_TankIndex].Add(otherTank);
                    }
                }
            }
        }
    }
}
