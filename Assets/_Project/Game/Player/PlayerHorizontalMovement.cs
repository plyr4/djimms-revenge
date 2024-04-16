using System;
using System.Collections;
using Project.Input;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerHorizontalMovement : MonoBehaviour
{
    [Space]
    [Header("Physics")]
    [SerializeField]
    private Rigidbody _rb;
    [Space]
    [Header("Input")]
    [SerializeField]
    private Vector3 _horizontalMovement;
    [SerializeField]
    private bool _boost;
    [SerializeField]
    private bool _boostPressed;
    [Space]
    [Header("Beamable Effects")]
    [SerializeField]
    [Range(1f, 50f)]
    public float _horizontalMaxSpeedModifierFire = 1f;
    [Space]
    [Header("Movement Settings")]
    [SerializeField]
    [Range(1f, 50f)]
    private float _horizontalMaxSpeed = 25f;
    [SerializeField]
    [Range(1f, 200f)]
    private float _horizontalAcceleration = 100f;
    [SerializeField]
    [Range(1f, 200f)]
    private float _horizontalMaxAccelerationForce = 100f;
    [SerializeField]
    private Vector3 _horizontalAccelerationForceScale = new Vector3(1f, 0f, 1f);
    [SerializeField]
    private AnimationCurve _accelerationFactorFromDot;
    [SerializeField]
    private AnimationCurve _maxAccelerationForceFactorFromDot;
    [SerializeField]
    private Vector3 _forwardAngleBias = new Vector3(0f, 0f, 0f);
    private Vector3 _horizontalUnitGoal;
    private Vector3 _horizontalGoalVel;
    private Vector3 _verticalUnitGoal;
    private Vector3 _verticalGoalVel;
    public float _currentHorizontalVelocityRatio;
    public float _currentVerticalVelocityRatio;
    [Space]

    // animation IDs
    [SerializeField]
    private int _animIDSpeed;
    [SerializeField]
    private int _animIDGrounded;
    [SerializeField]
    private int _animIDJump;
    [SerializeField]
    private int _animIDFreeFall;
    [SerializeField]
    private int _animIDMotionSpeed;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private bool _hasAnimator;
    [SerializeField]
    private float _animationBlend;
    [SerializeField]
    private float _animationBlendFactor = 3f;
    [Header("Grounded")]
    public bool _grounded = true;
    public float _groundedOffset = -0.14f;
    public float _groundedRadius = 0.28f;
    public LayerMask _groundLayers;

    [Space(10)]
    [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
    public float JumpTimeout = 0.50f;
    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    public float FallTimeout = 0.15f;

    // timeout deltatime
    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;
    public float _jumpHeight = 1.2f;
    public float _gravity = -15.0f;
    private float _terminalVelocity = 53.0f;

    [SerializeField]
    private Transform _rotationParent;

    [SerializeField]
    private GameObject _gameOverAura;
    [SerializeField]
    private bool _gameOver;

    private void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDGrounded = Animator.StringToHash("Grounded");
        _animIDJump = Animator.StringToHash("Jump");
        _animIDFreeFall = Animator.StringToHash("FreeFall");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    }


    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _hasAnimator = TryGetComponent(out _animator);

        AssignAnimationIDs();

        // reset our timeouts on start
        _jumpTimeoutDelta = JumpTimeout;
        _fallTimeoutDelta = FallTimeout;

        _gameOver = false;
    }

    private void Update()
    {
        _horizontalMovement = Vector3.zero;

        switch (GStateMachineGame.GetCurrentState())
        {
            case GStatePlay _:
                _horizontalMovement = GameInput.Instance._horizontalMovement;
                break;
            case GStateGameOver _:
                // activate "genie aura" bubble effect and float the player around in a bubble
                _gameOverAura.SetActive(true);
                _rotationParent.rotation = Quaternion.Euler(0f, 180f, 0f);
                _rb.useGravity = false;
                if (!_gameOver)
                {
                    _gameOver = true;
                    _rb.drag = 0f;
                    _rb.velocity = Vector3.zero;
                    StartCoroutine(addDeathForceNextFrame());
                }

                break;
            default:
                return;
        }
    }


    private void FixedUpdate()
    {
        if (!_gameOver) JumpAndGravity();
        if (!_gameOver) GroundedCheck();
        MoveHorizontalWithForce();
        _currentHorizontalVelocityRatio = new Vector3(_rb.velocity.x, 0f, 0f).magnitude / _horizontalMaxSpeed;
        _currentVerticalVelocityRatio = new Vector3(0f, _rb.velocity.y, 0f).magnitude / _horizontalMaxSpeed;
        // how do i get only the horizontal velocity magnitude ratio
        // _currentHorizontalVelocityRatio
    }


    private void MoveHorizontalWithForce()
    {
        // capture relevant input axis
        Vector3 movement = new Vector3(_horizontalMovement.x, 0f, 0f);

        float inputMagnitude = GameInput.Instance._controlScheme == "KeyboardMouse" ? 1f : movement.magnitude;

        // normalize if necessary
        if (movement.magnitude > 0)
        {
            movement.Normalize();
        }

        Vector3 direction = transform.TransformDirection(movement);

        _horizontalUnitGoal = direction * inputMagnitude;
        _horizontalUnitGoal = Quaternion.Euler(_forwardAngleBias) * direction * inputMagnitude;

        Vector3 unitVel = _horizontalGoalVel.normalized;

        // groundVel is just going to be zero for now
        Vector3 groundVel = Vector3.zero;

        float velDot = Vector3.Dot(_horizontalUnitGoal, unitVel);

        float accel = _horizontalAcceleration * _accelerationFactorFromDot.Evaluate(velDot) *
                      _horizontalMaxSpeedModifierFire;

        float maxSpeed = _horizontalMaxSpeed;

        Vector3 goalVel = _horizontalUnitGoal * maxSpeed * _horizontalMaxSpeedModifierFire;

        _horizontalGoalVel = Vector3.MoveTowards(_horizontalGoalVel, goalVel + groundVel, accel * Time.deltaTime);

        Vector3 neededAccel = (_horizontalGoalVel - _rb.velocity) / Time.deltaTime;

        float maxAccel = _horizontalMaxAccelerationForce * _maxAccelerationForceFactorFromDot.Evaluate(velDot);

        neededAccel = Vector3.ClampMagnitude(neededAccel, maxAccel);

        _rb.AddForce(Vector3.Scale(neededAccel * _rb.mass, _horizontalAccelerationForceScale));

        _animationBlend = _currentHorizontalVelocityRatio * _animationBlendFactor;
        // _animationBlend = Mathf.Lerp(_animationBlend, _currentVelocityRatio, Time.deltaTime * 0.5f);
        // if (_animationBlend < 0.01f) _animationBlend = 0f;

        // update animator if using character
        if (_hasAnimator)
        {
            _animator.SetFloat(_animIDSpeed, _animationBlend);
            _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
        }

        // point in the direction of the input
        if (movement.x > 0f)
        {
            _rotationParent.rotation = Quaternion.Euler(0f, 90f, 0f);
        }

        if (movement.x < 0f)
        {
            _rotationParent.rotation = Quaternion.Euler(0f, -90f, 0f);
        }
    }

    private void JumpAndGravity()
    {
        float verticalVelocity = _rb.velocity.y;

        if (_grounded)
        {
            // reset the fall timeout timer
            _fallTimeoutDelta = FallTimeout;

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDJump, false);
                _animator.SetBool(_animIDFreeFall, false);
            }

            // stop our velocity dropping infinitely when grounded
            if (verticalVelocity < 0.0f)
            {
                verticalVelocity = -0.5f;
            }

            // Jump
            if (GameInput.Instance._jump && _jumpTimeoutDelta <= 0.0f)
            {
                // the square root of H * -2 * G = how much velocity needed to reach desired height
                verticalVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravity);

                // update animator if using character
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, true);
                }
            }

            // jump timeout
            if (_jumpTimeoutDelta >= 0.0f)
            {
                _jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            // reset the jump timeout timer
            _jumpTimeoutDelta = JumpTimeout;

            // fall timeout
            if (_fallTimeoutDelta >= 0.0f)
            {
                _fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                // update animator if using character
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDFreeFall, true);
                }
            }

            // if we are not grounded, do not jump
            GameInput.Instance._jump = false;
        }

        // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
        if (verticalVelocity < _terminalVelocity)
        {
            verticalVelocity += _gravity * Time.deltaTime;
        }

        _rb.velocity = new Vector3(_rb.velocity.x, verticalVelocity, _rb.velocity.z);
    }

    private void GroundedCheck()
    {
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - _groundedOffset,
            transform.position.z);
        _grounded = Physics.CheckSphere(spherePosition, _groundedRadius, _groundLayers,
            QueryTriggerInteraction.Ignore);

        // update animator if using character
        if (_hasAnimator)
        {
            _animator.SetBool(_animIDGrounded, _grounded);
        }
    }

    private void OnFootstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            // if (FootstepAudioClips.Length > 0)
            // {
            //     var index = Random.Range(0, FootstepAudioClips.Length);
            //     AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
            // }
        }
    }

    private void OnLand(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            // AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
        }
    }

    IEnumerator addDeathForceNextFrame()
    {
        yield return null;
        _rb.AddForce(new Vector3(0f, 5f, 0f), ForceMode.Impulse);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(
            new Vector3(transform.position.x, transform.position.y - _groundedOffset, transform.position.z),
            _groundedRadius);
    }
}