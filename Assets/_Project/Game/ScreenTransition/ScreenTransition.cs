using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenTransition : MonoBehaviour
{
    private static ScreenTransition _instance;
    public static ScreenTransition Instance
    {
        get
        {
            // attempt to locate the singleton
            if (_instance == null)
            {
                _instance = (ScreenTransition)FindObjectOfType(typeof(ScreenTransition));
            }

            // create a new singleton
            if (_instance == null)
            {
                _instance = (new GameObject("Screen Transition")).AddComponent<ScreenTransition>();
            }

            // return singleton
            return _instance;
        }
    }

    [SerializeField]
    private Material _mat;
    [SerializeField]
    private float _transitionSpeed = 2f;
    [SerializeField]
    [ReadOnlyInspector]
    private float _transitionProgress = 0f;
    [SerializeField]
    [ReadOnlyInspector]
    private TransitionState _state = 0f;
    [SerializeField]
    private GenericEvent _onTransitionOpenDoneEvent;
    [SerializeField]
    private GenericEvent _onTransitionCloseDoneEvent;
    [SerializeField]
    private Image _image;
    [SerializeField]
    [ReadOnlyInspector]
    private bool _transitioning;

    public enum TransitionState
    {
        Close,
        Open
    }

    void Start()
    {
        _transitionProgress = 0f;
        _state = TransitionState.Open;
        _mat = new Material(_mat);
        _image.material = _mat;
        _transitioning = false;
    }

    public void HandleOnGameStateChange(GenericEventOpts opts)
    {
        switch (opts._newState)
        {
            case GStateInit _:
                Close();
                _transitionProgress = 1f;
                break;
            case GStateStartIn _:
                DelayedOpen(1f);
                break;
        }
        
    }

    void Update()
    {
        switch (_state)
        {
            case TransitionState.Close:
                _transitionProgress += Time.deltaTime * _transitionSpeed;
                _transitionProgress = Mathf.Clamp(_transitionProgress, 0f, 1f);

                if (_transitionProgress >= 1f)
                {
                    GenericEventOpts opts = new GenericEventOpts
                    {
                    };
                    if (_transitioning)
                    {
                        _transitioning = false;
                        _onTransitionCloseDoneEvent.Invoke(opts);
                    }
                }

                break;
            case TransitionState.Open:
                _transitionProgress -= Time.deltaTime * _transitionSpeed;
                _transitionProgress = Mathf.Clamp(_transitionProgress, 0f, 1f);

                if (_transitionProgress <= 0f)
                {
                    GenericEventOpts opts = new GenericEventOpts
                    {
                    };
                    if (_transitioning)
                    {
                        _transitioning = false;
                        _onTransitionOpenDoneEvent.Invoke(opts);
                    }
                }

                break;
        }

        _mat.SetFloat("_CircleTransitionProgress", _transitionProgress);
    }

    public void Close()
    {
        _state = TransitionState.Close;
        _transitioning = true;
    }

    public void DelayedOpen(float delay)
    {
        StartCoroutine(delayedOpen(delay));
    }

    IEnumerator delayedOpen(float delay)
    {
        yield return new WaitForSeconds(delay);
        _state = TransitionState.Open;
        _transitioning = true;
    }
}