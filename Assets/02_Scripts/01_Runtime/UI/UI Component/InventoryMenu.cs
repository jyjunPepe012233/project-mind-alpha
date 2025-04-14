using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using MinD.Runtime.Entity;
using MinD.Runtime.Managers;
using MinD.SO.Item;
using UnityEngine.EventSystems;
using NotImplementedException = System.NotImplementedException;

namespace MinD.Runtime.UI
{
    public class InventoryMenu : PlayerMenu
{
    public ItemActionPanel itemActionPanel;

    public Text itemNameText;
    public Text itemDescriptionText;
    public GameObject itemDescriptionGroup;
        
    public GameObject slotPrefab;
    public ScrollRect scrollRect;
    public List<Transform> categoryPolygon;
    public List<Transform> categoryPanels;
    private List<List<InventorySlotTrash>> categorySlots = new();

    private int selectedSlotIndex = 0;
    private int inventoryWidth = 5;
    [SerializeField] PlayerInventoryHandler playerInventory;
    private ItemSOList itemSoList;

    private int currentCategoryIndex = 0;

    public GameObject inventoryPanel;
    public bool isInventoryActive = false;
    private bool isInteractingWithEquipmentPanel = false;
    private bool isSlotCreated = false;
    private EquipmentUI equipmentUI;
    void OnEnable()
    {
        equipmentUI = FindObjectOfType<EquipmentUI>();
        playerInventory = FindObjectOfType<PlayerInventoryHandler>();

        if (isSlotCreated == false)
        {
            for (int i = 0; i < categoryPanels.Count; i++)
            {
                List<InventorySlotTrash> slots = CreateSlots(categoryPanels[i], 25, i);
                categorySlots.Add(slots);
            }

            isSlotCreated = true;
        }

        UpdateCategory();
        UpdateInventoryUI();
        UpdateSelectionImage();

        UpdateItemDetails();
    }
    
    public override void Open() // 인벤토리 열고 닫을때를 Open, Close로 변경
    {
        UpdateCategory();
        UpdateSelectionImage();
        UpdateItemDetails();
    }

    public override void Close()
    {
        itemNameText.text = "";
        itemDescriptionText.text = "";

        // 액션 패널 비활성화
        itemActionPanel.HidePanel();
    }

    public override void OnInputWithDirection(Vector2 inputDirx)
    {
        if (itemActionPanel.isActionPanelActive) return;
        
        if (Mathf.Abs(inputDirx.y) > Mathf.Abs(inputDirx.x))
        {
            MoveSelection((int)Mathf.Sign(inputDirx.y) * inventoryWidth * -1);
            if (Mathf.Sign(inputDirx.y) > 0)
            {
                ScrollUp();
            }
            else
            {
                ScrollDown();
            }
        }
        else
        {
            MoveSelection((int)Mathf.Sign(inputDirx.x));
        }
    }

    public override void OnQuitInput()
    {
        if (itemActionPanel.isActionPanelActive)
        {
            itemActionPanel.HidePanel();
            return;
        }
        if (itemActionPanel.isActionPanelActive) return;
        PlayerHUDManager.Instance.CloseMenu(this);
    }

    public override void OnSelectInput()
    {
        if (itemActionPanel.isActionPanelActive)
        {
            itemActionPanel.StartCoroutine(itemActionPanel.HandleButtonClickAfterDelay());
            return;
        }
        if (equipmentUI.isInteractingWithEquipmentPanel)
        {
            equipmentUI.ClearSelectedSlot();
            return;
        }
        var selectedSlot = categorySlots[currentCategoryIndex][selectedSlotIndex];
        var item = selectedSlot.GetCurrentItem();
        if (item != null && item.itemCount > 0)
        {
            itemActionPanel.ShowPanel(item);
        }
    }

    public override void OnMoveTabInput(int inputDirx) {
        ChangeCategory(inputDirx);
    }

    void MaintainFocus()
    {
        if (isInventoryActive && EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(categorySlots[currentCategoryIndex][selectedSlotIndex].gameObject);
        }
    }

