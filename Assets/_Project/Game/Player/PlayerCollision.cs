using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField]
    private GenericEvent _onHitEvent;

    private void OnCollisionEnter(Collision other)
    {
        Spirit spirit = other.gameObject.GetComponent<Spirit>();
        if (spirit != null)
        {
            GenericEventOpts opts = new GenericEventOpts
            {
                _playerCollision = this
            };
            _onHitEvent.Invoke(opts);
            spirit.Kill(transform.position, false);
        }
    }
}