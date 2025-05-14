using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class TutorialPopupView : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Image tutorialImage;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text contentText;
    [SerializeField] private GameObject popupRoot;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 1f;

    private Coroutine fadeCoroutine;

    public void Show(string title, string content)
    {
        // tutorialImage.sprite = image;
        titleText.text = title;
        contentText.text = content;

        popupRoot.SetActive(true);

        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeCanvasGroup(0f, 1f));
    }

    public void Close()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeOutAndDisable());
    }

    private IEnumerator FadeCanvasGroup(float start, float end)
    {
        float timer = 0f;
        canvasGroup.alpha = start;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, timer / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = end;
    }

    private IEnumerator FadeOutAndDisable()
    {
        float timer = 0f;
        float start = canvasGroup.alpha;
        float end = 0f;

        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, timer / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        popupRoot.SetActive(false);
    }
}
