using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MinD.Runtime.Entity;
using MinD.Runtime.Managers;

public class BossHpbar : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image hpBar;
    [SerializeField] private Image damagedHpBar;
    [SerializeField] private TextMeshProUGUI bossNameTMP;
    [SerializeField] private TextMeshProUGUI damageTMP;

    [Header("Settings")]
    [SerializeField] private float damagedHpLerpSpeed = 1.5f;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float fadeDelay = 2f;
    [SerializeField] private float damagedHpDelay = 1f;

    private Enemy boss;
    private int lastHp;
    private int accumulatedDamage;

    private Coroutine fadeCoroutine;
    private Coroutine resetDamageCoroutine;
    private Coroutine delayedDamagedHpCoroutine;
    private Coroutine moveDamagedHpCoroutine;

    public bool IsBossFighting { get; private set; }

    private void Awake()
    {
        canvasGroup.alpha = 0f;
    }

    private void OnEnable()
    {
        BossFightManager.Instance.OnBossFightStart += HandleBossFightStart;
        BossFightManager.Instance.OnBossFightFinish += HandleBossFightFinish;
    }

    private void OnDisable()
    {
        if (BossFightManager.Instance == null) return;
        BossFightManager.Instance.OnBossFightStart -= HandleBossFightStart;
        BossFightManager.Instance.OnBossFightFinish -= HandleBossFightFinish;
    }

    private void Update()
    {
        if (!IsBossFighting || boss == null) return;

        UpdateHpBar();
        UpdateDamageDisplay();
    }

    private void HandleBossFightStart()
    {
        boss = BossFightManager.Instance.GetCurrentBoss();
        if (boss == null)
        {
            Debug.LogError("[BossHpbar] Boss is NULL after BossFightStart!");
            return;
        }

        Debug.Log("[BossHpbar] Boss fight started with boss: " + boss.name);

        IsBossFighting = true;
        bossNameTMP.text = boss.name;
        lastHp = boss.CurHp;
        accumulatedDamage = 0;
        damageTMP.text = "";

        FadeIn();
    }


    private void HandleBossFightFinish(bool isBossFelled)
    {
        IsBossFighting = false;
        boss = null;

        hpBar.fillAmount = 0f;

        FadeOut();
    }


    private void UpdateHpBar()
    {
        float currentHpPercent = (float)boss.CurHp / boss.attribute.MaxHp;
        hpBar.fillAmount = currentHpPercent;
    }

    private void UpdateDamageDisplay()
    {
        int currentHp = boss.CurHp;

        if (currentHp < lastHp)
        {
            int damageTaken = lastHp - currentHp;
            accumulatedDamage += damageTaken;
            damageTMP.text = accumulatedDamage.ToString();

            if (resetDamageCoroutine != null)
                StopCoroutine(resetDamageCoroutine);
            resetDamageCoroutine = StartCoroutine(ResetDamageText());

            if (delayedDamagedHpCoroutine != null)
                StopCoroutine(delayedDamagedHpCoroutine);

            if (moveDamagedHpCoroutine != null)
                StopCoroutine(moveDamagedHpCoroutine);

            delayedDamagedHpCoroutine = StartCoroutine(DelayAndStartDamagedHpMove());

            lastHp = currentHp;
        }
    }

    private IEnumerator DelayAndStartDamagedHpMove()
    {
        yield return new WaitForSeconds(damagedHpDelay);

        moveDamagedHpCoroutine = StartCoroutine(SmoothDamagedHpBar(hpBar.fillAmount));
    }

    private IEnumerator SmoothDamagedHpBar(float targetFill)
    {
        while (damagedHpBar.fillAmount > targetFill)
        {
            damagedHpBar.fillAmount = Mathf.MoveTowards(damagedHpBar.fillAmount, targetFill, Time.deltaTime * damagedHpLerpSpeed);
            yield return null;
        }

        damagedHpBar.fillAmount = targetFill;
    }

    private IEnumerator ResetDamageText()
    {
        yield return new WaitForSeconds(2f);
        accumulatedDamage = 0;
        damageTMP.text = "";
    }

    private void FadeIn()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeCanvasGroup(1f));
    }

    private void FadeOut()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeCanvasGroup(0f));
    }

    private IEnumerator FadeCanvasGroup(float targetAlpha)
    {
        yield return new WaitForSeconds(fadeDelay);

        float startAlpha = canvasGroup.alpha;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
    }
}
