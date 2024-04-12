using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[Serializable]
public class UnityEventActionCallback : UnityEvent<InputAction.CallbackContext> { }
public class PlayerInputActionEventListener : MonoBehaviour
{
    [SerializeField] private bool _listenerRegistered = false;
    [SerializeField] protected PlayerInputActionEvent _inputEvent;
    [SerializeField] protected UnityEventActionCallback _unityEventActionCallback;
    private int _registerRetries = 3;
    private int _registerAttempts = 0;

    private void Awake()
    {
        StartCoroutine(RegisterAsync());
    }

    private void OnEnable()
    {
        StartCoroutine(RegisterAsync());
    }

    private void OnDisable()
    {
        _inputEvent?.Deregister(this);
        _listenerRegistered = false;
    }

    public virtual void RaiseEvent(InputAction.CallbackContext context)
    {
        _unityEventActionCallback?.Invoke(context);
    }

    private IEnumerator RegisterAsync()
    {
        // halt if listener is already registered
        if (_listenerRegistered) yield break;

        // attempt to register the listener
        // this is spread across frames to account for out-of-order scene initialization
        while (!_listenerRegistered && _registerAttempts < _registerRetries)
        {
            _registerAttempts++;

            if (_inputEvent == null)
            {
                Debug.LogWarning($"EventListener: _inputEvent is null: name({gameObject.name}), attempt number ({_registerAttempts}/{_registerRetries})", gameObject);

                // attempt to register again next frame
                yield return null;
                continue;
            }

            if (_inputEvent._debug && Application.isPlaying)
            {
                Debug.Log($"EventListener: Register listener: name({gameObject.name})", gameObject);
            }

            _inputEvent?.Register(this);
            _listenerRegistered = true;
        }

        if (!_listenerRegistered)
        {
            Debug.LogWarning($"EventListener: unable to register _inputEvent after ({_registerAttempts}/{_registerRetries}) attempts: name({gameObject.name})", gameObject);
        }
    }
}
