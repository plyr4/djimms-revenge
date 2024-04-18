using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblesFactory : MonoBehaviour
{
    public float _overlapCheckSize = 1f;
    public float _minimumSpawnInterval = 1f;
    public float _normalSpawnInterval = 20f;
    public bool _spawnReady = false;
    public float _initialSpawnDelay = 10f;
    public LayerMask _playerLayerMask;
    public Transform[] _spawnPoints;
    public GameObject _collectiblePrefab;
    public Transform _spawnParent;

    void Start()
    {
        StopAllCoroutines();
        CancelInvoke(nameof(spawnReady));
        Invoke(nameof(spawnReady), _initialSpawnDelay);
    }

    public void HandleOnGameStateChange(GenericEventOpts opts)
    {
        switch (opts._newState)
        {
            case GStateNull _:
                break;
            case GStateRetry _:
            case GStateLoadPlay _:
                break;
            default:
                break;
        }
    }

    private void FixedUpdate()
    {
        if (_spawnReady)
        {
            // choose random spawn point position
            int index = Random.Range(0, _spawnPoints.Length);
            Transform spawnPoint = _spawnPoints[index];
            Collider[] colliders = Physics.OverlapSphere(spawnPoint.position, _overlapCheckSize, _playerLayerMask);
            if (colliders.Length > 0)
            {
                _spawnReady = false;
                Invoke(nameof(spawnReady), _minimumSpawnInterval);
                return;
            }
            _spawnReady = false;

            if (spawnPoint == null)
            {
                CancelInvoke(nameof(spawnReady));
                Invoke(nameof(spawnReady), _minimumSpawnInterval);
                return;
            }
            SpawnOne(spawnPoint.position);
            // remove this index from the spawn points
            List<Transform> list = new List<Transform>(_spawnPoints);
            list.RemoveAt(index);
            _spawnPoints = list.ToArray();
        }
    }

    void spawnReady()
    {
        _spawnReady = true;
    }


    public void SpawnOne(Vector3 spawnPosition)
    {
        // overlap sphere on the spawn point, if there is a spirit in the area then dont spawn another one
        GameObject go = Instantiate(_collectiblePrefab, spawnPosition, Quaternion.identity, _spawnParent);
        Collectible collectible = go.GetComponentInChildren<Collectible>();
        collectible.Initialize(this);
    }
    
    public void Collected()
    {
        Invoke(nameof(spawnReady), _normalSpawnInterval);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 0.2f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _overlapCheckSize);
    }
}