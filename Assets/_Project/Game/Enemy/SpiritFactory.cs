using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpiritFactory : MonoBehaviour
{
    [SerializeField]
    private GameObject _spiritPrefab;
    public Vector2 _initialRotationForceRange = new Vector2(-0.08f, 0.05f);
    [SerializeField]
    private float _overlapCheckSize = 0.5f;
    [SerializeField]
    private LayerMask _spiritLayerMask;
    [SerializeField]
    private Transform _spiritsParent;
    [SerializeField]
    private float _currentSpawnInterval = 1f;
    [SerializeField]
    private float _minSpawnInterval = 0.4f;
    [SerializeField]
    private float _spawnIntervalDecreaseRate = 0.05f;
    [SerializeField]
    private float _initialSpawnDelay = 1f;

    void Start()
    {
        StopAllCoroutines();
    }

    public void HandleOnGameStateChange(GenericEventOpts opts)
    {
        switch (opts._newState)
        {
            case GStateNull _:
                break;
            case GStatePlay _:
                StopAllCoroutines();
                StartCoroutine(SpawnSpiritChained(_initialSpawnDelay));
                break;
            default:
                break;
        }
    }

    IEnumerator SpawnSpiritChained(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnSpirit(transform.position);
        float f = Random.Range(0.8f, 1.2f);
        float interval = _currentSpawnInterval * f;
        _currentSpawnInterval = Mathf.Max(_minSpawnInterval, _currentSpawnInterval - _spawnIntervalDecreaseRate);
        StartCoroutine(SpawnSpiritChained(interval));
    }

    public void SpawnSpirit(Vector3 spawnPosition)
    {
        Collider[] colliders = Physics.OverlapSphere(spawnPosition, _overlapCheckSize, _spiritLayerMask);
        if (colliders.Length > 0) return;

        // overlap sphere on the spawn point, if there is a spirit in the area then dont spawn another one
        GameObject go = Instantiate(_spiritPrefab, spawnPosition, Quaternion.identity, _spiritsParent);
        Spirit spirit = go.GetComponent<Spirit>();
        spirit.Initialize(this);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 0.2f);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _overlapCheckSize);
    }
}