using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(
    menuName = "SO Variables/Game Event",
    fileName = "game event"
    )]
public class GameEvent : ScriptableObject
{
    private List<GameEventListener> listeners =
        new List<GameEventListener>();

    public void raise()
    {
        foreach (GameEventListener gel in listeners)
        {
            gel.OnEventRaised();
        }
    }

    public void register(GameEventListener listener)
    {
        listeners.Add(listener);
    }
    public void unregister(GameEventListener listener)
    {
        listeners.Remove(listener);
    }
}
