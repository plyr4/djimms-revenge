using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Collection : MonoBehaviour
{
    public TextMeshProUGUI _collectionText;

    void Update()
    {
        int left = 3 - Collector.Instance._collected;
        _collectionText.text = $"COLLECT {left} MORE WISHES!";
    }
}
