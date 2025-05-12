using System.Collections;
using UnityEngine;

public static class FogColorController
{
    private static Coroutine currentCoroutine;
    private static MonoBehaviour currentRunner;

    public static void ChangeFogColor(MonoBehaviour runner, Color targetColor, float duration)
    {
        if (currentCoroutine != null && currentRunner != null)
            currentRunner.StopCoroutine(currentCoroutine);

        currentRunner = runner;
        currentCoroutine = runner.StartCoroutine(ChangeFogColorCoroutine(targetColor, duration));
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
        currentCoroutine = null;
        currentRunner = null;
    }
}