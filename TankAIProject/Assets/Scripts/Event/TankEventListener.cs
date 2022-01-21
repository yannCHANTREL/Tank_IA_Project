using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class TankEventListener : MonoBehaviour
{
    [SerializeField] private TankEvent m_Event;
    [SerializeField] private UnityEvent m_OnEventRaised;
    [SerializeField] private TankIndexManager m_TankIndexManager;

    public void OnEventRaised(int tankIndex)
    {
        if (tankIndex == m_TankIndexManager.m_TankIndex) { m_OnEventRaised.Invoke(); }
    }

    public void OnEnable()
    {
        m_Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        m_Event.UnregisterListener(this);
    }
}