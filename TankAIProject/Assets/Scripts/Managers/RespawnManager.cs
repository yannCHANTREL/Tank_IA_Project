using System;
using System.Collections;
using System.Collections.Generic;
using Complete;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public TeamList m_TeamList;
    [Range(5,10)]
    public float m_TimeToRespawn;
    
    public void RespawnTank(GameObject go)
    {
        // This is not a tank
        if (!go.CompareTag("Tank")) return;
        
        StartCoroutine(RespawnTankCoroutine(go));
    }

    private IEnumerator RespawnTankCoroutine(GameObject go)
    {
        // Wait for the right time
        yield return new WaitForSeconds(m_TimeToRespawn);
        
        // Respawn the tank at the right spot
        
        TankMovement tankMovement = go.GetComponent<TankMovement>();
        int teamNumber = m_TeamList.GetTeamNumberByPlayerNumber(tankMovement.m_PlayerNumber);
        // Get the spawn of that team
        Transform spawn = m_TeamList.m_TeamsSpawn[(teamNumber - 1) % m_TeamList.m_TeamsSpawn.Length];

        TankHealth tankHealth = go.GetComponent<TankHealth>();
        tankHealth.m_Dead = false;

        go.transform.position = spawn.position;
        go.transform.rotation = spawn.rotation;
        
        go.SetActive(true);
    }
}
