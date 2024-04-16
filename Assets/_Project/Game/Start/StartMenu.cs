using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    [SerializeField]
    private GenericEvent _onStartPlayEvent;
    [SerializeField]
    private GenericEvent _onStartOutDoneEvent;
    [SerializeField]
    private GenericEvent _onGameOverQuitEvent;
    [SerializeField]
    private GameObject _viewParent;
    [SerializeField]
    private GameObject _scaler;
    private Sequence _menuTween;
    [SerializeField]
    private float _fadeInDuration = 0.5f;
    [SerializeField]
    private float _fadeInDelay = 0.5f;
    [SerializeField]
    private Ease _fadeInEase = Ease.OutBack;

    [SerializeField]
    private GameObject _inGameStuffViewParent;

    private void Start()
    {
        _viewParent.SetActive(false);
    }

    public void HandlePlay()
    {
        GenericEventOpts opts = new GenericEventOpts
        {
        };
        _onStartPlayEvent.Invoke(opts);
    }

    public void HandleQuit()
    {
        GenericEventOpts opts = new GenericEventOpts
        {
        };
        _onGameOverQuitEvent.Invoke(opts);
    }

    public void HandleOnGameStateChange(GenericEventOpts opts)
    {
        switch (opts._newState)
        {
            case GStateStartIn _:
                gameObject.SetActive(true);
                _viewParent.SetActive(true);
                _inGameStuffViewParent.SetActive(true);
                _scaler.transform.localScale = Vector3.zero;
                if (_menuTween != null)
                {
                    _menuTween.Kill();
                    _menuTween = null;
                }

                _menuTween = DOTween.Sequence();
                _menuTween.Insert(0f, _scaler.transform.DOScale(Vector3.one, _fadeInDuration)
                    .SetDelay(_fadeInDelay)
                    .SetEase(_fadeInEase));

                break;
            case GStateStart _:
                gameObject.SetActive(true);
                _viewParent.SetActive(true);
                _inGameStuffViewParent.SetActive(true);
                _scaler.transform.localScale = Vector3.one;
                break;
            case GStateLoadIntro _:
            case GStateLoadPlay _:
                _viewParent.SetActive(false);
                _inGameStuffViewParent.SetActive(false);
                _scaler.transform.localScale = Vector3.zero;
                break;
            default:
                break;
        }
    }
}