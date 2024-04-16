using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class Intro : MonoBehaviour
{
    [SerializeField]
    private GenericEvent _skipIntroEvent;
    [SerializeField]
    private GenericEvent _loadIntroDoneEvent;
    [SerializeField]
    private GameObject _menuEtcViewParent;
    [SerializeField]
    private GameObject _dialoguesViewParent;

    // [SerializeField]
    // private GameObject _scaler;
    // private Sequence _menuTween;
    [SerializeField]
    private float _fadeInDuration = 0.5f;
    [SerializeField]
    private Ease _fadeInEase = Ease.InOutQuad;
    [SerializeField]
    private GameObject _inGameStuffViewParent;
    [SerializeField]
    [ReadOnlyInspector]
    private Dialogue _activeDialogue;

    [SerializeField]
    private GameObject _geniePivotScaler;
    [SerializeField]
    private float _geniePivotScaleDuration = 0.5f;
    [SerializeField]
    private Ease _genieScaleEase = Ease.InOutQuad;
    private Sequence _geniePivotTween;

    [SerializeField]
    private Transform _genieMove1Destination;
    [SerializeField]
    private Ease _genieMoveEase = Ease.InOutQuad;
    private Sequence _genieMoveTween;

    [SerializeField]
    private Transform _genieMove2Destination;

    [SerializeField]
    private GameObject _genieShakeParent;
    private Sequence _genieShakeTween;
    [SerializeField]
    private Ease _genieShakeEase = Ease.InOutBounce;
    [SerializeField]
    private Vector3 _genieShakeStrength = new Vector3(0.01f, 0.08f, 0f);

    [SerializeField]
    private Transform _playerMoveParent;
    [SerializeField]
    private Transform _playerRotationParent;
    [SerializeField]
    private Animator _animator;
    public float _animationSpeed = 2f;
    public float _animationMotionSpeed = 2f;
    [SerializeField]
    private Transform _playerMoveOrigin;
    [SerializeField]
    private Transform _playerMoveInitialDestination;
    [SerializeField]
    private float _playerMoveInitialDuration = 3f;

    [SerializeField]
    private Ease _playerMoveEase = Ease.InOutQuad;
    private Sequence _playerMoveTween;
    [SerializeField]
    private Transform _playerMove1Destination;

    [SerializeField]
    private Ease _playerJumpEase = Ease.OutBounce;
    private Sequence _playerJumpTween;
    [SerializeField]
    private Transform _playerJumpParent;

    [SerializeField]
    private float _playerJumpHeight = 1f;
    [SerializeField]
    private float _playerJumpDuration = 0.5f;

    [SerializeField]
    private float _playerMove1Duration = 0.5f;

    [SerializeField]
    private Transform _playerMove2Destination;
    [SerializeField]
    private float _playerMove2Duration = 2f;
    [SerializeField]
    private Transform _playerMove3Destination;
    [SerializeField]
    private float _playerMove3Duration = 2f;
    [SerializeField]
    private int _animIDSpeed;
    [SerializeField]
    private int _animIDMotionSpeed;

    [SerializeField]
    public DialogueParams _playDialogue;


    public Dialogue _playerDialogue;
    public Dialogue _genieDialogue;
    
    private void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    }

    private void Start()
    {
        _menuEtcViewParent.SetActive(false);
        _dialoguesViewParent.SetActive(false);
        _inGameStuffViewParent.SetActive(false);
        AssignAnimationIDs();
    }

    public void HandleSkip()
    {
        if (_activeDialogue != null)
        {
            _activeDialogue.CloseWindow(null, true, false);
        }
        if (_playerMoveTween != null)
        {
            _playerMoveTween.Kill();
        }

        if (_playerJumpTween != null)
        {
            _playerJumpTween.Kill();
        }

        if (_genieMoveTween != null)
        {
            _genieMoveTween.Kill();
        }

        if (_genieShakeTween != null)
        {
            _genieShakeTween.Kill();
        }
        
        StopAllCoroutines();
        FinishIntro();
    }

    public void FinishIntro()
    {
        GenericEventOpts opts = new GenericEventOpts
        {
        };
        _skipIntroEvent.Invoke(opts);
    }

    public void HandleOnGameStateChange(GenericEventOpts opts)
    {
        switch (opts._newState)
        {
            case GStateLoadIntro _:
                _menuEtcViewParent.SetActive(true);
                _dialoguesViewParent.SetActive(true);
                _inGameStuffViewParent.SetActive(true);
                // _scaler.transform.localScale = Vector3.zero;
                _playerMoveParent.position = _playerMoveOrigin.position;
                StartCoroutine(finishLoading());
                break;
            case GStateIntro _:
                gameObject.SetActive(true);
                _menuEtcViewParent.SetActive(true);
                _dialoguesViewParent.SetActive(true);
                _inGameStuffViewParent.SetActive(true);

                // trigger the player walking in animation first
                // then start the dialogue when they walk in and walk up to the lamp
                // StartDialogue();


                if (_playerMoveTween != null)
                {
                    _playerMoveTween.Complete();
                    _playerMoveTween.Kill();
                    _playerMoveTween = null;
                }

                switch (opts._newState)
                {
                    case GStateIntro _:
                        StartDialogue();
                        break;
                }
                
                _playerMoveTween = DOTween.Sequence();
                _playerMoveTween.SetUpdate(true);
                _playerMoveTween.Insert(0f, _playerMoveParent.transform
                    .DOMove(_playerMoveInitialDestination.position, _playerMoveInitialDuration)
                    .SetUpdate(true)
                    .SetDelay(0f)
                    .SetEase(_playerMoveEase)
                    .OnStart(
                        () =>
                        {
                            _animator.SetFloat(_animIDSpeed, _animationSpeed);
                            _animator.SetFloat(_animIDMotionSpeed, _animationMotionSpeed);
                        })
                    .OnComplete(
                        () =>
                        {

                            _animator.SetFloat(_animIDSpeed, 0f);
                            _animator.SetFloat(_animIDMotionSpeed, 0f);
                        })
                );


                break;
            case GStateLoadPlay _:
                _menuEtcViewParent.SetActive(false);
                // _dialoguesViewParent.SetActive(false);
                _inGameStuffViewParent.SetActive(false);
                
                _genieDialogue.gameObject.SetActive(false);
                _playerDialogue.gameObject.SetActive(false);

                StartCoroutine(delayedShowPlay());
                
                if (_activeDialogue != null)
                {
                    _activeDialogue.CloseWindow(null, true, false);
                }
                
                // _scaler.transform.localScale = Vector3.zero;
                break;

            case GStatePlay _:
                _menuEtcViewParent.SetActive(false);
                // _dialoguesViewParent.SetActive(false);
                _inGameStuffViewParent.SetActive(false);
                
                // _scaler.transform.localScale = Vector3.zero;
                break;
            default:
                break;
        }
    }

    IEnumerator delayedShowPlay()
    {
        yield return new WaitForSecondsRealtime(2.5f);

        _activeDialogue = _playDialogue._dialogue;
        _activeDialogue.ShowWindow(_playDialogue, true);
    }
    
    IEnumerator finishLoading()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        GenericEventOpts opts = new GenericEventOpts
        {
        };
        _loadIntroDoneEvent.Invoke(opts);
    }

    [SerializeField]
    private DialogueParams[] _dialogues;
    [SerializeField]
    private int _currentDialogue;

    void StartDialogue()
    {
        StartCoroutine(startDialogue());
    }

    IEnumerator startDialogue()
    {
        yield return new WaitForSecondsRealtime(1f);
        _currentDialogue = 0;
        NextDialogue(null);
    }

    // cooldown for clicking next
    private bool _nextReady = true;
    private float _nextCooldownDuration = 0.2f;

    public void HandleNext()
    {
        if (_nextReady)
        {
            NextDialogue(_activeDialogue);
            _nextReady = false;
            StartCoroutine(NextCooldown());
        }
    }

    private IEnumerator NextCooldown()
    {
        yield return new WaitForSecondsRealtime(_nextCooldownDuration);
        _nextReady = true;
    }

    // inbound from the dialogue window
    public void NextDialogue(Dialogue dialogue)
    {
        if (dialogue != null && dialogue != _activeDialogue) return;


        DialogueParams nextDialogueParams = null;
        Dialogue nextDialogue = null;
        if (_currentDialogue + 1 < _dialogues.Length)
        {
            nextDialogueParams = _dialogues[_currentDialogue + 1];
            nextDialogue = nextDialogueParams._dialogue;
        }

        if (dialogue != null)
        {
            // if the animator is still playing, skip it, wait for another click

            if (dialogue.Skip())
            {
                if (_playerMoveTween != null)
                {
                    _playerMoveTween.Complete();
                }

                if (_playerJumpTween != null)
                {
                    _playerJumpTween.Complete();
                }

                if (_genieMoveTween != null)
                {
                    _genieMoveTween.Complete();
                }

                if (_genieShakeTween != null)
                {
                    _genieShakeTween.Complete();
                }

                return;
            }

            if (nextDialogueParams != null && nextDialogueParams._triggerGenieShake)
            {
                if (_genieShakeTween != null)
                {
                    _genieShakeTween.Complete();
                    _genieShakeTween.Kill();
                    _genieShakeTween = null;
                }

                _genieShakeTween = DOTween.Sequence();
                _genieShakeTween.SetUpdate(true);
                _genieShakeTween.Insert(0f,
                        _genieShakeParent.transform.DOShakePosition(_geniePivotScaleDuration, _genieShakeStrength)
                            // .SetFadeOut(true)
                            .SetUpdate(true)
                            .SetDelay(0f)
                            .SetEase(_genieMoveEase))
                    .OnComplete(
                        () => { });
            }

            dialogue.ConfirmNext(nextDialogue);
            _currentDialogue++;
        }


        if (nextDialogueParams != null && nextDialogueParams._triggerPlayerMove1)
        {
            if (_playerMoveTween != null)
            {
                _playerMoveTween.Complete();
                _playerMoveTween.Kill();
                _playerMoveTween = null;
            }

            _playerMoveTween = DOTween.Sequence();
            _playerMoveTween.SetUpdate(true);
            _playerMoveTween.Insert(0f, _playerMoveParent.transform
                    .DOMove(_playerMove1Destination.position, _playerMove1Duration)
                    .SetUpdate(true)
                    .SetDelay(0f)
                    .SetEase(_playerMoveEase)
                    .OnStart(
                        () =>
                        {
                            _animator.SetFloat(_animIDSpeed, _animationSpeed);
                            _animator.SetFloat(_animIDMotionSpeed, _animationMotionSpeed);
                        })
                    .OnComplete(
                        () =>
                        {
                            _animator.SetFloat(_animIDSpeed, 0f);
                            _animator.SetFloat(_animIDMotionSpeed, 0f);
                        })
                )
                ;
        }

        if (nextDialogueParams != null && nextDialogueParams._triggerPlayerJump)
        {
            if (_playerJumpTween != null)
            {
                _playerJumpTween.Complete();
                _playerJumpTween.Kill();
                _playerJumpTween = null;
            }

            _playerJumpTween = DOTween.Sequence();
            _playerJumpTween.SetUpdate(true);
            _playerJumpTween.Insert(0f, _playerJumpParent.transform
                    .DOLocalMove(new Vector3(0f, _playerJumpHeight, 0f), _playerJumpDuration)
                    .SetUpdate(true)
                    .SetRelative(true)
                    .SetDelay(0f)
                    .SetLoops(4, LoopType.Yoyo)
                    .SetEase(_playerJumpEase)
                    .OnStart(
                        () => { })
                    .OnComplete(
                        () => { })
                )
                ;
        }

        if (nextDialogueParams != null && nextDialogueParams._triggerPlayerMove2)
        {
            if (_playerMoveTween != null)
            {
                _playerMoveTween.Complete();
                _playerMoveTween.Kill();
                _playerMoveTween = null;
            }

            _playerMoveTween = DOTween.Sequence();
            _playerMoveTween.SetUpdate(true);
            _playerMoveTween.Insert(0f, _playerMoveParent.transform
                .DOMove(_playerMove2Destination.position, _playerMove2Duration)
                .SetUpdate(true)
                .SetDelay(0f)
                .SetEase(_playerMoveEase)
                .OnStart(
                    () =>
                    {
                        _animator.SetFloat(_animIDSpeed, _animationSpeed);
                        _animator.SetFloat(_animIDMotionSpeed, _animationMotionSpeed);
                    })
                .OnComplete(
                    () =>
                    {
                        _animator.SetFloat(_animIDSpeed, 0f);
                        _animator.SetFloat(_animIDMotionSpeed, 0f);
                    })
            );
        }

        if (nextDialogueParams != null && nextDialogueParams._triggerPlayerMove3)
        {
            if (_playerMoveTween != null)
            {
                _playerMoveTween.Complete();
                _playerMoveTween.Kill();
                _playerMoveTween = null;
            }

            _playerMoveTween = DOTween.Sequence();
            _playerMoveTween.SetUpdate(true);
            _playerMoveTween.Insert(0f, _playerMoveParent.transform
                .DOMove(_playerMove3Destination.position, _playerMove3Duration)
                .SetUpdate(true)
                .SetDelay(0f)
                .SetEase(_playerMoveEase)
                .OnStart(
                    () =>
                    {
                        _animator.SetFloat(_animIDSpeed, _animationSpeed);
                        _animator.SetFloat(_animIDMotionSpeed, _animationMotionSpeed);
                    })
                .OnComplete(
                    () =>
                    {
                        _animator.SetFloat(_animIDSpeed, 0f);
                        _animator.SetFloat(_animIDMotionSpeed, 0f);
                    })
            );
        }

        if (nextDialogueParams != null && nextDialogueParams._triggerGenieAppear)
        {
            if (_geniePivotTween != null)
            {
                _geniePivotTween.Complete();
                _geniePivotTween.Kill();
                _geniePivotTween = null;
            }

            _geniePivotTween = DOTween.Sequence();
            _geniePivotTween.SetUpdate(true);
            _geniePivotTween.Insert(0f, _geniePivotScaler.transform.DOScale(Vector3.one, _geniePivotScaleDuration)
                    .SetUpdate(true)
                    .SetDelay(0f)
                    .SetEase(_genieScaleEase))
                .OnComplete(
                    () => { });
        }

        if (nextDialogueParams != null && nextDialogueParams._triggerGenieMove1)
        {
            if (_genieMoveTween != null)
            {
                _genieMoveTween.Complete();
                _genieMoveTween.Kill();
                _genieMoveTween = null;
            }

            _genieMoveTween = DOTween.Sequence();
            _genieMoveTween.SetUpdate(true);
            _genieMoveTween.Insert(0f, _geniePivotScaler.transform
                    .DOMove(_genieMove1Destination.position, _geniePivotScaleDuration)
                    .SetUpdate(true)
                    .SetDelay(0f)
                    .SetEase(_genieMoveEase))
                .OnComplete(
                    () => { });
        }

        if (nextDialogueParams != null && nextDialogueParams._triggerGenieMove2)
        {
            if (_genieMoveTween != null)
            {
                _genieMoveTween.Complete();
                _genieMoveTween.Kill();
                _genieMoveTween = null;
            }

            _genieMoveTween = DOTween.Sequence();
            _genieMoveTween.SetUpdate(true);
            _genieMoveTween.Insert(0f, _geniePivotScaler.transform
                    .DOMove(_genieMove2Destination.position, _geniePivotScaleDuration)
                    .SetUpdate(true)
                    .SetDelay(0f)
                    .SetEase(_genieMoveEase))
                .OnComplete(
                    () => { });
        }

        if (nextDialogueParams != null && nextDialogueParams._triggerGenieAttack)
        {
            // spawn a shit load of spirits that grow really fast
        }


        if (_currentDialogue < _dialogues.Length)
        {
            _activeDialogue = _dialogues[_currentDialogue]._dialogue;
            _activeDialogue.ShowWindow(_dialogues[_currentDialogue]);
        }
        else
        {
            FinishIntro();
        }
    }
}