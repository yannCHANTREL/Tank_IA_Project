using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Tank")]
public class TankEvent : ScriptableObject
{
    [SerializeField]
    private List<TankEventListener> m_Listeners;

    public void Raise(int TankIndex)
    {
        m_Listeners[TankIndex - 1].OnEventRaised();
    }

    public void RegisterListener(TankEventListener listener)
    {
        m_Listeners.Add(listener);
    }

    public void UnregisterListener(TankEventListener listener)
    {
        m_Listeners.Remove(listener);
    }

}