using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class GameEventListenerWithArgument : MonoBehaviour
{
    [SerializeField] private GameEventWithArgument m_Event;
    [SerializeField] private UnityEvent<GameObject> m_OnEventRaised;

    public void OnEventRaised(GameObject gameObject)
    {
        m_OnEventRaised.Invoke(gameObject);
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