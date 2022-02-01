using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Point")]
public class PointVariable : ScriptableObject
{
    public Vector3 m_CenterPos;
    public float m_Radius;
}