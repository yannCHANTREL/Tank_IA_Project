using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SmartObjectManager : MonoBehaviour
{
    [SerializeField] private GameObject m_PrefabHealthObject;
    [SerializeField] private Transform[] m_ListSpawnPoint;
    [SerializeField] private float m_TimeRespawn = 10f;
    
    [SerializeField] private GameObjectVariable m_DataGameobject;
    [SerializeField] private FloatVariable m_TimeBeforeRespawn;

    private GameObject m_HealthObject;
    private float m_TimeSinceDestroy = 10f;

    private float m_TimeWhenObjectDestroyed = 0f;
    private bool m_StartCountTime = true;
    
    public void Start()
    {
        // Create first healthObject
        CreateHealthObject();
    }

    public void Update()
    {
        if (m_HealthObject == null)
        {
            if (m_StartCountTime)
            {
                m_TimeWhenObjectDestroyed = Time.realtimeSinceStartup;
                m_StartCountTime = false;
            }
            m_TimeSinceDestroy = (Time.realtimeSinceStartup - m_TimeWhenObjectDestroyed);
            
            // send remaining time before respawn to a ScriptableObject
            m_TimeBeforeRespawn.m_Value = m_TimeRespawn - m_TimeSinceDestroy;

            if (m_TimeSinceDestroy > m_TimeRespawn)
            {
                CreateHealthObject();
                m_StartCountTime = true;
            }
        }
    }

    public void CreateHealthObject()
    {
        int indexSpawn = Random.Range(0, m_ListSpawnPoint.Length);
        m_HealthObject = Instantiate(m_PrefabHealthObject, m_ListSpawnPoint[indexSpawn].position, m_ListSpawnPoint[indexSpawn].rotation);
        // send reference of object to a ScriptableObject
        m_DataGameobject.m_Value = m_HealthObject;
    }
}
