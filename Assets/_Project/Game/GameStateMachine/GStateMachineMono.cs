using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class GStateMachineMono : StateMachineMono
{
    protected new GStateMachine _stateMachine
    {
        get
        {
            try
            {
                return (GStateMachine)base._stateMachine;
            }
            catch (InvalidCastException e)
            {
                Debug.LogError(e);
                return null;
            }
        }
        set => base._stateMachine = value;
    }
    
    [Header("State - Init")]
    [Header("  Configurations")]

    [Space]
    [Header("Debug")]
    [Header("  Skip Start")]
    [SerializeField]
    public bool _skipStartInEditMode;
    [SerializeField]
    [ReadOnlyInspector]
    public bool _initDone;
    [SerializeField]
    [ReadOnlyInspector]
    public bool _pressStartInDone;
    [SerializeField]
    // [ReadOnlyInspector]
    public bool _startPlay;

    public GameStateEvent _onGameStateChange;
    
    public override void Start()
    {
        base.Start();
        Initialize(new GStateMachine(), new GStateFactory(this));
    }

    private void Reset()
    {
    }
}