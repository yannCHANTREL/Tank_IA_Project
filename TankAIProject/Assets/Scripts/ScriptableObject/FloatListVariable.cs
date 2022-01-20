using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/FloatList")]
public class FloatListVariable : ScriptableObject
{
    public List<float> m_Values;

    public void Reset()
    {
        m_Values.Clear();
    }

    public void IncrementSize()
    {
        m_Values.Add(0f);
    }
}
