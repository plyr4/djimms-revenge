using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Internal Components")]
    public Aimer _aimer;
    public Shooter _shooter;

    private void FixedUpdate()
    {
        if (Project.Input.GameInput.Instance._fire)
        {
            _shooter.Charge();
        }
        else
        {
            if (_shooter.Charged())
            {
                _shooter.Shoot(_aimer);
            }
        }
    }
    
    private void OnCollisionEnter(Collision other)
    {
        Spirit spirit = other.gameObject.GetComponent<Spirit>();
        if (spirit != null)
        {
            gameObject.SetActive(false);
            Debug.Log("Player has been hit!");
            // time scale to zero
            Time.timeScale = 0;
        }
    }
}