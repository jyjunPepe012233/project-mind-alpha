using System;
using System.Collections.Generic;
using System.Linq;
using MinD.Enums;
using MinD.Runtime.Entity;
using MinD.Runtime.Managers;
using MinD.Runtime.UI;
using MinD.SO.Item;
using UnityEngine;

public class InventoryMenuPresenter : PlayerMenu
{
    [Header("인벤토리")] 
    [SerializeField] private CanvasGroup inventoryMenu;
    
    [Header("참조")]
    [SerializeField] private InventoryMenuView inventoryMenuView;
    [SerializeField] private PlayerInventoryHandler playerInventoryHandler;
    [SerializeField] private ItemDetailView itemDetailView;
    [SerializeField] private EquipmentView equipmentView;
    [SerializeField] private FadeCanvasGroup fadeCanvasGroup;
    
    private InventoryCategory currentCategory = InventoryCategory.Magic;
    private int selectedIndex = 0;

    private const int RowSize = 5;
    
    private int equipOrderCounter = 0;
    
    [Header("[ CanvasGroup ]")]
    public CanvasGroup quickSlots;
    public CanvasGroup statusGroup;


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

        if (equipmentView == null)
        {
            equipmentView = FindObjectOfType<EquipmentView>();
        }
        
        equipmentView.Initialize();
    }

    public override void Open()
    {
        inventoryMenuView.SetActiveCategory((int)currentCategory);
        selectedIndex = 0;
        UpdateSlots();
        
        
        fadeCanvasGroup.FadeOut(quickSlots);
        fadeCanvasGroup.FadeOut(statusGroup);
        
        equipmentView.UpdateAllSlots();
        UpdateSelection();
    }

    public override void Close()
    {
        fadeCanvasGroup.FadeIn(quickSlots);
        fadeCanvasGroup.FadeIn(statusGroup);
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
        var slots = inventoryMenuView.GetCurrentSlots((int)currentCategory);
        if (slots == null || selectedIndex >= slots.Count) return;

        var selectedSlot = slots[selectedIndex];
        var item = selectedSlot.GetItem();
        if (item == null) return;

        if (!item.isEquipped && !CanEquipItem(item))
        {
            PlayerHUDManager.Instance.PlayPopup($"슬롯이 가득 차서 {item.itemName}을(를) 장착할 수 없습니다.");
            return;
        }

        selectedSlot.ToggleEquippedState();
        item.isEquipped = !item.isEquipped;
    
        UpdatePlayerEquipmentSlots(item);
        UpdateSlots();
        UpdateSelection();
        if (item is Weapon weapon)
        {
            Player.player.equipment.ChangeWeapon(item.isEquipped ? weapon : null);
        }
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
        InventoryCategory.Staff => 1,
        InventoryCategory.Tool => 2,
        InventoryCategory.Protection => 3,
        _ => -1
    };

    if (playerInventoryHandler == null || playerInventoryHandler.playerItemList == null)
        return new List<ItemSlotData>();

    var result = new List<ItemSlotData>();

    // 기존 방식: Magic, Tool 카테고리는 equipped 슬롯 배열과 playerItemList를 별도로 관리
    if (category == InventoryCategory.Magic || category == InventoryCategory.Tool)
    {
        Item[] equippedSlotArray = category switch
        {
            InventoryCategory.Magic => playerInventoryHandler.magicSlots,
            InventoryCategory.Tool => playerInventoryHandler.toolSlots,
            _ => null
        };

        if (equippedSlotArray != null)
        {
            foreach (var item in equippedSlotArray)
            {
                if (item != null && item.isEquipped && item.categoryId == categoryId)
                    result.Add(new ItemSlotData(item, item.itemCount, true));
            }
        }

        // 장착되지 않은 아이템만 추가 (플레이어 인벤토리 리스트에서)
        var unequippedItems = playerInventoryHandler.playerItemList
            .Where(item => item != null && item.categoryId == categoryId && !item.isEquipped);
        foreach (var item in unequippedItems)
        {
            result.Add(new ItemSlotData(item, item.itemCount, false));
        }
    }
    // 새 방식: Staff, Protection은 장착되어도 playerItemList에 남아있고, isEquipped 표시만 함
    else if (category == InventoryCategory.Staff || category == InventoryCategory.Protection)
    {
        var categoryItems = playerInventoryHandler.playerItemList
            .Where(item => item != null && item.categoryId == categoryId)
            .OrderByDescending(item => item.isEquipped)  // 장착된 아이템이 위에 표시되도록
            .ThenBy(item => item.equippedOrder)            // 장착 순서 유지
            .ToList();

        foreach (var item in categoryItems)
        {
            result.Add(new ItemSlotData(item, item.itemCount, item.isEquipped));
        }
    }

    return result;
}




    
    private void UpdatePlayerEquipmentSlots(Item item)
{
    if (!item.isEquipped)
    {
        RemoveFromEquipment(item);
        return;
    }
    item.equippedOrder = equipOrderCounter++;
    switch (item.categoryId)
    {
        case 0: // Magic
            if (item is Magic magicItem)
            {
                // Magic 슬롯에 아이템을 첫 번째 인덱스로 배치
                for (int i = 0; i < playerInventoryHandler.magicSlots.Length; i++)
                {
                    if (playerInventoryHandler.magicSlots[i] == null) // 슬롯에 비어있는 공간이 있을 경우에만 삽입
                    {
                        playerInventoryHandler.magicSlots[i] = magicItem;
                        item.equippedOrder = -1;
                        break;
                    }
                }
                equipmentView.UpdateAllSlots();
            }
            break;

        case 1: // Weapon
            if (item is Weapon weaponItem)
                playerInventoryHandler.weaponSlot = weaponItem;
            item.equippedOrder = -1;
            equipmentView.UpdateAllSlots();
            break;

        case 2: // Tool
            if (item is Tool toolItem)
            {
                // Tool 슬롯에 아이템을 첫 번째 인덱스로 배치
                for (int i = 0; i < playerInventoryHandler.toolSlots.Length; i++)
                {
                    if (playerInventoryHandler.toolSlots[i] == null)
                    {
                        playerInventoryHandler.toolSlots[i] = toolItem;
                        item.equippedOrder = -1;
                        
                        break;
                    }
                }
                equipmentView.UpdateAllSlots();
            }
            break;

        case 3: // Protection
            if (item is Protection protectionItem)
                playerInventoryHandler.protectionSlot = protectionItem;
            item.equippedOrder = -1;
            equipmentView.UpdateAllSlots();
            break;

        default:
            Debug.LogWarning($"알 수 없는 categoryId: {item.categoryId}");
            break;
    }
}

