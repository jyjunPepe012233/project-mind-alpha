using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;

public class TutorialPopupView : MonoBehaviour
{
    public event Action<TutorialPopupView> OnPopupClosed;

    [Header("UI Components")]
    [SerializeField] private Image tutorialImage;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text contentText;
    [SerializeField] private GameObject popupRoot;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image timerBar;

    [Header("Animation Settings")]
    [SerializeField] private float fadeDuration = 0.3f;

    [Header("Timer Settings")]
    [SerializeField] private bool useTimer = false;
    [SerializeField] private float displayTime = 5f;

    private Coroutine fadeCoroutine;
    private Coroutine timerCoroutine;
    private bool isClosing = false;


    private void Awake()
    {
        if (timerBar != null && timerBar.type != Image.Type.Filled)
        {
            timerBar.type = Image.Type.Filled;
        }
    }

    public void ShowWithTimer(string title, string content, float displayTime)
    {
        isClosing = false;

        titleText.text = title;
        contentText.text = content;

        StopAllCoroutines();

        popupRoot.SetActive(true);
        canvasGroup.alpha = 0f;

        if (displayTime > 0 && timerBar != null)
        {
            timerBar.gameObject.SetActive(true);
            timerBar.fillAmount = 1.0f;
            timerCoroutine = StartCoroutine(TimerRoutine(displayTime));
        }
        else if (timerBar != null)
        {
            timerBar.gameObject.SetActive(false);
        }

        fadeCoroutine = StartCoroutine(FadeCanvasGroup(0f, 1f));
    }

    public void Close()
    {
        if (isClosing) return;
        isClosing = true;

        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }

        StartCoroutine(CloseWithAnimation());
    }

    private IEnumerator CloseWithAnimation()
    {
        yield return StartCoroutine(FadeCanvasGroup(canvasGroup.alpha, 0f));
        popupRoot.SetActive(false);
        isClosing = false;
        OnPopupClosed?.Invoke(this); // 팝업이 닫혔음을 알림
    }

    private IEnumerator FadeCanvasGroup(float start, float end)
    {
        float timer = 0f;
        canvasGroup.alpha = start;
        canvasGroup.blocksRaycasts = (end > 0);
        canvasGroup.interactable = (end > 0);

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, timer / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = end;
    }

    private IEnumerator TimerRoutine(float displayTime)
    {
        float remainingTime = displayTime;

        while (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;

            if (timerBar != null)
            {
                timerBar.fillAmount = Mathf.Clamp01(remainingTime / displayTime);
            }

            yield return null;
        }

        if (!isClosing)
        {
            Close();
        }
    }
}