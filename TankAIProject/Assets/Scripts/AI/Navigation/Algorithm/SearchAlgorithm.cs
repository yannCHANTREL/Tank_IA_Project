using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

public abstract class SearchAlgorithm : MonoBehaviour
{
    public abstract void Initialization(VirtualGrid grid);

    public abstract Path LaunchSearch(Vector3 posStart, Vector3 posEnd);

    public abstract Dictionary<Vector2Int, Node> GetListNode();

    public abstract void SetArrayForNavMesh(Vector3[] navMeshPath);
}
