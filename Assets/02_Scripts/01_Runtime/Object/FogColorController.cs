using UnityEngine;
using System.Collections;

public static class FogColorController
{
    private static Coroutine coroutine;

    public static void ChangeFogColor(MonoBehaviour runner, Color targetColor, float duration)
    {
        if (coroutine != null)
            runner.StopCoroutine(coroutine);

        coroutine = runner.StartCoroutine(ChangeFogColorCoroutine(targetColor, duration));
    }

    private static IEnumerator ChangeFogColorCoroutine(Color targetColor, float duration)
    {
        Color startColor = RenderSettings.fogColor;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            RenderSettings.fogColor = Color.Lerp(startColor, targetColor, time / duration);
            yield return null;
        }

        RenderSettings.fogColor = targetColor;
        coroutine = null;
    }
}