using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using Febucci.UI;
using TMPro;
using UnityEngine.TextCore.Text;

public class Dialogue : MonoBehaviour
{
    [Header("Targets")]
    [SerializeField]
    private Intro _intro;
    [SerializeField]
    private GameObject _windowScaleParent;
    [SerializeField]
    private TextMeshProUGUI _nameText;
    [SerializeField]
    private TextMeshProUGUI _speechText;
    [SerializeField]
    private TextAnimator_TMP _textAnimatorPlayer;
    [SerializeField]
    private TypewriterByWord _textTypewriter;
    [SerializeField]
    [ReadOnlyInspector]
    private TMP_FontAsset _originalFont;

    [Header("Settings")]
    [SerializeField]
    private float _windowScaleDuration = 0.3f;
    [SerializeField]
    private float _textShowedCloseDelay = 5f;
    [SerializeField]
    private float _confirmCloseDelay = 0.1f;

    [Header("Debug")]
    [SerializeField]
    [ReadOnlyInspector]
    private string _originalText;
    [SerializeField]
    [ReadOnlyInspector]
    private bool _busy;
    [Space]
    [Header("Debug")]
    [SerializeField]
    [ReadOnlyInspector]
    public DialogueParams _currentDialogueParams;
    [SerializeField]
    [ReadOnlyInspector]
    private bool _waitingForConfirmation;
    private Tween _windowScaleTween;
    private Tween _buttonsScaleTween;
    private readonly Queue<DialogueParams> _queue = new Queue<DialogueParams>();
    public GenericEvent _openEvent;
    void Start()
    {
        _originalFont = _speechText.font;
        _originalText = _textAnimatorPlayer.GetOriginalTextFromSource();
        _windowScaleParent.transform.localScale = Vector3.zero;
        _textAnimatorPlayer.ResetState();
        _busy = false;

        _textTypewriter.onTextShowed.AddListener(() =>
        {
            _busy = false;
        });
        
        _textTypewriter.onTypewriterStart.AddListener(() =>
        {
            _busy = true;
        });
    }

    void CloseImmediate()
    {
        _textAnimatorPlayer.SetTextToSource("");
        _textAnimatorPlayer.ResetState();
        _windowScaleParent.transform.localScale = Vector3.zero;
    }

    public void ShowWindow(DialogueParams dialogueParams, bool autoClose = false)
    {
        _busy = true;
        _currentDialogueParams = dialogueParams;
        CloseImmediate();

        if (_windowScaleTween != null)
        {
            _windowScaleTween.Kill();
            _windowScaleTween = null;
        }

        StopCoroutine(nameof(setNewText));

        _windowScaleTween = _windowScaleParent.transform.DOScale(Vector3.one, _windowScaleDuration)
            .OnComplete(() =>
            {
                StartCoroutine(setNewText(dialogueParams._text));
                
                
                if (autoClose)
                {
                    StartCoroutine(closeWindowDelayed());
                }
                
                
            });
        _openEvent.Invoke(new GenericEventOpts());
    }

    IEnumerator closeWindowDelayed()
    {
        yield return new WaitForSeconds(_textShowedCloseDelay);
        CloseWindow(_currentDialogueParams, true, false);
    }
    
    private IEnumerator setNewText(string text)
    {
        yield return null;
        // because the editor wont let you insert newlines into string arrays
        string[] lines = text.Split(new string[] { "\\n" }, StringSplitOptions.None);
        text = string.Join("\n", lines);
        _textAnimatorPlayer.SetTextToSource(text);
        _textAnimatorPlayer.ResetState();
        
    }

    public void CloseWindow(DialogueParams dialogueParams, bool tweenSetUpdate, bool scaleDownButtons)
    {
        _textAnimatorPlayer.SetTextToSource("");
        _textAnimatorPlayer.ResetState();

        if (_windowScaleTween != null)
        {
            _windowScaleTween.Kill();
            _windowScaleTween = null;
        }

        _windowScaleTween = _windowScaleParent.transform.DOScale(Vector3.zero, _windowScaleDuration)
            .SetUpdate(tweenSetUpdate)
            .OnComplete(() => { _busy = false; });
    }
    
    public void ConfirmNext(Dialogue nextDialogue)
    {
        DialogueParams opts = new DialogueParams();
        CloseWindow(opts, true, nextDialogue == this);
    }

    public bool Skip()
    {
        if (_busy) {
            _textTypewriter.SkipTypewriter();
            _busy = false;
            return true;
        }

        return false;
    }
}