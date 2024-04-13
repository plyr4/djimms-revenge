using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
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

    [SerializeField]
    private Transform _chargeTestViz;
    
    private void Update()
    {
        _chargeTestViz.localScale = new Vector3(1f, _charge, 1f);
    }

    public void Charge()
    {
        if (!_missileReady ) return;
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

    public void Shoot(Aimer aimer)
    {
        if (!_missileReady) return;

        _missileReady = false;

        GameObject go = Instantiate(_missilePrefab, transform.position, Quaternion.identity, _missilesParent);
        Missile missile = go.GetComponent<Missile>();
        missile.Initialize(aimer, this);

        StartCoroutine(MissileCooldown());

        Discharge();
    }

    private IEnumerator MissileCooldown()
    {
        yield return new WaitForSeconds(_missileCooldown);
        _missileReady = true;
    }
}