using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Game")]
public class GameEvent : ScriptableObject
{
    [SerializeField]
    private List<GameEventListener> m_Listeners;

    public void Raise()
    {
        for (int i = m_Listeners.Count - 1; i >= 0; i--)
        {
            m_Listeners[i].OnEventRaised();
        }
    }

    public void RegisterListener(GameEventListener listener)
    {
        m_Listeners.Add(listener);
    }

    public void UnregisterListener(GameEventListener listener)
    {
        m_Listeners.Remove(listener);
    }

}