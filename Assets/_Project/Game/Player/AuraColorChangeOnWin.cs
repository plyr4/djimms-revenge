using UnityEngine;

public class AuraColorChangeOnWin : MonoBehaviour
{
    public Color _onWinAuraColor = Color.yellow;
    public void HandleGameOverWinEvent(GenericEventOpts opts)
    {
        // change the color of the aura to yellow
        GetComponent<Renderer>().material.color = _onWinAuraColor;
    }
}
