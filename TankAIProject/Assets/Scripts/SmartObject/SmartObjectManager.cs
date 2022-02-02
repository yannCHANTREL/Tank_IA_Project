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
        CreateHealthObject();

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
                CreateHealthObject();
            }
        }
    }

    public void CreateHealthObject()
    {
        int indexSpawn = Random.Range(0, m_ListSpawnPoint.Length);
        m_HealthObject = Instantiate(m_PrefabHealthObject, m_ListSpawnPoint[indexSpawn].position, m_ListSpawnPoint[indexSpawn].rotation);
    }
}
