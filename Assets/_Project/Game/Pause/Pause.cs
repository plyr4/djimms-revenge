using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Pause : MonoBehaviour
{
    [SerializeField]
    private GenericEvent _continueEvent;
    [SerializeField]
    private GenericEvent _quitEvent;
    [SerializeField]
    private GameObject _viewParent;
    
    [SerializeField]
    private GameObject _scaler;
    private Sequence _menuTween;
    [SerializeField]
    private float _fadeInDuration = 0.5f;
    [SerializeField]
    private Ease _fadeInEase = Ease.OutBack;

    
    private void Start()
    {
        _viewParent.SetActive(false);
    }
    
    public void HandleContinue()
    {
        GenericEventOpts opts = new GenericEventOpts
        {
        };
        _continueEvent.Invoke(opts);
    }

    public void HandleQuit()
    {
        GenericEventOpts opts = new GenericEventOpts
        {
        };
        _quitEvent.Invoke(opts);
    }
    
    public void HandleOnGameStateChange(GenericEventOpts opts)
    {
        switch (opts._newState)
        {
            case GStatePause _:
                gameObject.SetActive(true);
                _viewParent.SetActive(true);
                _scaler.transform.localScale = Vector3.zero;
                if (_menuTween != null)
                {
                    _menuTween.Kill();
                    _menuTween = null;
                }

                _menuTween = DOTween.Sequence();
                _menuTween.SetUpdate(true);
                _menuTween.Insert(0f, _scaler.transform.DOScale(Vector3.one, _fadeInDuration / 2f)
                    .SetUpdate(true)
                    .SetDelay(0f)
                    .SetEase(_fadeInEase));
                break;
            case GStatePlay _:
                if (_menuTween != null)
                {
                    _menuTween.Kill();
                    _menuTween = null;
                }
            
                _menuTween = DOTween.Sequence();
                _menuTween.SetUpdate(true);
                _menuTween.Insert(0f, _scaler.transform.DOScale(Vector3.zero, _fadeInDuration / 4f)
                        .SetUpdate(true)
                        .SetDelay(0.1f)
                        .SetEase(_fadeInEase))
                    .OnComplete(() =>
                    {
                        // GenericEventOpts opts = new GenericEventOpts
                        // {
                        // };
                        // _onStartOutDoneEvent.Invoke(opts);
                        // _inGameStuffViewParent.SetActive(false);
                    });
                break;
            default:
                break;
        }
    }
}