private void RemoveFromEquipment(Item item)
{
    switch (item.categoryId)
    {
        case 0: // Magic
            if (item is Magic magicItem)
            {
                for (int i = 0; i < playerInventoryHandler.magicSlots.Length; i++)
                {
                    if (playerInventoryHandler.magicSlots[i] == magicItem)
                    {
                        playerInventoryHandler.magicSlots[i] = null;
                        item.equippedOrder = -1;
                        equipmentView.UpdateAllSlots();
                        break;
                    }
                }
            }
            break;

        case 1: // Weapon
            if (item is Weapon weaponItem && playerInventoryHandler.weaponSlot == weaponItem)
                playerInventoryHandler.weaponSlot = null;
            item.equippedOrder = -1;
            equipmentView.UpdateAllSlots();
            break;

        case 2: // Tool
            if (item is Tool toolItem)
            {
                for (int i = 0; i < playerInventoryHandler.toolSlots.Length; i++)
                {
                    if (playerInventoryHandler.toolSlots[i] == toolItem)
                    {
                        playerInventoryHandler.toolSlots[i] = null;
                        item.equippedOrder = -1;
                        equipmentView.UpdateAllSlots();
                        break;
                    }
                }
            }
            break;

        case 3: // Protection
            if (item is Protection protectionItem && playerInventoryHandler.protectionSlot == protectionItem)
                playerInventoryHandler.protectionSlot = null;
            item.equippedOrder = -1;
            equipmentView.UpdateAllSlots();
            break;
    }
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
    
    private bool CanEquipItem(Item item)
    {
        switch (item.categoryId)
        {
            case 0: // Magic
                if (item is Magic)
                {
                    for (int i = 0; i < playerInventoryHandler.magicSlots.Length; i++)
                    {
                        if (playerInventoryHandler.magicSlots[i] == null)
                            return true; 
                    }
                    return false; 
                }
                break;

            case 1: // Weapon
                return true;

            case 2: // Tool
                if (item is Tool)
                {
                    for (int i = 0; i < playerInventoryHandler.toolSlots.Length; i++)
                    {
                        if (playerInventoryHandler.toolSlots[i] == null)
                            return true;
                    }
                    return false; 
                }
                break;

            case 3: // Protection
                return true;
        }

        return true; // 기본적으로 장착 가능
    }
}
