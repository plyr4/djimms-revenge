using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spirit : MonoBehaviour
{
    private SpiritFactory _factory;
    [SerializeField]
    private float _growSpeed = 0.1f;
    [SerializeField]
    private float _maxGrowScale = 2f;
    [SerializeField]
    private Vector2 _maxGrowScaleRandomFactorRange = new Vector2(-0.2f, 0.5f);
    [SerializeField]
    private Vector2 _initialRotationForceRange = new Vector2(-0.08f, 0.05f);
    [SerializeField]
    private float _initialLiftForce = 1f;
    [SerializeField]
    private Vector2 _initialHorizontalForceRange = new Vector2(-0.4f, 0.05f);
    [SerializeField]
    private float _floatLiftForce = 0.2f;
    [SerializeField]
    private float _minimumLiftVelocity = 0.2f;
    private Rigidbody _rb;
    private float _maxGrowScaleRandomFactor;
    private float _desiredScale;
    private float _growInterval = 2f;
    [SerializeField]
    [ReadOnlyInspector]
    private int _growCrowd;
    [SerializeField]
    private int _growthCrowdedThreshold = 4;

    [SerializeField]
    private float _growScaleSpawnThreshold1 = 1.5f;
    [SerializeField]
    private float _growScaleSpawnThreshold2 = 3f;
    [SerializeField]
    private float _maxSmokePuffScale = 1f;

    [SerializeField]
    private Transform _scaler;
    [SerializeField]
    private ParticleSystem _smokePuff;
    [SerializeField]
    private GenericEvent _killedEvent;

    public void Initialize(SpiritFactory factory)
    {
        _factory = factory;
        _initialRotationForceRange = factory._initialRotationForceRange;
    }

    void Start()
    {
        _desiredScale = _scaler.localScale.x;

        _rb = GetComponent<Rigidbody>();
        _rb.AddForce(Vector3.up * _initialLiftForce, ForceMode.Impulse);
        float hForce = Random.Range(_initialHorizontalForceRange.x, _initialHorizontalForceRange.y);
        _rb.AddForce(Vector3.right * hForce, ForceMode.Impulse);
        _maxGrowScaleRandomFactor = Random.Range(_maxGrowScaleRandomFactorRange.x, _maxGrowScaleRandomFactorRange.y);
        _rb.AddTorque(
            -Vector3.forward * hForce * Random.Range(_initialRotationForceRange.x, _initialRotationForceRange.y),
            ForceMode.Impulse);
        _growInterval *= Random.Range(0.8f, 1.2f);
        _maxGrowScale *= Random.Range(0.8f, 1.2f);
        _growSpeed *= Random.Range(0.8f, 1.2f);

        InvokeRepeating(nameof(Grow), _growInterval, _growInterval);
    }
    
    private void Grow()
    {
        if (_scaler.localScale.x <= _maxGrowScale + _maxGrowScaleRandomFactor)
        {
            // overlap sphere at the increased size to check if there is room to grow, if not skip it
            _desiredScale = _scaler.localScale.x + _growSpeed;
            var colliders = Physics.OverlapSphere(transform.position, _desiredScale);
            _growCrowd = colliders.Length;
            if (_growCrowd >= _growthCrowdedThreshold)
            {
                _desiredScale = _scaler.localScale.x;
                return;
            }

            float toScale = Util.Round(_desiredScale, 10f);
            _scaler.localScale =
                Vector3.one * toScale;
            _scaler.localScale = new Vector3(_scaler.localScale.x, _scaler.localScale.y, 1f);
        }
    }

    void FixedUpdate()
    {
        if (_rb.velocity.y < _minimumLiftVelocity)
        {
            float f = Random.Range(0, 1) == 1 ? 1 : -1;
            _rb.AddForce(Vector3.up * (f * _floatLiftForce), ForceMode.Force);
        }
    }

    public void Kill(Vector3 hitPosition, bool spawnSpirit = true)
    {
        gameObject.SetActive(false);
        var ls = _smokePuff.transform.lossyScale;
        _smokePuff.transform.parent = null;
        _smokePuff.transform.position = hitPosition;
        if (ls.x >= _maxSmokePuffScale)
        {
            _smokePuff.transform.localScale = Vector3.one * _maxSmokePuffScale;
        }

        _smokePuff.Play();

        // if it grew too big, spawn a new spirit
        if (_scaler.localScale.x > _growScaleSpawnThreshold1)
        {
            CoroutineRunner.instance.StartCoroutine(SpawnSpirit(hitPosition, 0.1f));
        }

        if (spawnSpirit && _scaler.localScale.x > _growScaleSpawnThreshold2)
        {
            float rx = Random.Range(0.5f, 1f) * Util.RandomSign();
            float ry = Random.Range(0.5f, 1f) * Util.RandomSign();
            Vector3 r = new Vector3(rx, ry, 0f);
            CoroutineRunner.instance.StartCoroutine(SpawnSpirit(hitPosition + r, 0.25f));
        }
        GenericEventOpts opts = new GenericEventOpts();
        _killedEvent.Invoke(opts);
    }

    IEnumerator SpawnSpirit(Vector3 hitPosition, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        _factory.SpawnSpirit(hitPosition);
    }
}