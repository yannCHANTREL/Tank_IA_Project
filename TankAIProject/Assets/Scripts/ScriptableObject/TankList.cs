using System.Collections;
using System.Collections.Generic;
using Complete;
using UnityEngine;

[CreateAssetMenu(menuName = "Tank List")]
public class TankList : ScriptableObject
{
    private List<TankManager> m_Tanks;

    public void AddTank(TankManager tank)
    {
        m_Tanks.Add(tank);
    }
    
    public List<TankManager> Tanks
    {
        get
        {
            return m_Tanks;
        }
    }
    
    public List<GameObject> GameObjectsTanks
    {
        get
        {
            List<GameObject> gameObjectsTanks = new List<GameObject>();
            foreach (var tank in m_Tanks)
            {
                gameObjectsTanks.Add(tank.m_Instance);
            }
            return gameObjectsTanks;
        }
    }
}
