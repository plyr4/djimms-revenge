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
    public bool _skipIntroInEditMode;
    [SerializeField]
    [ReadOnlyInspector]
    public bool _initDone;
    [SerializeField]
    [ReadOnlyInspector]
    public bool _startInDone;
    [SerializeField]
    // [ReadOnlyInspector]
    public bool _startOnPlay;
    [SerializeField]
    // [ReadOnlyInspector]
    public bool _startOutDone;

    [SerializeField]
    // [ReadOnlyInspector]
    public bool _loadIntroDone;

    [SerializeField]
    // [ReadOnlyInspector]
    public bool _introDone;

    [SerializeField]
    // [ReadOnlyInspector]
    public bool _loadPlayDone;

    [SerializeField]
    // [ReadOnlyInspector]
    public bool _gameOver;
    [SerializeField]
    // [ReadOnlyInspector]
    public bool _win;
    [SerializeField]
    // [ReadOnlyInspector]
    public bool _gameOverRetryCloseDone;
    [SerializeField]
    // [ReadOnlyInspector]
    public bool _gameOverQuitCloseDone;
    [SerializeField]
    // [ReadOnlyInspector]
    public bool _pauseContinue;

    [SerializeField]
    // [ReadOnlyInspector]
    public bool _pauseQuit;


    [SerializeField]
    // [ReadOnlyInspector]
    public bool _playPause;

    public override void Start()
    {
        base.Start();
        Initialize(new GStateMachine(), new GStateFactory(this));
    }

    private void Reset()
    {
    }
}