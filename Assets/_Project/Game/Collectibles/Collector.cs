using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    public Vector3 _overlapDimension = Vector3.one;
    public Vector3 _overlapOffset = Vector3.one;
    public LayerMask _collectibleLayerMask;
    public int _collected = 0;
    public GenericEvent _gameWinEvent;
    public GenericEvent _collectEvent;
    public bool _gameWon = false;

    private void Start()
    {
        _collected = 0;
        _gameWon = false;    
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + _overlapOffset, _overlapDimension);
    }

    private void FixedUpdate()
    {
        if (_gameWon)
        {
            return;
        }
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
                _collectEvent.Invoke(new GenericEventOpts(
                ) { _collector = this });
                if (_collected >= 3)
                {
                    _gameWon = true;
                    GenericEventOpts opts = new GenericEventOpts();
                    _gameWinEvent.Invoke(opts);
                }
            }
        }
    }
}