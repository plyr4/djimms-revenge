using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Collection : MonoBehaviour
{
    public TextMeshProUGUI _collectionText;
 
    public void HandleOnGameStateChange(GenericEventOpts opts)
    {
        switch (opts._newState)
        {
            case GStateRetry _:
            case GStateLoadPlay _:
                _collectionText.text = $"COLLECT <color=yellow>3</color> WISHES!";
                break;
            default:
                break;
        }
    }
    
    public void HandleCollectEvent(GenericEventOpts opts)
    {
        int left = 3 - opts._collector._collected;
        _collectionText.text = $"COLLECT <color=yellow>{left}</color> WISHES!";
        if (left == 1)
        {
            _collectionText.text = $"COLLECT <color=yellow>{left}</color> WISH!";
        }
    }
}
