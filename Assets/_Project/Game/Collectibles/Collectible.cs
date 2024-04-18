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

    public void Start()
    {
        _smokePuff.transform.localScale = Vector3.one;
        _smokePuff.gameObject.SetActive(true);
        _smokePuff.Play();
    }
    
    public void Collect()
    {
        _factory.Collected();
        gameObject.SetActive(false);
        _smokePuff.transform.parent = null;
        _smokePuff.transform.position = transform.position - new Vector3(0f,0.2f,0f);
        _smokePuff.transform.rotation = Quaternion.Euler(90f,0f,0f);
        _smokePuff.transform.localScale = Vector3.one;
        _smokePuff.gameObject.SetActive(true);
        _smokePuff.Play();
    }
}
