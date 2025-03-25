using MinD.Runtime.Entity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MinD.SO.Item;

namespace MinD.Runtime.UI
{
    public class InteractionPanelController : MonoBehaviour
    {
        public GameObject interactionPanel;      // 기존 상호작용 패널
        public GameObject itemRootingPanel;     // 루팅 패널
        public CanvasGroup rootingCanvasGroup;  // 루팅 패널의 CanvasGroup (투명도 조정용)
        public TextMeshProUGUI itemNameText;    // 루팅 패널의 아이템 이름
        public Image itemImage;                 // 루팅 패널의 아이템 이미지
        public float fadeOutDuration = 1f;      // 페이드 아웃 지속 시간

        private bool isFadingOut = false;       // 페이드 아웃 상태 확인용

        // 상호작용 패널 업데이트
        public void RefreshInteractionPanel()
        {
            if (Player.player.interaction.currentInteractables.Count == 0)
            {
                UnDisplayItemInteractionPanel();
            }
            else
            {
                DisplayItemInteractionPanel();
                interactionPanel.GetComponentInChildren<TextMeshProUGUI>().text = Player.player.interaction.currentInteractables[0].interactionText;
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
            isFadingOut = false;

            StartCoroutine(FadeOutLootingPanel());
        }

        private IEnumerator FadeOutLootingPanel()
        {
            isFadingOut = true;
            float elapsedTime = 0f;

            yield return new WaitForSeconds(1.5f);
            
            while (elapsedTime < fadeOutDuration)
            {
                elapsedTime += Time.deltaTime;
                rootingCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutDuration);
                yield return null;
            }

            rootingCanvasGroup.alpha = 0f;
            itemRootingPanel.SetActive(false);
            isFadingOut = false;
        }
    }
}
