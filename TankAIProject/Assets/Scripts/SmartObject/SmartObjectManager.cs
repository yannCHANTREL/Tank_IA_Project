using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartObjectManager : MonoBehaviour
{
    [SerializeField] private GameObject m_PrefabHealthObject;
    [SerializeField] private Transform[] m_ListSpawnPoint;

    private GameObject m_HealthObject;
    private bool isAlive;
    
    public void Start()
    {
        // Create first healthObject
        createHealthObject();

        // Verification object is always alive, and manage it recreation
        StartCoroutine(LaunchUpdate());
    }
    
    public IEnumerator LaunchUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (m_HealthObject == null)
            {
                yield return new WaitForSeconds(10f);
                createHealthObject();
            }
        }
    }

    public void createHealthObject()
    {
        isAlive = true;
        int indexSpawn = Random.Range(0, m_ListSpawnPoint.Length);
        m_HealthObject = Instantiate(m_PrefabHealthObject, m_ListSpawnPoint[indexSpawn].position, m_ListSpawnPoint[indexSpawn].rotation);
    }
}