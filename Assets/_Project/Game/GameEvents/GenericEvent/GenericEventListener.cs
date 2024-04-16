using System;
using UnityEngine;
using UnityEngine.Events;

public class GenericEventOpts
{
    public StateBase _newState;
    public PlayerCollision _playerCollision;
    public DialogueParams _dialogueParams;
}

[Serializable]
public class UnityEventGeneric : UnityEvent<GenericEventOpts>
{
}

public class GenericEventListener : GameEventListenerBase
{
    [SerializeField]
    protected UnityEventGeneric _unityEventGeneric;

    public void RaiseEvent(GenericEventOpts opts)
    {
        _unityEvent?.Invoke();
        _unityEventGeneric?.Invoke(opts);
    }
}