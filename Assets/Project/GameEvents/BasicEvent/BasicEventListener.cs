using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class UnityEventFloat : UnityEvent<float> { }
[Serializable]
public class UnityEventString : UnityEvent<string> { }
public class BasicEventListener : GameEventListenerBase
{
    [SerializeField] protected UnityEventFloat _unityEventFloat;
    [SerializeField] protected UnityEventString _unityEventString;
    public void RaiseEvent(float f)
    {
        _unityEvent?.Invoke();
        _unityEventFloat?.Invoke(f);
    }
    public void RaiseEvent(string s)
    {
        _unityEvent?.Invoke();
        _unityEventString?.Invoke(s);
    }
}
