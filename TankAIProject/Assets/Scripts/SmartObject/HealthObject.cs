using System;
using System.Collections;
using System.Collections.Generic;
using Complete;
using UnityEngine;

public class HealthObject : MonoBehaviour
{
    private List<GameObject> m_ListInstanceOfTank;
    
    [SerializeField] private float m_AmountHeal;

    [SerializeField] private TeamList m_TeamList;

    // Start is called before the first frame update
    void Start()
    {
        Team[] listTeam = m_TeamList.m_Teams;
        List<TankManager> listTank = new List<TankManager>();
        foreach (var team in listTeam)
        {
            listTank.AddRange(team.m_TeamTank);
        }

        m_ListInstanceOfTank = new List<GameObject>();
        foreach (var tankManager in listTank)
        {
            m_ListInstanceOfTank.Add(tankManager.m_Instance);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        double radiusObject = renderer.bounds.size.x / 2;
        
        double radiusTank, radius;
        Collider collider;
        Vector3 posTank;
        double distance;
        
        Vector3 myPosition = transform.position;
        foreach (var instanceOfTank in m_ListInstanceOfTank)
        {
            collider = instanceOfTank.GetComponent<Collider>();
            radiusTank = collider.bounds.size.x / 2;
            radius = radiusObject + radiusTank;
            
            posTank = instanceOfTank.transform.position;
            distance = Math.Sqrt(Math.Pow((posTank.x - myPosition.x), 2f) +
                                 Math.Pow((posTank.z - myPosition.z), 2f));
            
            if (distance < radius)
            {
                ActionHealthObject(instanceOfTank);
            }
        }
    }

    private void ActionHealthObject(GameObject instanceOfTank)
    {
        TankHealth pvManager = instanceOfTank.GetComponent<TankHealth>();
        if (pvManager != null)
        {
            pvManager.Heal(m_AmountHeal);
            Destroy(gameObject);
            
            Debug.Log("Heal !");
        }
        else
        {
            Debug.Log("Error : HealthObject - ActionHealthObject. TankHealth null");
        }
    }
}
