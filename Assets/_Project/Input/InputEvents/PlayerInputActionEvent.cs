using System.Collections.Concurrent;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Player Input Action Event", fileName = "New Player Input Action Event")]
public class PlayerInputActionEvent : ScriptableObject
{
    private ConcurrentDictionary<int, PlayerInputActionEventListener> _listeners = new ConcurrentDictionary<int, PlayerInputActionEventListener>();
    public bool _debug;

    public void Deregister(PlayerInputActionEventListener listener) => _listeners.TryRemove(listener.GetInstanceID(), out _);

    public void Register(PlayerInputActionEventListener listener) => _listeners.TryAdd(listener.GetInstanceID(), listener);

    public void Invoke(InputAction.CallbackContext context)
    {
#if UNITY_EDITOR
        if (_debug && Application.isPlaying)
        {
            Debug.Log($"PlayerInputActionEvent: Invoked PlayerInputActionEvent listeners: num_listeners({_listeners.Values.Count})");
        }
#endif

        foreach (var listener in _listeners)
        {
#if UNITY_EDITOR
            if (_debug && Application.isPlaying)
            {
                Debug.Log($"PlayerInputActionEvent: RaiseEvent PlayerInputActionEvent listener: name({listener.Value.gameObject.name})", listener.Value.gameObject);
            }
#endif
            listener.Value?.RaiseEvent(context);
        }
    }
}