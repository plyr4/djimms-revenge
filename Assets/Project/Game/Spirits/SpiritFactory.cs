using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritFactory : MonoBehaviour
{
    [SerializeField]
    private GameObject _spiritPrefab;
    public Vector2 _initialRotationForceRange = new Vector2(-0.08f, 0.05f);
    void Start()
    {
        InvokeRepeating(nameof(SpawnSpirit), 1f, 1f);
    }

    void SpawnSpirit()
    {
        GameObject go = Instantiate(_spiritPrefab, transform.position, Quaternion.identity, transform);
        Spirit spirit = go.GetComponent<Spirit>();
        spirit.Initialize(this);
    }
}
