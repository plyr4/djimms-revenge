using UnityEngine;

public class Spirit : MonoBehaviour
{
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

    public void Initialize(SpiritFactory factory)
    {
        _initialRotationForceRange = factory._initialRotationForceRange;
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.AddForce(Vector3.up * _initialLiftForce, ForceMode.Impulse);
        float hForce = Random.Range(_initialHorizontalForceRange.x, _initialHorizontalForceRange.y);
        _rb.AddForce(Vector3.right * hForce, ForceMode.Impulse);
        _maxGrowScaleRandomFactor = Random.Range(_maxGrowScaleRandomFactorRange.x, _maxGrowScaleRandomFactorRange.y);
        _rb.AddTorque(-Vector3.forward * hForce * Random.Range(_initialRotationForceRange.x, _initialRotationForceRange.y),
            ForceMode.Impulse);
    }

    void FixedUpdate()
    {
        if ( transform.localScale.x <= _maxGrowScale + _maxGrowScaleRandomFactor)
        {
            transform.localScale += Vector3.one * (Time.deltaTime * _growSpeed);
        }

        if (_rb.velocity.y < _minimumLiftVelocity)
        {
            _rb.AddForce(Vector3.up * _floatLiftForce, ForceMode.Force);
        }
    }

    public void Kill()
    {
        gameObject.SetActive(false);
    }
}