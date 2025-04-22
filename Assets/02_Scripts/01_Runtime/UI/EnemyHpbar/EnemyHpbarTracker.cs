using MinD.Runtime.Entity;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpbarTracker : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform hpbarUI;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Vector3 offset = new Vector3(0, -5f, 0); // 기본 offset 값
    [SerializeField] private float scaleFactor = 10f; // 거리에 따른 스케일 보정값
    [SerializeField] private float minScale = 0.5f;
    [SerializeField] private float maxScale = 2f;

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
        hpbarUI.gameObject.SetActive(false);
    }

    private void Update()
    {
        var target = Player.player.combat.target;

        if (!Player.player.isLockOn || target == null || target.targetOptions == null || target.targetOptions.Count == 0)
        {
            hpbarUI.gameObject.SetActive(false);
            return;
        }

        Transform currentTransform = target.targetOptions[0].transform;

        if (currentTransform == null)
        {
            hpbarUI.gameObject.SetActive(false);
            return;
        }

        float enemyHeight = currentTransform.localScale.y;
        Vector3 dynamicOffset = offset + new Vector3(0, enemyHeight * -1f, 0);
        Vector3 worldPos = currentTransform.position + dynamicOffset;
        Vector3 screenPos = mainCamera.WorldToScreenPoint(worldPos);

        if (screenPos.z < 0)
        {
            hpbarUI.gameObject.SetActive(false);
            return;
        }

        // 거리 기반 스케일 조정
        float distance = Vector3.Distance(mainCamera.transform.position, currentTransform.position);
        float scale = Mathf.Clamp(scaleFactor / distance, minScale, maxScale);
        hpbarUI.localScale = Vector3.one * scale;

        hpbarUI.gameObject.SetActive(true);
        hpbarUI.position = screenPos;
    }
}