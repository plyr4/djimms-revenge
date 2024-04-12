using System.Collections.Concurrent;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "InputSystem Event", fileName = "New InputSystem Event")]
public class InputSystemEvent : ScriptableObject
{
    private ConcurrentDictionary<int, InputSystemEventListener> _listeners = new ConcurrentDictionary<int, InputSystemEventListener>();
    public bool _debug;

    public void Invoke(PlayerInput playerInput = null)
    {
        if (_debug && Application.isPlaying)
        {
            Debug.Log($"InputSystemEvent: Invoked InputSystemEvent listeners with: num_listeners({_listeners.Values.Count})");
        }

        foreach (var listener in _listeners)
        {
            if (_debug && Application.isPlaying)
            {
                Debug.Log($"InputSystemEvent: RaiseEvent InputSystemEvent listener: name({listener.Value.gameObject.name})", listener.Value.gameObject);
            }

            listener.Value?.RaiseEvent(playerInput);
        }
    }

    public void Deregister(InputSystemEventListener listener) => _listeners.TryRemove(listener.GetInstanceID(), out _);
    public void Register(InputSystemEventListener listener) => _listeners.TryAdd(listener.GetInstanceID(), listener);
}