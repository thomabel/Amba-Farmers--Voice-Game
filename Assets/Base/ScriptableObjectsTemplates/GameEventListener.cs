using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A component class that reacts to an GameEvent scriptable object.
/// It uses the UnityEvent class to connect a method to in the editor.
/// See this talk for more information: https://youtu.be/raQ3iHhE_Kk
/// </summary>
public class GameEventListener : MonoBehaviour
{
    public GameEvent Event;
    public UnityEvent Response;

    public void OnEventRaised()
    {
        Response.Invoke();
    }

    private void OnEnable()
    {
        Event.register(this);
    }
    private void OnDisable()
    {
        Event.unregister(this);
    }
}
