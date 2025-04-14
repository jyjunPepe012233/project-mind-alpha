using System;
using System.Collections.Generic;
using System.Linq;
using MinD.Enums;
using MinD.Runtime.Entity;
using MinD.Runtime.UI;
using MinD.SO.Item;
using UnityEngine;

public class InventoryMenuPresenter : PlayerMenu
{
    [SerializeField] private InventoryMenuView inventoryMenuView;
    [SerializeField] private PlayerInventoryHandler playerInventoryHandler;
    [SerializeField] private ItemDetailView itemDetailView;
    
    private InventoryCategory currentCategory = InventoryCategory.Magic;
    private int selectedIndex = 0;

    private const int RowSize = 5;

    public void Awake()
    {
        if (playerInventoryHandler == null)
        {
            playerInventoryHandler = FindObjectOfType<PlayerInventoryHandler>();
        }

        if (itemDetailView == null)
        {
            itemDetailView = FindObjectOfType<ItemDetailView>();
        }
    }

    public override void Open()
    {
        inventoryMenuView.SetActiveInventoryMenu(true);
        inventoryMenuView.SetActiveCategory((int)currentCategory);
        selectedIndex = 0;
        UpdateSelection();
        UpdateSlots();
    }

    public override void Close()
    {
        inventoryMenuView.SetActiveInventoryMenu(false);
    }

    public override void OnInputWithDirection(Vector2 inputDir)
    {
        int column = selectedIndex % RowSize;
        int row = selectedIndex / RowSize;

        if (inputDir.x > 0 && column < RowSize - 1) selectedIndex++;
        if (inputDir.x < 0 && column > 0) selectedIndex--;
        if (inputDir.y > 0 && row > 0) selectedIndex -= RowSize;
        if (inputDir.y < 0 && row < 4) selectedIndex += RowSize;

        UpdateSelection();
    }

    public override void OnSelectInput()
    {
        Debug.Log("선택 슬롯에서 아이템 액션 패널 실행");
    }

    public override void OnMoveTabInput(int inputDir)
    {
        int categoryCount = System.Enum.GetValues(typeof(InventoryCategory)).Length;
        int nextCategory = ((int)currentCategory + inputDir + categoryCount) % categoryCount;

        currentCategory = (InventoryCategory)nextCategory;
        inventoryMenuView.SetActiveCategory(nextCategory);
        selectedIndex = 0;

        UpdateSlots();
        UpdateSelection();
    }

    private void UpdateSelection()
    {
        var slots = inventoryMenuView.GetCurrentSlots((int)currentCategory);
        if (slots == null) return;

        for (int i = 0; i < slots.Count; i++)
            slots[i].SetSelected(i == selectedIndex);

        if (selectedIndex < slots.Count)
        {
            var selectedSlot = slots[selectedIndex];
            var item = selectedSlot.GetItem();
            itemDetailView.ShowItemDetails(item);
        }
        else
        {
            itemDetailView.Clear();
        }
    }


    private void UpdateSlots()
    {
        var slots = inventoryMenuView.GetCurrentSlots((int)currentCategory);
        if (slots == null) return;

        var categoryItems = GetSortedItems(currentCategory);

        for (int i = 0; i < slots.Count; i++)
        {
            if (i < categoryItems.Count)
            {
                var itemData = categoryItems[i];
                slots[i].SetItem(itemData.Item, itemData.Count, itemData.IsEquipped);
            }
            else
            {
                slots[i].Clear();
            }
        }
    }

    private List<ItemSlotData> GetSortedItems(InventoryCategory category)
    {
        int categoryId = category switch
        {
            InventoryCategory.Magic => 0,
            InventoryCategory.Staff => 18,
            InventoryCategory.Tool => 1,
            InventoryCategory.Protection => 2,
            _ => -1
        };

        if (playerInventoryHandler == null)
        {
            Debug.LogError("playerInventoryHandler가 null입니다.");
            return new List<ItemSlotData>();
        }

        if (playerInventoryHandler.playerItemList == null)
        {
            Debug.LogError("PlayerItemList가 null입니다.");
            return new List<ItemSlotData>();
        }

        var allItems = playerInventoryHandler.playerItemList;

        var filteredItems = allItems
            .Where(item => item != null && item.categoryId == categoryId)
            .ToList();

        var result = new List<ItemSlotData>();

        foreach (var item in filteredItems)
        {
            if (item.isEquipped)
                result.Insert(0, new ItemSlotData(item, item.itemCount, true));
        }

        foreach (var item in filteredItems)
        {
            if (!item.isEquipped)
                result.Add(new ItemSlotData(item, item.itemCount, false));
        }

        return result;
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
