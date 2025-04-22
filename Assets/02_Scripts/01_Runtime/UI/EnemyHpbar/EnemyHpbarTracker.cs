using MinD.Runtime.Entity;
using UnityEngine;

public class EnemyHpbarTracker : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform hpbarUI;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Vector3 offset = new Vector3(0, 5f, 0);
    [SerializeField] private float scaleFactor = 10f;
    [SerializeField] private float minScale = 0.5f;
    [SerializeField] private float maxScale = 2f;
    [SerializeField] private EnemyHpbar enemyHpbar;

    private Camera mainCamera;

    private Target targetComponent;
    private Target previousTarget;

    private void Awake()
    {
        mainCamera = Camera.main;
        hpbarUI.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!IsValidTarget(out targetComponent))
        {
            hpbarUI.gameObject.SetActive(false);
            return;
        }

        if (targetComponent != previousTarget)
        {
            ResetHpbar(targetComponent);
        }

        previousTarget = targetComponent;

        UpdateHpbarPosition(targetComponent);
    }

    private bool IsValidTarget(out Target target)
    {
        target = null;

        var combatTarget = Player.player.combat.target;

        if (!Player.player.isLockOn || combatTarget == null)
            return false;

        target = combatTarget.GetComponent<Target>();

        return target != null;
    }

    private void ResetHpbar(Target target)
    {
        if (enemyHpbar != null && target != null)
        {
            enemyHpbar.ResetAll(target.CurrentHealth, target.MaxHealth);
        }
    }

    private void UpdateHpbarPosition(Target target)
    {
        Transform targetTransform = target.transform;
        float enemyHeight = targetTransform.localScale.y;
        Vector3 dynamicOffset = offset + new Vector3(0, enemyHeight * 0.5f, 0);
        Vector3 worldPos = targetTransform.position + dynamicOffset;
        Vector3 screenPos = mainCamera.WorldToScreenPoint(worldPos);

        if (screenPos.z < 0)
        {
            hpbarUI.gameObject.SetActive(false);
            return;
        }

        UpdateScale(targetTransform);
        hpbarUI.gameObject.SetActive(true);
        hpbarUI.position = screenPos;

        UpdateHpbarInfo(target);
    }

    private void UpdateScale(Transform targetTransform)
    {
        float distance = Vector3.Distance(mainCamera.transform.position, targetTransform.position);
        float scale = Mathf.Clamp(scaleFactor / distance, minScale, maxScale);
        hpbarUI.localScale = Vector3.one * scale;
    }

    private void UpdateHpbarInfo(Target target)
    {
        enemyHpbar.UpdateHealthBar(target.CurrentHealth, target.MaxHealth); 
    }
}