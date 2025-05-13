using UnityEngine;

public class FogColorChanger : MonoBehaviour
{
    public Color targetFogColor = Color.red;
    public float targetFogDensity = 0f;
    public float duration = 2f;

    private void OnTriggerEnter(Collider other)
    {
        FogColorController.ChangeFogColor(this, targetFogColor, targetFogDensity, duration);
    }
}