using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class RenderTextureCamera : MonoBehaviour
{
    [SerializeField]
    public Camera cam;
    [SerializeField]
    public RenderTexture _renderTexture;
    [SerializeField]
    public UnityEngine.UI.RawImage _outImage;
    [SerializeField]
    public int TextureDepth;
    [SerializeField]
    [Range(0.01f, 1f)]
    public float ScaleFactor = 0.5f;

    private void Awake()
    {
        if (cam == null) cam = GetComponent<Camera>();
        if (_renderTexture == null) UpdateRenderTexture();
        if (_renderTexture.width != Width() || _renderTexture.height != Height()) UpdateRenderTexture();
    }

    private void Update() {
        if (_renderTexture == null) UpdateRenderTexture();
        if (_renderTexture.width != Width() || _renderTexture.height != Height()) UpdateRenderTexture();
    }

    private int Width() {
        return Mathf.FloorToInt(Screen.width * ScaleFactor);
    }

    private int Height() {
        return Mathf.FloorToInt(Screen.height * ScaleFactor);
    }

    public void UpdateRenderTexture()
    {
        if (cam.targetTexture != null)
        {
            cam.targetTexture.Release();
        }
        _renderTexture = new RenderTexture(Width(), Height(), TextureDepth){
            filterMode = FilterMode.Point,
            antiAliasing = 1,
        };
        cam.targetTexture = _renderTexture;
        _outImage.texture = _renderTexture;
    }
}
