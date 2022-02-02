using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public VirtualGrid m_VirualGrid;
    
    void Awake()
    {
        m_VirualGrid.CreateGrid();
    }

    private void OnDrawGizmos()
    {
        m_VirualGrid.DrawGizmos();
    }
}
