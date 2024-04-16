using UnityEngine;


[RequireComponent(typeof(RectTransform))]
[ExecuteInEditMode]

public class AspectRatioScalerGameTexture : MonoBehaviour
{
    [Header("Reference Resolution")]
    private Vector2 _referenceResolution = new Vector2(2560f, 1440f);
    [Space]

    [Header("Scale Target")]
    [SerializeField]
    private RectTransform[] _scaleTargets;
    [Space]

    [Header("Update Settings")]
    [SerializeField]
    public bool _updateResolutionOnStart = true;
    [SerializeField]
    public bool _updateResolutionOnRectTransformDimensionsChange = true;
    [Space]

    [Header("Apply Settings")]
    [SerializeField]
    public bool _applyResolutionOnRectTransformDimensionsChange = true;
    [SerializeField]
    public bool _applyResolutionOnStart = true;
    [SerializeField]
    public AspectRatioScalerMode _resolutionScaleMode = AspectRatioScalerMode.Height;
    [Space]

    [Header("Debug")]
    [SerializeField]
    [ReadOnlyInspector]
    private Vector2 _currentScreenSize = new Vector2(2560f, 1440f);
    [SerializeField]
    [ReadOnlyInspector]
    private Vector2 _currentActualScreenSize = new Vector2(2560f, 1440f);
    [SerializeField]
    [ReadOnlyInspector]
    private RectTransform _rectTransform;

    private void Start()
    {
        if (_updateResolutionOnStart) UpdateResolution();
        if (_applyResolutionOnStart) ApplyResolution();
    }

    private void OnRectTransformDimensionsChange()
    {
        if (_updateResolutionOnRectTransformDimensionsChange) UpdateResolution();
        if (_applyResolutionOnRectTransformDimensionsChange) ApplyResolution();
    }

    public void UpdateResolution()
    {
        if (_rectTransform == null) _rectTransform = GetComponent<RectTransform>();
        Rect rect = _rectTransform.rect;
        _currentScreenSize = new Vector2(rect.width, rect.height);
        _currentActualScreenSize = new Vector2(Screen.width, Screen.height);
    }

    public void ApplyResolution()
    {
        Vector2 size = _currentScreenSize;
        if (_resolutionScaleMode == AspectRatioScalerMode.Height)
        {
            float r = _currentScreenSize.y / _referenceResolution.y;
            size = new Vector2(_referenceResolution.x * r, _currentScreenSize.y);
        }
        else
        {
            float r = _currentScreenSize.x / _referenceResolution.x;
            size = new Vector2(_currentScreenSize.x, _referenceResolution.y * r);
        }

        for (int i = 0; i < _scaleTargets.Length; i++)
        {
            _scaleTargets[i].localScale = new Vector3(size.x, size.y, 1f);
        }
    }

    public void SetScaleMode(AspectRatioScalerMode mode)
    {
        _resolutionScaleMode = mode;
        UpdateResolution();
        ApplyResolution();
    }
}