    void Update()
    {
        MaintainFocus();

        if (equipmentUI.isEquipmentPanelActive)
        {
            itemDescriptionGroup.SetActive(false);
        }
        else
        {
            itemDescriptionGroup.SetActive(true);

            UpdateItemDetails();
        }
    }


    
    // void ToggleInventory()
    // {
    //     isInventoryActive = !isInventoryActive;
    //     inventoryPanel.SetActive(isInventoryActive);
    //
    //     if (isInventoryActive)
    //     {
    //         UpdateCategory();
    //         UpdateSelectionImage();
    //         UpdateItemDetails();
    //     }
    //     else
    //     {
    //         itemNameText.text = "";
    //         itemDescriptionText.text = "";
    //
    //         // 액션 패널 비활성화
    //         itemActionPanel.HidePanel();
    //     }
    // }
    
    

    void ChangeCategory(int direction)
    {
        currentCategoryIndex = (currentCategoryIndex + direction + categoryPanels.Count) % categoryPanels.Count;

        UpdateCategory();

        scrollRect.content.anchoredPosition = new Vector2(scrollRect.content.anchoredPosition.y, 0);

        UpdateItemDetails();

        FocusSelectedSlot();
    }

    void UpdateCategory()
    {
        foreach (var panel in categoryPanels)
        {
            panel.gameObject.SetActive(false);
        }

        Transform currentPanel = categoryPanels[currentCategoryIndex];
        currentPanel.gameObject.SetActive(true);

        UpdateCategoryPolygon();

        scrollRect.content = currentPanel.GetComponent<RectTransform>();

        UpdateInventoryUI();
        UpdateSelectionImage();
        ScrollToSelectedSlot();
        scrollRect.normalizedPosition = new Vector2(scrollRect.normalizedPosition.y, 0);
    }

    void OnSlotSelected(InventorySlotTrash selectedSlotTrash)
    {
        int selectedIndex = categorySlots[currentCategoryIndex].IndexOf(selectedSlotTrash);
        if (selectedIndex >= 0)
        {
            itemActionPanel.ShowPanel(selectedSlotTrash.GetCurrentItem());
        }
    }

    void UpdateCategoryPolygon()
    {
        for (int i = 0; i < categoryPolygon.Count; i++)
        {
            categoryPolygon[i].gameObject.SetActive(i == currentCategoryIndex);
        }
    }

    void MoveSelection(int direction)
    {
        if (equipmentUI.isInteractingWithEquipmentPanel)
        {
            Debug.Log("무시");
            return;
        }
        
        int newSelectedIndex = selectedSlotIndex + direction;

        if (newSelectedIndex < 0 || newSelectedIndex >= categorySlots[currentCategoryIndex].Count)
        {
            return;
        }

        if (direction == 1 && (selectedSlotIndex + 1) % inventoryWidth == 0)
        {
            return;
        }
        else if (direction == -1 && selectedSlotIndex % inventoryWidth == 0)
        {
            return;
        }

        selectedSlotIndex = newSelectedIndex;

        UpdateSelectionImage();
        UpdateItemDetails();
    }

    void UpdateSelectionImage()
    {
        var slots = categorySlots[currentCategoryIndex];
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].SetSelected(i == selectedSlotIndex);
        }

