using System.Collections;
using MinD.Runtime.Entity;
using MinD.SO.Item;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionPanelController : MonoBehaviour
{
    public GameObject interactionPanel;
    public GameObject itemRootingPanel;
    public CanvasGroup rootingCanvasGroup;
    public TextMeshProUGUI interactionText;
    public TextMeshProUGUI itemNameText;
    public Image itemImage;
    public float fadeOutDuration = 1f;

    private Coroutine fadeOutCoroutine; // 현재 실행 중인 코루틴 저장
    public static InteractionPanelController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void RefreshInteractionPanel()
    {
        if (Player.player.interaction.currentInteractables.Count == 0)
        {
            UnDisplayItemInteractionPanel();
        }
        else
        {
            DisplayItemInteractionPanel();
            interactionText.text = Player.player.interaction.currentInteractables[0].interactionText;
        }
    }

    public void DisplayItemInteractionPanel()
    {
        interactionPanel.SetActive(true);
    }

    public void UnDisplayItemInteractionPanel()
    {
        interactionPanel.SetActive(false);
    }

    public void ShowLootingPanel(Item item)
    {
        itemNameText.text = item.itemName;
        itemImage.sprite = item.itemImage;

        itemRootingPanel.SetActive(true);
        rootingCanvasGroup.alpha = 1f;

        // 코루틴 중복 방지
        if (fadeOutCoroutine != null)
        {
            StopCoroutine(fadeOutCoroutine);
        }
        fadeOutCoroutine = StartCoroutine(FadeOutLootingPanel());
    }

    private IEnumerator FadeOutLootingPanel()
    {
        yield return new WaitForSeconds(1.5f);

        float elapsedTime = 0f;
        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            rootingCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutDuration);
            yield return null;
        }

        rootingCanvasGroup.alpha = 0f;
        itemRootingPanel.SetActive(false);
        fadeOutCoroutine = null; // 종료된 코루틴 null 처리
    }
}
