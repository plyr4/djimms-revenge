using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField]
    private PlayerAim _aimer;
    [SerializeField]
    private GameObject _missilePrefab;
    [SerializeField]
    private Transform _missilesParent;
    [SerializeField]
    private float _missileCooldown = 1f;
    private bool _missileReady = true;

    [SerializeField]
    private float _chargeRate = 1f;

    [SerializeField]
    [ReadOnlyInspector]
    private float _charge;

    [FormerlySerializedAs("_chargeTestViz")]
    [SerializeField]
    private Transform _chargeFillScaler;
    [SerializeField]
    private Transform _chargeViewParent;
    [SerializeField]
    private Transform _rotationParent;

    private void Start()
    {
        if (_aimer == null) _aimer = FindObjectOfType<PlayerAim>();
    }

    private void Update()
    {
        _chargeFillScaler.localScale = new Vector3(1f, _charge, 1f);
        
        switch (GStateMachineGame.GetCurrentState())
        {
            case GStatePlay _:
            case GStateLoadPlay _:
                _chargeViewParent.gameObject.SetActive(true);
                break;
            case GStateGameOver _:
                _chargeViewParent.gameObject.SetActive(false);
                break;
            default:
                return;
        }
    }

    private void FixedUpdate()
    {
        switch (GStateMachineGame.GetCurrentState())
        {
            case GStatePlay _:
                break;
            default:
                return;
        }

        if (Project.Input.GameInput.Instance._fire)
        {
            Charge();
        }
        else
        {
            if (Charged())
            {
                Shoot(_aimer);
            }
        }
    }

    public void Charge()
    {
        if (!_missileReady) return;
        _charge += _chargeRate * Time.deltaTime;
        _charge = Mathf.Clamp(_charge, 0, 1);
    }

    private void Discharge()
    {
        _charge = 0;
        _charge = Mathf.Clamp(_charge, 0, 1);
    }

    public bool Charged()
    {
        return _charge > 0f;
    }

    public float GetCharge()
    {
        return _charge;
    }

    public void Shoot(PlayerAim aimer)
    {
        if (!_missileReady) return;

        _missileReady = false;

        GameObject go = Instantiate(_missilePrefab, transform.position, Quaternion.identity, _missilesParent);
        Missile missile = go.GetComponent<Missile>();
        missile.Initialize(aimer, this);

        StartCoroutine(MissileCooldown());

        Discharge();

        if (transform.position.x - aimer.LaunchPosition().x < 0f)
        {
            _rotationParent.rotation = Quaternion.Euler(0f, 90f, 0f);
        }

        if (transform.position.x - aimer.LaunchPosition().x > 0f)
        {
            _rotationParent.rotation = Quaternion.Euler(0f, -90f, 0f);
        }
    }

    private IEnumerator MissileCooldown()
    {
        yield return new WaitForSeconds(_missileCooldown);
        _missileReady = true;
    }
}