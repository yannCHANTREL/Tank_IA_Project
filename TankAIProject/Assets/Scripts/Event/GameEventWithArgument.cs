using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Game With Argument")]
public class GameEventWithArgument : ScriptableObject
{
    [SerializeField]
    private List<GameEventListenerWithArgument> m_Listeners;

    public void Raise(GameObject gameObject)
    {
        for (int i = m_Listeners.Count - 1; i >= 0; i--)
        {
            m_Listeners[i].OnEventRaised(gameObject);
        }
    }

    public void RegisterListener(GameEventListenerWithArgument listener)
    {
        m_Listeners.Add(listener);
    }

    public void UnregisterListener(GameEventListenerWithArgument listener)
    {
        m_Listeners.Remove(listener);
    }

}