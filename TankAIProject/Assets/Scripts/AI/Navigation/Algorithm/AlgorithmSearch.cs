using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public abstract class AlgorithmSearch : MonoBehaviour
{
    public VirtualGrid m_ClassGrid;
    public Vector2Int m_Start;
    public Vector2Int m_End;

    public abstract void Initialization();

    public abstract void LaunchSearch(Vector2Int indexStart, Vector2Int indexEnd, NavigationManager navigationManager);
}
