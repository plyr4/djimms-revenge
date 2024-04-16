using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GStateMachineGame : GStateMachineMono
{
    public override void Start()
    {
        base.Start();
        Initialize(new GStateMachine(), new GStateFactory(this));
    }

    protected override void Initialize(StateMachine stateMachine, StateFactory factory)
    {
        // set up the state machine and state factory
        base.Initialize(stateMachine, factory);

        // states
        GStateBase nan = ((GStateFactory)_stateFactory).Null();
        GStateBase init = ((GStateFactory)_stateFactory).Init();
        GStateBase pressStartIn = ((GStateFactory)_stateFactory).StartIn();
        GStateBase pressStart = ((GStateFactory)_stateFactory).Start();
        GStateBase pressStartOut = ((GStateFactory)_stateFactory).StartOut();
        GStateBase loadIntro = ((GStateFactory)_stateFactory).LoadIntro();
        GStateBase intro = ((GStateFactory)_stateFactory).Intro();
        GStateBase loadPlay = ((GStateFactory)_stateFactory).LoadPlay();
        GStateBase play = ((GStateFactory)_stateFactory).Play();
        GStateBase pause = ((GStateFactory)_stateFactory).Pause();
        GStateBase pauseQuit = ((GStateFactory)_stateFactory).PauseQuit();
        GStateBase retryIn = ((GStateFactory)_stateFactory).RetryIn();
        GStateBase retry = ((GStateFactory)_stateFactory).Retry();
        GStateBase gameOver = ((GStateFactory)_stateFactory).GameOver();

        // transitions
        at(nan, init, new FuncPredicate(() =>
            true
        ));
        at(init, pressStartIn, new FuncPredicate(() =>
            _initDone
        ));
        at(pressStartIn, pressStart, new FuncPredicate(() =>
            _startInDone
        ));

        at(pressStart, pressStartOut, new FuncPredicate(() =>
            _startOnPlay
        ));

        at(pressStartOut, loadIntro, new FuncPredicate(() =>
            _startOutDone
        ));

        at(loadIntro, intro, new FuncPredicate(() =>
            _loadIntroDone
        ));

        at(intro, loadPlay, new FuncPredicate(() =>
            _introDone
        ));


        at(loadPlay, play, new FuncPredicate(() =>
            _loadPlayDone
        ));

        at(play, pause, new FuncPredicate(() =>
            _playPause
        ));

        at(pause, play, new FuncPredicate(() =>
            _pauseContinue
        ));

        at(pause, pauseQuit, new FuncPredicate(() =>
            _pauseQuit
        ));

        at(pause, retryIn, new FuncPredicate(() =>
            false
        ));

        at(retryIn, retry, new FuncPredicate(() =>
            false
        ));

        at(retry, play, new FuncPredicate(() =>
            false
        ));

        at(play, gameOver, new FuncPredicate(() =>
            _gameOver
        ));

        _stateMachine.SetState(nan);
    }

    public void HandleTransitionCloseDoneEvent(GenericEventOpts opts)
    {
        switch (_stateMachine._current._state)
        {
            case GStateStartOut:
                _startOutDone = true;
                break;
            case GStateStart:
                if (Application.isPlaying)
                {
                    Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
                }

                break;

            case GStateIntro:
                _introDone = true;

                break;
            case GStatePauseQuit:
            case GStateGameOver:
                // if (opts._isGameOverQuit)
                // {
                //     _gameOverQuitCloseDone = true;
                // }
                //
                // if (opts._isGameOverRetry)
                // {
                //     _gameOverRetryCloseDone = true;
                // }

                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager
                    .GetActiveScene().name);
                break;
        }
    }

    public void HandleTransitionOpenDoneEvent(GenericEventOpts opts)
    {
        switch (_stateMachine._current._state)
        {
            case GStateLoadPlay:
                _loadPlayDone = true;
                break;
        }
    }

    public void HandlePlayerCollisionOnHit(GenericEventOpts opts)
    {
        switch (_stateMachine._current._state)
        {
            case GStatePlay:
                _gameOver = true;
                break;
        }
    }

    public void HandleStartMenuPlay(GenericEventOpts opts)
    {
        switch (_stateMachine._current._state)
        {
            case GStateStart:
                _startOnPlay = true;
                break;
        }
    }

    public void HandleStartMenuOutDone(GenericEventOpts opts)
    {
        switch (_stateMachine._current._state)
        {
            case GStateStartOut:
                _startOutDone = true;
                break;
        }
    }

    public void HandleStartMenuQuit(GenericEventOpts opts)
    {
        ScreenTransition.Instance.Close();
    }

    public void HandleGameOverRetry(GenericEventOpts opts)
    {
        ScreenTransition.Instance.Close();
    }

    public void HandleGameOverQuit(GenericEventOpts opts)
    {
        ScreenTransition.Instance.Close();
    }

    public void HandleGameOverWin(GenericEventOpts opts)
    {
        _gameOver = true;
        _win = true;
    }

    public void OnKeyEscape(InputAction.CallbackContext context)
    {
        if (!context.action.WasPressedThisFrame()) return;
        switch (_stateMachine._current._state)
        {
            case GStatePlay:
                _playPause = true;
                break;
            case GStatePause:
                _pauseContinue = true;
                break;
        }
    }

    public void OnButtonStart(InputAction.CallbackContext context)
    {
        if (!context.action.WasPressedThisFrame()) return;
        switch (_stateMachine._current._state)
        {
            case GStatePlay:
                _playPause = true;
                break;
            case GStatePause:
                _pauseContinue = true;
                break;
        }
    }

    public void HandlePauseContinue(GenericEventOpts opts)
    {
        _pauseContinue = true;
    }

    public void HandlePauseQuit(GenericEventOpts opts)
    {
        _pauseQuit = true;
    }

    public void HandleSkipIntro(GenericEventOpts opts)
    {
        ScreenTransition.Instance.Close();
    }

    public void HandleLoadIntroDone(GenericEventOpts opts)
    {
        _loadIntroDone = true;
    }

    public bool Won()
    {
        return _win;
    }
}