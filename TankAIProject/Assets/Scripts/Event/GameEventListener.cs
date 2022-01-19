using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [SerializeField] private GameEvent _event;
    [SerializeField] private UnityEvent onEventRaised;

    public void OnEventRaised()
    {
        onEventRaised.Invoke();
    }

    public void Awake()
    {
        _event.RegisterListener(this);
    }

    private void OnDisable()
    {
        _event.UnregisterListener(this);
    }
}