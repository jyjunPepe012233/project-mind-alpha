using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyHpbarView : MonoBehaviour
{
    [Header("Bar Images")]
    [SerializeField] private Image hpBarImage;
    [SerializeField] private Image damagedBarImage;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI enemyNameTMP;
    [SerializeField] private TextMeshProUGUI damageTMP;

    [Header("Root Transform")]
    [SerializeField] private RectTransform rootTransform;

    public void SetHp(float normalizedValue)
    {
        hpBarImage.fillAmount = normalizedValue;
    }

    public void SetDamagedHp(float normalizedValue)
    {
        damagedBarImage.fillAmount = normalizedValue;
    }

    public void SetEnemyName(string name)
    {
        enemyNameTMP.text = name;
    }

    public void SetDamageText(string damageText)
    {
        damageTMP.text = damageText;
    }

    public void ClearDamageText()
    {
        damageTMP.text = "";
    }

    public void SetActive(bool isActive)
    {
        rootTransform.gameObject.SetActive(isActive);
    }

    public void SetPosition(Vector3 screenPosition)
    {
        rootTransform.position = screenPosition;
    }

    public void SetScale(float scale)
    {
        rootTransform.localScale = Vector3.one * scale;
    }
}