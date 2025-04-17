using System.Collections;
using UnityEngine;

namespace MinD.Runtime.UI
{
    public class FadeCanvasGroup : MonoBehaviour
    {
        public float fadeDuration = 0.5f;
        public static bool isFading;
        public void FadeIn(CanvasGroup canvasGroup)
        {
            StartCoroutine(FadeCanvas(canvasGroup, 0f, 1f, fadeDuration));
        }

        public void FadeOut(CanvasGroup canvasGroup)
        {
            StartCoroutine(FadeCanvas(canvasGroup, 1f, 0f, fadeDuration));
        }

        private IEnumerator FadeCanvas(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration)
        {
            isFading = true;
            float time = 0f;
            canvasGroup.alpha = startAlpha;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            while (time < duration)
            {
                time += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, time / duration);
                yield return null;
            }

            canvasGroup.alpha = endAlpha;
            canvasGroup.interactable = endAlpha > 0.9f;
            canvasGroup.blocksRaycasts = endAlpha > 0.9f;
            isFading = false;
        }
    }
}