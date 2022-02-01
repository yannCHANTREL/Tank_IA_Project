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
    private int m_tankIndex;

    void Start()
    {
        if (!m_TankIndexManager) return;
        m_tankIndex = m_TankIndexManager.m_TankIndex;
        m_Transform = transform;
    }
    
    void Update()
    {
        m_SensedTank.m_TankSensedEnemies[m_tankIndex] = new List<GameObject>();
        for (int i = 0; i < m_TeamList.m_Teams.Length; i++)
        {
            if (i != m_tankIndex)
            {
                List<TankManager> teamTanks = m_TeamList.m_Teams[i].m_TeamTank;
                for (int j = 0; j < teamTanks.Count; j++)
                {
                    GameObject otherTank = teamTanks[j].m_Instance;
                    float distToTank = (otherTank.transform.position - m_Transform.position).magnitude;
                    if (distToTank <= m_DetectionRange)
                    {
                        m_SensedTank.m_TankSensedEnemies[m_tankIndex].Add(otherTank);
                    }
                }
            }
        }
    }
}
