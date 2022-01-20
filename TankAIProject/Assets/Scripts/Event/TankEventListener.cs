using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class TankEventListener : MonoBehaviour
{
    [SerializeField] private TankEvent m_Event;
    [SerializeField] private UnityEvent m_OnEventRaised;
    public int m_TankIndex;

    public void OnEventRaised()
    {
        m_OnEventRaised.Invoke();
    }

    public void Awake()
    {
        m_Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        m_Event.UnregisterListener(this);
    }
}