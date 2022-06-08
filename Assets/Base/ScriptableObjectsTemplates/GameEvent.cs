using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Stores a list of event listeners and activates all of their events
/// whenever the event is raised. Anything that references this object
/// can raise the event.
/// See this talk for more information: https://youtu.be/raQ3iHhE_Kk
/// </summary>
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
