using System.Collections;
using System.Collections.Generic;
using MinD.SO.Item;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class InventorySlotView : MonoBehaviour
{
    [SerializeField] private Image inventorySlotStroke;
    [SerializeField] private Image inventorySelectedSlotStroke;
    [SerializeField] private Image inventorySlotBackground;
    [SerializeField] private Image inventorySlotItemImage;
    [SerializeField] private TextMeshProUGUI equipedTMP;
    [SerializeField] private TextMeshProUGUI countTMP;

    // 슬롯에 아이템 표시
    public void Set(Item item, int count, bool isEquipped)
    {
        if (item == null)
        {
            Clear();
            return;
        }

        inventorySlotItemImage.enabled = true;
        inventorySlotItemImage.sprite = item.itemImage;

        equipedTMP.gameObject.SetActive(isEquipped);
        countTMP.text = count > 1 ? count.ToString() : "";
    }

    // 슬롯 초기화 (비우기)
    public void Clear()
    {
        inventorySlotItemImage.enabled = false;
        inventorySlotItemImage.sprite = null;

        equipedTMP.gameObject.SetActive(false);
        countTMP.text = "";
    }

    // 선택 상태 표현
    public void SetSelected(bool isSelected)
    {
        inventorySelectedSlotStroke.enabled = isSelected;
    }
}
