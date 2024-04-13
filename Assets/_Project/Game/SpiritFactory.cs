using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameStateEventListener))]
public class SpiritFactory : MonoBehaviour
{
    [SerializeField]
    private GameObject _spiritPrefab;
    public Vector2 _initialRotationForceRange = new Vector2(-0.08f, 0.05f);

    void Start()
    {
        // InvokeRepeating(nameof(SpawnSpirit), 1f, 1f);
    }

    public void HandleOnGameStateChange(GStateBase previousState, GStateBase nextState)
    {
        switch (nextState)
        {
            case GStateNull _:
                break;
            case GStatePlay _:
                InvokeRepeating(nameof(SpawnSpirit), 1f, 1f);
                break;
            default:
                break;
        }
    }

    void SpawnSpirit()
    {
        GameObject go = Instantiate(_spiritPrefab, transform.position, Quaternion.identity, transform);
        Spirit spirit = go.GetComponent<Spirit>();
        spirit.Initialize(this);
    }
}