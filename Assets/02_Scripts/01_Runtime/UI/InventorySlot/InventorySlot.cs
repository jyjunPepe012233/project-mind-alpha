using MinD.SO.Item;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Image inventorySlotStroke;
    [SerializeField] private GameObject inventorySelectedSlotStroke;
    [SerializeField] private Image inventorySlotBackground;
    [SerializeField] private Image inventorySlotItemImage;
    [SerializeField] private TextMeshProUGUI equipedTMP;
    [SerializeField] private TextMeshProUGUI countTMP;

    private Item item;
    private int count;
    private bool isEquipped;

    // 아이템 상태를 설정하는 메서드
    public void SetItem(Item newItem, int newCount, bool newIsEquipped)
    {
        item = newItem;
        count = newCount;
        isEquipped = newIsEquipped;

        UpdateView();
    }

    // 슬롯 초기화 메서드
    public void Clear()
    {
        item = null;
        count = 0;
        isEquipped = false;

        UpdateView();
    }

    // 슬롯의 UI를 업데이트하는 메서드
    private void UpdateView()
    {
        if (item == null)
        {
            ClearSlot();
            return;
        }

        inventorySlotItemImage.enabled = true;
        inventorySlotItemImage.sprite = item.itemImage;

        equipedTMP.gameObject.SetActive(isEquipped);
        countTMP.text = count > 1 ? count.ToString() : "";
    }

    // 아이템이 없는 슬롯을 비우는 메서드
    private void ClearSlot()
    {
        inventorySlotItemImage.enabled = false;
        inventorySlotItemImage.sprite = null;

        equipedTMP.gameObject.SetActive(false);
        countTMP.text = "";
    }

    // 선택 상태 표현
    public void SetSelected(bool isSelected)
    {
        inventorySelectedSlotStroke.SetActive(isSelected);
    }

    // 슬롯 상태를 토글하는 메서드 (장착 상태 변경)
    public void ToggleEquippedState()
    {
        isEquipped = !isEquipped;
        UpdateView();
    }

    // 아이템 수를 변경하는 메서드
    public void SetItemCount(int newCount)
    {
        count = newCount;
        UpdateView();
    }
}