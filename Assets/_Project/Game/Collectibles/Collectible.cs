using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public CollectiblesFactory _factory;
    public ParticleSystem _smokePuff;
    public void Initialize(CollectiblesFactory factory)
    {
        _factory = factory;
    }

    public void Collect()
    {
        _factory.Collected();
        gameObject.SetActive(false);
        _smokePuff.transform.parent = null;
        _smokePuff.transform.position = transform.position;
        _smokePuff.Play();
    }
}
