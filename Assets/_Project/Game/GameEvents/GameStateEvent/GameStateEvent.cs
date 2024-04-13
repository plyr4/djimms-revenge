using UnityEngine;

[CreateAssetMenu(menuName = "Game State Event", fileName = "New Game State Event")]
public class GameStateEvent : GameEventBase
{
    public void Invoke(GStateBase previousState, GStateBase nextState)
    {
        if (_debug && Application.isPlaying)
        {
            Debug.Log($"GameStateEvent: Invoked GameStateEvent listeners: num_listeners({_listeners.Values.Count})");
        }

        foreach (var listener in _listeners)
        {
            if (_debug && Application.isPlaying)
            {
                Debug.Log($"GameStateEvent: RaiseEvent GameStateEvent listener: name({listener.Value.gameObject.name})",
                    listener.Value.gameObject);
            }

            (listener.Value as GameStateEventListener).RaiseEvent(previousState, nextState);
        }
    }
}