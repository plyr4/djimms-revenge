using UnityEngine;

[CreateAssetMenu(menuName = "Basic Event", fileName = "New Basic Event")]
public class BasicEvent : GameEventBase
{
    public void Invoke(float f)
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
            (listener.Value as BasicEventListener).RaiseEvent(f);
        }
    }
    public void Invoke(string s)
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
            (listener.Value as BasicEventListener).RaiseEvent(s);
        }
    }
}