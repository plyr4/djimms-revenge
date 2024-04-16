using UnityEngine;
using System.Collections.Generic;

[ExecuteAlways]
public class LightFlicker : MonoBehaviour
{
    [Header("Light")]
    [SerializeField]
    private Light _light;
    [Space]

    [Header("Update Settings")]
    [SerializeField]
    private bool _run = true;
    [SerializeField]
    private bool _runInEditMode = false;
    [Space]

    [Header("Intensity Settings")]
    [SerializeField]
    private float _minIntensity = 0f;
    [SerializeField]
    private float _maxIntensity = 1f;
    [Space]

    [Header("Smoothing Settings")]
    [Range(1, 50)]
    [SerializeField]
    private int _smoothing = 20; // how much to smooth out the randomness; lower values = sparks, higher = lantern
    [SerializeField]
    private bool _shouldSmoothIntensityChange = true;
    [SerializeField]
    [Range(0f, 5f)]
    private float _smoothSpeed = 2f;
    [Space]

    [Header("Debug")]
    // continuous average calculation via FIFO queue to save on iteration calculation costs
    private Queue<float> _smoothQueue;
    [SerializeField]
    [ReadOnlyInspector]
    private float _goalIntensity = 0f;
    [SerializeField]
    [ReadOnlyInspector]
    private float _sumOfIntensityValues = 0f;

    public void Reset()
    {
        if (_light == null)
        {
            _light = GetComponent<Light>();
        }

        _smoothQueue.Clear();
        _sumOfIntensityValues = 0f;
    }

    private void Start()
    {
        _smoothQueue = new Queue<float>(_smoothing);

        if (_light == null)
        {
            _light = GetComponent<Light>();
        }

        if (_light == null)
        {
            Debug.LogError("LightFlicker: light is null, did you mean to assign it in the inspector? this will cause issues");
        }

        _smoothQueue.Clear();
        _goalIntensity = _minIntensity;
        _sumOfIntensityValues = 0f;
        _light.intensity = _goalIntensity;
    }

    private void Update()
    {
        if (_light == null) return;
        if (!_run) return;
        if (!Application.isPlaying && !_runInEditMode) return;
        if (_smoothQueue == null)
        {
            _smoothQueue = new Queue<float>(_smoothing);
        }

        // pop off an item if too big
        while (_smoothQueue.Count >= _smoothing)
        {
            _sumOfIntensityValues -= _smoothQueue.Dequeue();
        }

        // use the queue to store a new goal intensity then smooth using the average
        float newIntensity = Random.Range(_minIntensity, _maxIntensity);
        _smoothQueue.Enqueue(newIntensity);
        _sumOfIntensityValues += newIntensity;

        // calculate and apply new smoothed average
        _goalIntensity = _sumOfIntensityValues / (float)_smoothQueue.Count;

        if (_shouldSmoothIntensityChange)
        {
            _light.intensity = Mathf.Lerp(_light.intensity, _goalIntensity, Time.deltaTime * _smoothSpeed * 10f);
        }
        else
        {
            _light.intensity = _goalIntensity;
        }
    }
}