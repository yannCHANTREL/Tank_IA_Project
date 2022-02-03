using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBTDebug : MonoBehaviour
{
    public int m_TankIndex;
    public TankModeListVariable m_TankModeListVariable;

    private void Update()
    {
        print(m_TankModeListVariable.m_Values[m_TankIndex]);
    }
}
