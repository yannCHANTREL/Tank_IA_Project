using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public abstract class SearchAlgorithm : MonoBehaviour
{
    public abstract void Initialization(VirtualGrid grid);

    public abstract void LaunchSearch(Vector2Int indexStart, Vector2Int indexEnd, NavigationManager navigationManager);
}
