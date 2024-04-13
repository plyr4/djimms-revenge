using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

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
                DebugLogger.LogError($"using the wrong context type: InvalidCastException: {e.Message}");
                return null;
            }
        }
        set => base._stateMachine = value;
    }
    
    [Header("State - LoadLevel")]
    [SerializeField]
    public LevelConfig _currentLevel;
    [Space]
    [Header("State - Init")]
    [Header("  Configurations")]
    [SerializeField]
    public Scenes _coreScenes;
    [SerializeField]
    public float _minimumCoreScenesLoadTime = 1f;
    [SerializeField]
    public bool _skipCoreScenes;
    [Header("  Framerate")]
    [SerializeField]
    public bool _setTargetFrameRateInBuild = true;
    [SerializeField]
    public int _buildTargetFrameRate = 144;
    [Header("  Rendering")]
    [SerializeField]
    public bool _setSRPBatcherInBuild = true;
    [SerializeField]
    public bool _buildSRPBatcher;
    [SerializeField]
    public bool _setVSyncCountInBuild;
    [SerializeField]
    public int _buildVSyncCount = 0;
    [Header("  DOTween")]
    [SerializeField]
    public int _DOTweenTweenersCapacity = 1500;
    [SerializeField]
    public int _DOTweenSequencesCapacity = 100;
    [Space]
    [Header("Debug")]
    [Header("  Skip Start")]
    [SerializeField]
    public bool _skipStartInEditMode;
    [SerializeField]
    public bool _skipStartInBuild;
    [SerializeField]
    public GameState _skipStartImmediateLoadState = GameState.Load_Level_2_Config;
    [SerializeField]
    [ReadOnlyInspector]
    public bool _initDone;
    [SerializeField]
    [ReadOnlyInspector]
    public bool _pressStartInDone;
    public UniversalRenderPipelineAsset _urp => (UniversalRenderPipelineAsset)GraphicsSettings.currentRenderPipeline;

    public override void Start()
    {
        base.Start();
        Initialize(new GStateMachine(), new GStateFactory(this));
    }

    private void Reset()
    {
    }
}