using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using MinD.Enums;
using MinD.Runtime.Entity;
using MinD.SO.Item;

public class EquipmentView : MonoBehaviour
{
    [Header("Slot Parents")]
    [SerializeField] private Transform toolSlotParent;
    [SerializeField] private Transform magicSlotParent;
    [SerializeField] private Transform staffSlotParent;
    [SerializeField] private Transform protectionSlotParent;

    private Dictionary<InventoryCategory, List<EquipmentSlotView>> slotMap;
    private PlayerInventoryHandler playerInventoryHandler;

    public void Initialize()
    {
        playerInventoryHandler = FindObjectOfType<PlayerInventoryHandler>();
        slotMap = new Dictionary<InventoryCategory, List<EquipmentSlotView>>
        {
            { InventoryCategory.Tool, GetSlotViews(toolSlotParent) },
            { InventoryCategory.Magic, GetSlotViews(magicSlotParent) },
            { InventoryCategory.Staff, GetSlotViews(staffSlotParent) },
            { InventoryCategory.Protection, GetSlotViews(protectionSlotParent) }
        };
    }

    private List<EquipmentSlotView> GetSlotViews(Transform parent)
    {
        List<EquipmentSlotView> slotViews = new();
        foreach (Transform child in parent)
        {
            var view = child.GetComponent<EquipmentSlotView>();
            if (view != null)
                slotViews.Add(view);
        }
        return slotViews;
    }

    public void UpdateAllSlots()
    {
        UpdateCategorySlots(InventoryCategory.Tool, playerInventoryHandler.toolSlots);
        UpdateCategorySlots(InventoryCategory.Magic, playerInventoryHandler.magicSlots);
        UpdateSingleCategorySlot(InventoryCategory.Staff, playerInventoryHandler.weaponSlot);
        UpdateSingleCategorySlot(InventoryCategory.Protection, playerInventoryHandler.protectionSlot);
        playerInventoryHandler.SortEquippedToolSlots();
        playerInventoryHandler.SortEquippedMagicSlots();
    }

    private void UpdateCategorySlots(InventoryCategory category, Item[] items)
    {
        if (!slotMap.TryGetValue(category, out var slotViews))
            return;

        // 정렬된 아이템 목록 가져오기
        var sortedItems = GetSortedItems(category);

        int slotIndex = 0;
        
        // 장착된 아이템만 할당
        foreach (var itemData in sortedItems)
        {
            if (itemData.IsEquipped && slotIndex < slotViews.Count)
            {
                slotViews[slotIndex].SetItem(itemData.Item.itemImage, true);
                slotIndex++;
            }
        }

        // 남은 슬롯 비우기
        for (int i = slotIndex; i < slotViews.Count; i++)
        {
            slotViews[i].ClearSlot();
        }
    }

    private void UpdateSingleCategorySlot(InventoryCategory category, Item item)
    {
        if (!slotMap.TryGetValue(category, out var slotViews))
            return;

        if (slotViews.Count > 0)
        {
            if (item != null)
                slotViews[0].SetItem(item.itemImage, true); // 장착된 아이템 처리
            else
                slotViews[0].ClearSlot();
        }

        // 첫 번째 슬롯을 제외한 나머지 슬롯은 비우기
        for (int i = 1; i < slotViews.Count; i++)
        {
            slotViews[i].ClearSlot();
        }
    }

    public void ClearAll()
    {
        foreach (var list in slotMap.Values)
        {
            foreach (var slot in list)
            {
                slot.ClearSlot();
            }
        }
    }

    // 정렬 함수
    private List<ItemSlotData> GetSortedItems(InventoryCategory category)
    {
        Item[] sourceSlots = category switch
        {
            InventoryCategory.Magic => playerInventoryHandler.magicSlots,
            InventoryCategory.Tool => playerInventoryHandler.toolSlots,
            _ => null
        };

        if (sourceSlots == null)
        {
            Debug.LogError($"{category} 슬롯 정보가 없습니다.");
            return new List<ItemSlotData>();
        }

        List<ItemSlotData> sortedList = new();

        foreach (var item in sourceSlots)
        {
            if (item != null && item.isEquipped)
            {
                sortedList.Add(new ItemSlotData(item, item.itemCount, true));
            }
        }

        return sortedList;
    }



    private class ItemSlotData
    {
        public Item Item;
        public int Count;
        public bool IsEquipped;

        public ItemSlotData(Item item, int count, bool isEquipped)
        {
            Item = item;
            Count = count;
            IsEquipped = isEquipped;
        }
    }

}
