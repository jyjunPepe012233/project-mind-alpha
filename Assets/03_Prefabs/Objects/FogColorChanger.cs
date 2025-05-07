using UnityEngine;

public class FogColorChanger : MonoBehaviour
{
    public Color targetFogColor = Color.red;
    
    private void OnTriggerEnter(Collider other)
    {
        ChangeFogColor();
    }
    
    private void ChangeFogColor()
    {
        RenderSettings.fogColor = targetFogColor;
    }
}