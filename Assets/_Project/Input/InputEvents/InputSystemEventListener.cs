using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[Serializable]
public class UnityEventInputSystem : UnityEvent<PlayerInput> { }
public class InputSystemEventListener : MonoBehaviour
{
    [SerializeField] protected InputSystemEvent _inputSystemEvent;
    [SerializeField] protected UnityEventInputSystem _unityEventInputSystem;

    private void Awake() => _inputSystemEvent?.Register(this);
    private void OnEnable() => _inputSystemEvent?.Register(this);
    private void OnDisable() => _inputSystemEvent?.Deregister(this);
    public virtual void RaiseEvent(PlayerInput playerInput = null)
    {
        _unityEventInputSystem?.Invoke(playerInput);
    }
}
