using UnityEngine;

public class EnableOnStart : MonoBehaviour
{
    [SerializeField]
    private GameObject _target;
    void Awake()
    {
        _target.SetActive(true);
    }
    
    void Start()
    {
        _target.SetActive(true);
    }
}