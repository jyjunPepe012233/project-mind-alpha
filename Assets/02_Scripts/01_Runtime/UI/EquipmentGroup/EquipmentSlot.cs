using System;
using MinD.SO.Item;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    public Image itemImage;
    public Text itemCountText;
    public GameObject selectionImage;
    public int categoryId;
    public int slotIndex;

    [SerializeField] public Item currentItem;

    public void UpdateSlot(Item item)
    {
        if (item == null)
        {
            ClearSlot();
            return;
        }

        currentItem = item;

        if (item.itemCount <= 0)
        {
            ClearSlot();
            return;
        }

        itemImage.sprite = item.itemImage;
        itemImage.gameObject.SetActive(true);

        if (item.itemCount > 1)
        {
            itemCountText.text = item.itemCount.ToString();
            itemCountText.gameObject.SetActive(true);
        }
        else
        {
            itemCountText.gameObject.SetActive(false);
        }
    }

    public void ClearSlot()
    {
        currentItem = null;
        itemImage.gameObject.SetActive(false);
        itemCountText.gameObject.SetActive(false);
    }

    public void OnSlotClicked()
    {
        if (currentItem != null)
        {
            Debug.Log($"슬롯 클릭: 카테고리 ID: {categoryId}, 슬롯 인덱스: {slotIndex}, 아이템 이름: {currentItem.itemName}");
        }
        else
        {
            Debug.Log($"슬롯 클릭: 카테고리 ID: {categoryId}, 슬롯 인덱스: {slotIndex}, 빈 슬롯");
        }
    }
}
