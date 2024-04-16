using UnityEngine;

[CreateAssetMenu(menuName = "Generic Event", fileName = "New Generic Event")]
public class GenericEvent : GameEventBase
{
    public void Invoke(GenericEventOpts opts)
    {
        if (_debug && Application.isPlaying)
        {
            Debug.Log($"BasicEvent: Invoked BasicEvent listeners: num_listeners({_listeners.Values.Count})");
        }

        foreach (var listener in _listeners)
        {
            if (_debug && Application.isPlaying)
            {
                Debug.Log($"BasicEvent: RaiseEvent BasicEvent listener: name({listener.Value.gameObject.name})", listener.Value.gameObject);
            }
            (listener.Value as GenericEventListener).RaiseEvent(opts);
        }
    }
}