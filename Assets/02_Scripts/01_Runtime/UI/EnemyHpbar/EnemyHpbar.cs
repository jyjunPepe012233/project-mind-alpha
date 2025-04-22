using MinD.Runtime.Entity;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyHpbar : MonoBehaviour
{
    [Header("Bar Images")]
    [SerializeField] private Image hpBarImage;
    [SerializeField] private Image damagedBarImage;

    [Header("Texts")]
    [SerializeField] public TextMeshProUGUI damageTMP;

    [Header("Root Transform")]
    [SerializeField] private RectTransform rootTransform;

    private float damageDisplayTime = 2f;
    private float lastDamageTime;
    private int previousHp = -1;
    private int accumulatedDamage = 0;
    private bool isDamageTextVisible = false;
    private float damageFadeTime = 3f;
    private float currentFadeTime = 0f;
    private float waitTimeBeforeFade = 0.5f;
    private float currentWaitTime = 0f;

    private void Start()
    {
        if (damageTMP != null)
            damageTMP.gameObject.SetActive(false); 

        if (damagedBarImage != null)
            damagedBarImage.fillAmount = 1f; 
    }

    private void Update()
    {
        var target = Player.player.combat.target;

        if (target != null && target is Enemy enemy)
        {
            int curHp = enemy.CurHp;

            if (curHp != previousHp)
            {
                int damage = previousHp - curHp;
                if (damage > 0)
                {
                    accumulatedDamage += damage;
                    UpdateDamage(accumulatedDamage);
                    lastDamageTime = Time.time;
                    damageTMP.gameObject.SetActive(true);
                    isDamageTextVisible = true;

                    currentWaitTime = 0f;
                    currentFadeTime = 0f;
                }
                previousHp = curHp;
            }

            if (Time.time - lastDamageTime > damageDisplayTime && isDamageTextVisible)
            {
                accumulatedDamage = 0;
                damageTMP.gameObject.SetActive(false);
                isDamageTextVisible = false;
            }

            if (curHp <= 0)
            {
                damageTMP.gameObject.SetActive(false);
                isDamageTextVisible = false;
            }

            UpdateHealthBar(curHp, enemy.attribute.MaxHp);

            if (currentWaitTime < waitTimeBeforeFade)
            {
                currentWaitTime += Time.deltaTime;
            }
            else
            {
                float targetFillAmount = (float)curHp / (float)enemy.attribute.MaxHp;

                if (currentFadeTime < damageFadeTime)
                {
                    currentFadeTime += Time.deltaTime;
                    damagedBarImage.fillAmount = Mathf.Lerp(damagedBarImage.fillAmount, targetFillAmount, currentFadeTime / damageFadeTime);
                }
                else
                {
                    damagedBarImage.fillAmount = targetFillAmount;
                }
            }
        }
    }
    
    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        if (hpBarImage != null)
        {
            float fillAmount = currentHealth / maxHealth;

            if (currentHealth <= 0)
                fillAmount = 0;
            hpBarImage.fillAmount = fillAmount;
        }
    }

    public void UpdateDamage(int damage)
    {
        if (damageTMP != null)
        {
            if (damage == 0)
            {
                damageTMP.gameObject.SetActive(false);
            }
            else
            {
                damageTMP.gameObject.SetActive(true);
                damageTMP.text = damage.ToString();
            }
        }
    }

    public void ResetDamage()
    {
        accumulatedDamage = 0;
        UpdateDamage(accumulatedDamage);
    }
    
    public void ResetAll(float newHp, float maxHp)
    {
        previousHp = (int)newHp;
        accumulatedDamage = 0;
        isDamageTextVisible = false;
        currentFadeTime = 0f;
        currentWaitTime = 0f;
        lastDamageTime = 0f;

        if (damageTMP != null)
        {
            damageTMP.text = "";
            damageTMP.gameObject.SetActive(false);
        }

        float fillAmount = (float)newHp / (float)maxHp;

        if (hpBarImage != null)
            hpBarImage.fillAmount = fillAmount;

        if (damagedBarImage != null)
            damagedBarImage.fillAmount = fillAmount;
    }

}
