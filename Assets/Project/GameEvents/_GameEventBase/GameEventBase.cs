using System.Collections.Concurrent;
using UnityEngine;

public class GameEventBase : ScriptableObject
{
    protected ConcurrentDictionary<int, GameEventListenerBase> _listeners = new ConcurrentDictionary<int, GameEventListenerBase>();
    public bool _debug;
    virtual public void Deregister(GameEventListenerBase listener) => _listeners.TryRemove(listener.gameObject.GetInstanceID(), out _);
    virtual public void Register(GameEventListenerBase listener) => _listeners.TryAdd(listener.gameObject.GetInstanceID(), listener);
}