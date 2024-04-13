using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class UnityEventGameState : UnityEvent<GStateBase, GStateBase> { }
public class GameStateEventListener : GameEventListenerBase
{
    [SerializeField] protected UnityEventGameState _unityEventGameState;
    public void RaiseEvent(GStateBase previousState, GStateBase newState)
    {
        _unityEvent?.Invoke();
        _unityEventGameState?.Invoke(previousState, newState);
    }
}
