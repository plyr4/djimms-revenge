using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class RenderTextureInitializer : MonoBehaviour
{
    [Header("Base Render Texture Template")]
    [SerializeField]
    private RenderTexture _renderTextureTemplate;
    [Space]
    [Header("Targets")]
    [SerializeField]
    private Camera _targetCamera;
    [SerializeField]
    private MeshRenderer _targetMeshRenderer;
    [Space]
    [Header("Overrides")]
    [SerializeField]
    private TextureWrapMode _wrapMode = TextureWrapMode.Clamp;
    [SerializeField]
    private FilterMode _filterMode = FilterMode.Bilinear;
    [Header("WebGL")]
    [SerializeField]
    private GraphicsFormat _webglGraphicsFormatOverride = GraphicsFormat.R32G32B32A32_SFloat;
    [Space]
    [Header("Settings")]
    [SerializeField]
    private bool _createRenderTextureOnStart = true;
    [SerializeField]
    private bool _assignRenderTextureToCameraOnStart = true;
    [SerializeField]
    private bool _assignRenderTextureToMaterialOnStart = true;
    [Space]
    [Header("Debug")]
    [SerializeField]
    [ReadOnlyInspector]
    private RenderTexture _createdRenderTexture;

    private void Start()
    {
        _ = _webglGraphicsFormatOverride;
        if (_createRenderTextureOnStart && _renderTextureTemplate != null)
        {
            GraphicsFormat graphicsFormat = _renderTextureTemplate.graphicsFormat;
#if UNITY_WEBGL && !UNITY_EDITOR
            graphicsFormat = _webglGraphicsFormatOverride;
#endif
            _createdRenderTexture = CreateRenderTexture(_renderTextureTemplate, graphicsFormat,
                _renderTextureTemplate.depthStencilFormat, _wrapMode, _filterMode);

            if (_assignRenderTextureToCameraOnStart && _targetCamera != null)
            {
                _targetCamera.targetTexture = _createdRenderTexture;
            }

            if (_assignRenderTextureToMaterialOnStart && _targetMeshRenderer != null)
            {
                _targetMeshRenderer.material.mainTexture = _createdRenderTexture;
            }
        }
    }

    private void Reset()
    {
        if (_targetCamera == null) _targetCamera = GetComponent<Camera>();
    }

    private RenderTexture CreateRenderTexture(RenderTexture template, GraphicsFormat colorFormat,
        GraphicsFormat depthStencilFormat, TextureWrapMode wrapMode, FilterMode filterMode, int width = 0,
        int height = 0)
    {
        RenderTexture createdRenderTexture = _createdRenderTexture;
        if (template != null)
        {
            if (width == 0)
            {
                width = template.width;
            }

            if (height == 0)
            {
                height = template.height;
            }

            Debug.Log(
                $"creating render texture from template ({template.name}) using size({width}, {height}) color_format({colorFormat}) depth_stencil_format({depthStencilFormat}) wrap_mode({wrapMode}) filter_mode({filterMode})");

            createdRenderTexture =
                new RenderTexture(width, height, colorFormat, depthStencilFormat, template.mipmapCount);
            createdRenderTexture.wrapMode = wrapMode;
            createdRenderTexture.filterMode = filterMode;
        }
        else
        {
            Debug.LogError("unable to create render texture, no template provided");
        }

        return createdRenderTexture;
    }
}