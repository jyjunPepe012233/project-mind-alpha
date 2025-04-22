using MinD.Runtime.Entity;
using UnityEngine;

public class EnemyHpbarTracker : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform hpbarUI;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Vector3 offset = new Vector3(0, 2f, 0);
    [SerializeField] private float scaleFactor = 10f;
    [SerializeField] private float minScale = 0.5f;
    [SerializeField] private float maxScale = 2f;
    [SerializeField] private EnemyHpbar enemyHpbar;

    private Camera mainCamera;

    private Enemy previousEnemy;
    private float targetLostTimer = 0f;
    private float targetGraceDuration = 2f;

    private void Awake()
    {
        mainCamera = Camera.main;
        hpbarUI.gameObject.SetActive(false);
    }

    private void Update()
    {
        bool hasTarget = TryGetTargetEnemy(out Enemy enemy);

        if (hasTarget)
        {
            if (enemy != previousEnemy)
            {
                ResetHpbar(enemy);
            }

            previousEnemy = enemy;
            targetLostTimer = 0f;

            UpdateHpbarPosition(enemy);
        }
        else
        {
            if (previousEnemy != null)
            {
                targetLostTimer += Time.deltaTime;

                if (targetLostTimer < targetGraceDuration)
                {
                    UpdateHpbarPosition(previousEnemy);
                    return;
                }
            }

            previousEnemy = null;
            hpbarUI.gameObject.SetActive(false);
        }
    }

    private bool TryGetTargetEnemy(out Enemy enemy)
    {
        enemy = null;

        var combatTarget = Player.player.combat.target;

        if (!Player.player.isLockOn || combatTarget == null)
            return false;

        enemy = combatTarget.GetComponent<Enemy>();
        return enemy != null;
    }

    private void ResetHpbar(Enemy enemy)
    {
        var attributeHandler = enemy.GetComponent<EnemyAttributeHandler>();
        float maxHp = attributeHandler != null ? attributeHandler.MaxHp : 100f;
        float curHp = enemy.CurHp;

        enemyHpbar.ResetAll(curHp, maxHp);
    }

    private void UpdateHpbarPosition(Enemy enemy)
    {
        Transform targetTransform = enemy.transform;
        float enemyHeight = targetTransform.localScale.y;
        Vector3 dynamicOffset = new Vector3(0, enemyHeight * offset.y, 0);
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

        UpdateHpbarInfo(enemy);
    }

    private void UpdateScale(Transform targetTransform)
    {
        float distance = Vector3.Distance(mainCamera.transform.position, targetTransform.position);
        float scale = Mathf.Clamp(scaleFactor / distance, minScale, maxScale);
        hpbarUI.localScale = Vector3.one * scale;
    }

    private void UpdateHpbarInfo(Enemy enemy)
    {
        var attributeHandler = enemy.GetComponent<EnemyAttributeHandler>();
        float maxHp = attributeHandler != null ? attributeHandler.MaxHp : 100f;
        float curHp = enemy.CurHp;

        enemyHpbar.UpdateHealthBar(curHp, maxHp);
    }
}
