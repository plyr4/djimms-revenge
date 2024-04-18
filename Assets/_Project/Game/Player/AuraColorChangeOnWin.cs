using UnityEngine;

public class AuraColorChangeOnWin : MonoBehaviour
{
    public Color _onWinAuraColor = Color.yellow;
    public MeshRenderer _auraRenderer;
    public void HandleGameOverWinEvent(GenericEventOpts opts)
    {
        // change the color of the aura to yellow
        _auraRenderer.material.color = _onWinAuraColor;
    }
}
