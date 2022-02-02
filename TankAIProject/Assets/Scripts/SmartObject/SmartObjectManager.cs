using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartObjectManager : MonoBehaviour
{
    [SerializeField] private GameObject m_PrefabHealthObject;
    [SerializeField] private Transform[] m_ListSpawnPoint;
    [SerializeField] private float m_TimeRespawn = 10f;
    
    [SerializeField] private GameObjectVariable m_DataGameobject;
    [SerializeField] private FloatVariable m_TimeBeforeRespawn;

    private GameObject m_HealthObject;
    private float m_TimeSinceDestroy = 10f;
    
    public void Start()
    {
        // Create first healthObject
        CreateHealthObject();
        
        // Verification object is always alive, and manage it recreation
        StartCoroutine(LaunchUpdate());
    }
    
    public IEnumerator LaunchUpdate()
    {
        float timeWhenObjectDestroyed = 0f;
        bool startCountTime = true;
        
        while (true)
        {
            if (m_HealthObject == null)
            {
                if (startCountTime)
                {
                    timeWhenObjectDestroyed = Time.realtimeSinceStartup;
                    startCountTime = false;
                }
                m_TimeSinceDestroy = (Time.realtimeSinceStartup - timeWhenObjectDestroyed);
                m_TimeBeforeRespawn.m_Value = m_TimeRespawn - m_TimeSinceDestroy;

                if (m_TimeSinceDestroy > m_TimeRespawn)
                {
                    CreateHealthObject();
                    startCountTime = true;
                }
            }

            yield return null;
        }
    }

    public void CreateHealthObject()
    {
        int indexSpawn = Random.Range(0, m_ListSpawnPoint.Length);
        m_HealthObject = Instantiate(m_PrefabHealthObject, m_ListSpawnPoint[indexSpawn].position, m_ListSpawnPoint[indexSpawn].rotation);
        m_DataGameobject.m_Value = m_HealthObject;
    }
}
