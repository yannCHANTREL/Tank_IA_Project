using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Tank")]
public class TankEvent : ScriptableObject
{
    [SerializeField]
    private List<TankEventListener> m_Listeners;

    public void Raise(int TankIndex)
    {
        for (int i = m_Listeners.Count - 1; i >= 0; i--)
        {
            if (m_Listeners[i].m_TankIndex == TankIndex)
            {
                m_Listeners[i].OnEventRaised();
            }
        }
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