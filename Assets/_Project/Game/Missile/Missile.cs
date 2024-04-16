using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class Missile : MonoBehaviour
{
    [SerializeField]
    private float _initialHorizontalForce = 3f;

    [SerializeField]
    private AnimationCurve _chargeForceCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField]
    private float _chargeForceMultiplier = 3f;

    [SerializeField]
    [ReadOnlyInspector]
    private float _charge;

    [SerializeField]
    [ReadOnlyInspector]
    private Vector3 _launchDirection;

    private Rigidbody _rb;

    public GenericEvent _dropEvent;

    public void Initialize(PlayerAim aimer, PlayerShoot shoot)
    {
        transform.position = aimer.LaunchPosition();
        _launchDirection = aimer.LaunchDirection();
        _charge = shoot.GetCharge();
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        Vector3 force = _launchDirection * _initialHorizontalForce;
        force *= _chargeForceCurve.Evaluate(_charge) * _chargeForceMultiplier;
        _rb.AddForce(force, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision other)
    {
        Spirit spirit = other.gameObject.GetComponent<Spirit>();
        if (spirit != null)
        {
            spirit.Kill(other.GetContact(0).point);
            gameObject.SetActive(false);
        }

        Ground ground = other.gameObject.GetComponent<Ground>();
        if (ground != null)
        {
            StartCoroutine(deactivate());
            _dropEvent.Invoke(new GenericEventOpts());
        }
    }

    IEnumerator deactivate()
    {
        yield return new WaitForSeconds(2);
        Tween tween = transform.DOScale(Vector3.zero, 0.5f).OnComplete(
            () => { gameObject.SetActive(false); });
    }
}