using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartObjectManager : MonoBehaviour
{
    [SerializeField] private GameObject m_PrefabHealthObject;
    [SerializeField] private Transform[] m_ListSpawnPoint;

    private GameObject m_HealthObject;
    
    public void Start()
    {
        // Create first healthObject
        createHealthObject();

        // Verification object is always alive, and manage it recreation
        LaunchUpdate();
    }
    
    public IEnumerator LaunchUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            UpdateHealthObject();
        }
    }

    public IEnumerator UpdateHealthObject()
    {
        Debug.Log("A");
        // if destroy, wait 10 seconds and recreate healthObject
        if (m_HealthObject != null)
        {
            Debug.Log("B");
            yield return new WaitForSeconds(10f);
            createHealthObject();
        }

        yield return null;
    }

    public void createHealthObject()
    {
        int indexSpawn = Random.Range(0, m_ListSpawnPoint.Length);
        m_HealthObject = Instantiate(m_PrefabHealthObject, m_ListSpawnPoint[indexSpawn].position, m_ListSpawnPoint[indexSpawn].rotation);
    }
}
