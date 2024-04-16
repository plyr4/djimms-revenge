using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public Color _winTitleColor = Color.yellow;
    public TextMeshProUGUI _title;
    public TextMeshProUGUI _subtitle;
    public TextMeshProUGUI _info;
    public bool _win;
    [SerializeField]
    private GenericEvent _onGameOverRetryEvent;
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


    private void Start()
    {
        _viewParent.SetActive(false);
    }

    public void HandleRetry()
    {
        GenericEventOpts opts = new GenericEventOpts
        {
        };
        _onGameOverRetryEvent.Invoke(opts);
    }

    public void HandleQuit()
    {
        GenericEventOpts opts = new GenericEventOpts
        {
        };
        _onGameOverQuitEvent.Invoke(opts);
    }

    public void HandleGameOverWinEvent(GenericEventOpts opts)
    {
        _win = true;
    }


    public void HandleOnGameStateChange(GenericEventOpts opts)
    {
        switch (opts._newState)
        {
            case GStateGameOver _:

                if (_win)
                {
                    _title.text = "YOU WIN";
                    _title.color = _winTitleColor;
                    _subtitle.text = "YOUR WISHES COME TRUE";
                    _info.gameObject.transform.parent.gameObject.SetActive(true);
                    _info.text = "DONT WORRY, DJIMM CAN REST NOW";
                }

                gameObject.SetActive(true);
                _viewParent.SetActive(true);
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
            // case GStateGameOverRetry _:
            //     if (_menuTween != null)
            //     {
            //         _menuTween.Kill();
            //         _menuTween = null;
            //     }
            //
            //     _menuTween = DOTween.Sequence();
            //     _menuTween.Insert(0f, _scaler.transform.DOScale(Vector3.zero, _fadeInDuration)
            //             .SetDelay(0.1f)
            //             .SetEase(_fadeInEase))
            //         .OnComplete(() =>
            //         {
            //             // GenericEventOpts opts = new GenericEventOpts
            //             // {
            //             // };
            //             // _onStartOutDoneEvent.Invoke(opts);
            //             // _inGameStuffViewParent.SetActive(false);
            //         });
            //     break;
            default:
                break;
        }
    }
}