        FocusSelectedSlot();
    }

    void UpdateItemDetails()
    {
        var selectedSlot = categorySlots[currentCategoryIndex][selectedSlotIndex];
        var item = selectedSlot.GetCurrentItem();
        
        if (item != null)
        {
            itemNameText.text = item.itemName;
            itemDescriptionText.text = item.itemDescription;
        }
        else
        {
            itemNameText.text = "아이템을 선택 하지 않았습니다";
            itemDescriptionText.text = "";
        }
    }

    void ScrollDown()
    {
        if (scrollRect.content.anchoredPosition.y >= 65 * GetCurrentCategorySlotRange()-1) 
            return;

        Vector2 newPosition = scrollRect.content.anchoredPosition;
        newPosition.y += 134;
        scrollRect.content.anchoredPosition = newPosition;
    }

    void ScrollUp()
    {
        if (scrollRect.content.anchoredPosition.y <= -65 * GetCurrentCategorySlotRange())
            return;

        Vector2 newPosition = scrollRect.content.anchoredPosition;
        newPosition.y -= 134;
        scrollRect.content.anchoredPosition = newPosition;
    }

    void ScrollToSelectedSlot()
    {
        if (scrollRect == null || categorySlots[currentCategoryIndex].Count == 0) return;

        RectTransform slotRect = categorySlots[currentCategoryIndex][selectedSlotIndex].GetComponent<RectTransform>();

        Vector2 viewportLocalPosition = (Vector2)scrollRect.viewport.InverseTransformPoint(slotRect.position);
        Vector2 contentLocalPosition = (Vector2)scrollRect.content.InverseTransformPoint(slotRect.position);

        float contentHeight = scrollRect.content.rect.height;
        float viewportHeight = scrollRect.viewport.rect.height;

        float targetY = contentLocalPosition.y - viewportLocalPosition.y;
        float clampedY = Mathf.Clamp(targetY, 0, contentHeight - viewportHeight);

        Vector2 newContentPosition = new Vector2(scrollRect.content.anchoredPosition.x, -clampedY);
        scrollRect.content.anchoredPosition = newContentPosition;
    }

    public List<InventorySlotTrash> CreateSlots(Transform panel, int slotCount, int categoryId)
    {
        List<InventorySlotTrash> slots = new List<InventorySlotTrash>();

        for (int i = 0; i < slotCount; i++)
        {
            InventorySlotTrash newSlotTrash = AddSlot(panel, categoryId);
            slots.Add(newSlotTrash);
        }

        return slots;
    }

    InventorySlotTrash AddSlot(Transform panel, int categoryId)
    {
        GameObject newSlotObject = Instantiate(slotPrefab, panel);
        InventorySlotTrash slotTrash = newSlotObject.GetComponent<InventorySlotTrash>();

        slotTrash.categoryId = categoryId;

        return slotTrash;
    }

    public void OnSlotClicked(InventorySlotTrash clickedSlotTrash)
    {
        int clickedIndex = categorySlots[currentCategoryIndex].IndexOf(clickedSlotTrash);
        if (clickedIndex >= 0)
        {
            SelectedSlotIndex = clickedIndex;
            UpdateSelectionImage();
            UpdateItemDetails();
        }
    }
    public int GetSlotIndex(InventorySlotTrash slotTrash)
    {
        return categorySlots[currentCategoryIndex].IndexOf(slotTrash);
    }
    public void UpdateInventoryUI()
    {
        Item[] playerItems = playerInventory.playerItemList;
        var slots = categorySlots[currentCategoryIndex];
        int slotIndex = 0;

        for (int i = 0; i < playerItems.Length; i++)
        {
            if (playerItems[i] != null && playerItems[i].itemCount > 0 && playerItems[i].categoryId == currentCategoryIndex)
            {
                if (slotIndex < slots.Count)
                {
                    slots[slotIndex].SetItem(playerItems[i], currentCategoryIndex);
                    slotIndex++;
                }
            }
        }

        for (int i = slotIndex; i < slots.Count; i++)
        {
            slots[i].ClearSlot();
        }
    }
    int GetCurrentCategorySlotRange()
    {
        int slotCount = categorySlots[currentCategoryIndex].Count;
        int rangeSize = 5;
        int baseCount = 25;

        if (slotCount < baseCount)
            return -1;

        return (slotCount - baseCount) / rangeSize;
    }
    void FocusSelectedSlot()
    {
        if (categorySlots[currentCategoryIndex].Count > selectedSlotIndex)
        {
            GameObject selectedSlotObject = categorySlots[currentCategoryIndex][selectedSlotIndex].gameObject;
            /*EventSystem.current.SetSelectedGameObject(selectedSlotObject);*/
        }
    }
    public void DisableSelectionImage()
    {
        var slots = categorySlots[currentCategoryIndex];
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].SetSelected(false);
        }
    }
    public void EnableSelectionImage(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < categorySlots[currentCategoryIndex].Count)
        {
            categorySlots[currentCategoryIndex][slotIndex].SetSelected(true);
        }
    }
    public int SelectedSlotIndex
    {
        get { return selectedSlotIndex; }
        set { selectedSlotIndex = value; }
    }

   
}
}