using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Tank")]
public class TankEvent : ScriptableObject
{
    [SerializeField]
    private List<TankEventListener> m_Listeners;

    public void Raise(int tankIndex)
    {
        foreach (TankEventListener listener in m_Listeners) { listener.OnEventRaised(tankIndex); }
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