using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEventTester : MonoBehaviour
{
    [SerializeField]
    private BasicEvent _event;
    void Start()
    {
        _event.Invoke(0f);
    }

    public void HandleEvent(float v)
    {
        Debug.Log("Received float event: " + v);
    }

    public void HandleOnFire()
    {
        Debug.Log("Received on fire event");
    }
}
