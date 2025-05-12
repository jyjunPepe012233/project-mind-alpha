using UnityEngine;

public class FogColorChanger : MonoBehaviour
{
    public Color targetFogColor = Color.red;
    public float duration = 2f;

    private void OnTriggerEnter(Collider other)
    {
        FogColorController.ChangeFogColor(this, targetFogColor, duration);
    }
}