using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    private static Collector _instance;
    public static Collector Instance
    {
        get
        {
            // attempt to locate the singleton
            if (_instance == null)
            {
                _instance = (Collector)FindObjectOfType(typeof(Collector));
            }

            // create a new singleton
            if (_instance == null)
            {
                _instance = (new GameObject("Collector")).AddComponent<Collector>();
            }

            // return singleton
            return _instance;
        }
    }

    public Vector3 _overlapDimension = Vector3.one;
    public Vector3 _overlapOffset = Vector3.one;
    public LayerMask _collectibleLayerMask;
    public int _collected = 0;
    public GenericEvent _gameWinEvent;
    public GenericEvent _collectEvent;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + _overlapOffset, _overlapDimension);
    }

    private void FixedUpdate()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position + _overlapOffset, _overlapDimension,
            Quaternion.identity,
            _collectibleLayerMask);
        foreach (var collider in colliders)
        {
            Collectible collectible = collider.GetComponentInChildren<Collectible>();
            if (collectible != null)
            {
                collectible.Collect();
                _collected++;
                _collectEvent.Invoke(new GenericEventOpts());
                if (_collected == 3)
                {
                    GenericEventOpts opts = new GenericEventOpts();
                    _gameWinEvent.Invoke(opts);
                }
            }
        }
    }
}