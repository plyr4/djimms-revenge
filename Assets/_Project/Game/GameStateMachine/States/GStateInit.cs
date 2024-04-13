using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class GStateInit : GStateBase
{
    public GStateInit(StateMachineMono context, StateFactory factory) : base(context, factory)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        if (_context == null) return;

        InitializeGame();
    }

    public override void OnExit()
    {
        base.OnExit();

        if (_context == null) return;
        
        _context._initDone = false;
    }

    private void InitializeGame()
    {
        // system configurations
        SetSystemConfigurations();

        if (!_context._skipCoreScenes)
        {
            if (_context._coreScenes == null)
            {
                DebugLogger.LogWarning(
                    "_coreScenes not initialized and _skipCoreScenes is false, is this intentional? skipping");
                return;
            }

            _context._coreScenes.LoadAllScenes(() =>
            {
                // delay to next frame to account for immediate loading (ex: core scenes were pre-loaded)
                _context.StartCoroutine(finishLoadingCoreScenesNextFrame());
            }, _context._minimumCoreScenesLoadTime);
        }
        else
        {
            // delay to next frame to account for immediate loading (ex: core scenes were pre-loaded)
            _context.StartCoroutine(finishLoadingCoreScenesNextFrame());
        }
    }

    private IEnumerator finishLoadingCoreScenesNextFrame()
    {
        yield return null;
        FinishLoadingCoreScenes();
    }


    private void FinishLoadingCoreScenes()
    {
#if UNITY_EDITOR
        // ShouldEditorSkipStartMenu controls functionality for letting the Unity Editor immediately jump into gameplay 
        // 1. immediately loads the default levelScenes (next frame)
        // 2. skips state to Play Level
        if (_context._skipStartInEditMode)
        {
            _context.StartCoroutine(doEditorImmediateLoadNextFrame());
            return;
        }
#endif
        if (_context._skipStartInBuild)
        {
            _context.StartCoroutine(doEditorImmediateLoadNextFrame());
            return;
        }

        // entrypoint to the actual interactive application
        _context._initDone = true;
    }

    
    private IEnumerator doEditorImmediateLoadNextFrame()
    {
        yield return null;
        if (_context._currentLevel._scenes != null)
        {
            _context._currentLevel._scenes.LoadAllScenes(() =>
            {
                _context._initDone = true;

                // todo: set up transitions to jump states
                // updateState(_context._editorImmediateLoadState);
            });
        }
        else
        {
            _context._initDone = true;

            // DebugLogger.LogWarning("GameManager: _level._scenes is null");
            // DebugLogger.LogWarning("GameManager: performing manual invoke for LevelIsReady(ready)");
            // updateState(_context._editorImmediateLoadState);
        }
    }
    
    // System Configurations
    private void SetSystemConfigurations()
    {
        DOTween.SetTweensCapacity(_context._DOTweenTweenersCapacity, _context._DOTweenSequencesCapacity);

        // build only configurations
#if !UNITY_EDITOR
        if (_setTargetFrameRateInBuild) SetTargetFrameRate();
        if (_setSRPBatcherInBuild) SetUseSRPBatcher();
        if (_setVSyncCountInBuild) SetVSyncCount();
#else
        // ignore unused reference warnings
        bool[] _ =
        {
            _context._setTargetFrameRateInBuild, _context._setSRPBatcherInBuild, _context._setVSyncCountInBuild
        };
#endif
    }

    private void SetTargetFrameRate()
    {
        Application.targetFrameRate = _context._buildTargetFrameRate;
    }

    private void SetUseSRPBatcher()
    {
        _context._urp.useSRPBatcher = _context._buildSRPBatcher;
    }

    private void SetVSyncCount()
    {
        QualitySettings.vSyncCount = _context._buildVSyncCount;
    }

    private string startupConfigurationsToString()
    {
        string configurations = "<GAME STARTUP CONFIGURATIONS>\n\n";
        configurations += $"{nameof(Application.targetFrameRate)}: ({Application.targetFrameRate})\n";
        configurations += $"{nameof(QualitySettings.vSyncCount)}: ({QualitySettings.vSyncCount})\n";
        configurations += $"{nameof(_context._urp.useSRPBatcher)}: ({_context._urp.useSRPBatcher})\n";
        configurations += $"main_camera.environment.volumeUpdateMode: ({Camera.main.GetVolumeFrameworkUpdateMode()})\n";
        configurations +=
            $"DOTween.SetTweensCapacity(tweeners={_context._DOTweenTweenersCapacity}, sequences={_context._DOTweenSequencesCapacity})\n";
        return configurations;
    }
}