using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public VirtualGrid m_VirualGrid;
    public TeamList m_TeamList;                 // Reference Team List
    
    void Awake()
    {
        m_VirualGrid.CreateGrid();
        //DetectionTank();
    }

    private void Update()
    {
        //DetectionTank();
    }

    private void DetectionTank()
    {
        // POTENTIELLEMENT A FAIRE PLUS TARD (dynamic detect tank)
        //m_VirualGrid.UpdateGrid(m_TankList.GameObjectsTanks);
    }

    private void OnDrawGizmos()
    {
        //m_VirualGrid.DrawGizmos();
    }
}
