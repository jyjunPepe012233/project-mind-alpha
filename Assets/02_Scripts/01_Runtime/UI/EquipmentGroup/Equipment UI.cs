using System.Collections.Generic;
using MinD.Enums;
using MinD.Runtime.Entity;
using MinD.Runtime.UI;
using UnityEngine;

public class EquipmentUI : MonoBehaviour
{
    public List<Transform> EquipmentPanels; // 각 카테고리별 패널 (Talisman, Tool, Protection, Weapon)
    public GameObject EquipmentSlotPrefab;

    private InventoryMenu _inventoryMenu;
    private PlayerInventoryHandler playerInventory;
    private int currentPanelIndex = -1; // (-1: 인벤토리 모드)
    private int currentSlotIndex = 0;
    private int columns = 5;
    
    public bool isInteractingWithEquipmentPanel = false;
    public bool isEquipmentPanelActive = true;

    void Start()
    {
        _inventoryMenu = FindObjectOfType<InventoryMenu>();
        playerInventory = FindObjectOfType<PlayerInventoryHandler>();
        CreateEquipmentSlots();
        UpdateSelectedSlot(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleAllEquipmentPanels();
        }

        
        if (isEquipmentPanelActive && Input.GetKeyDown(KeyCode.C))
        {
            ToggleEquipmentPanel();
        }
        
        if (isInteractingWithEquipmentPanel)
        {
            
            
            HandleSlotSelection();

            if (Input.GetKeyDown(KeyCode.Return))
            {
                TriggerSlotAction();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                ClearSelectedSlot();
            }
        }
    }
    
    private void ToggleAllEquipmentPanels()
    {
        if (isInteractingWithEquipmentPanel)
        {
            return;
        }

        isEquipmentPanelActive = !isEquipmentPanelActive;

        foreach (Transform panel in EquipmentPanels)
        {
            panel.gameObject.SetActive(isEquipmentPanelActive);
        }

        if (!isEquipmentPanelActive)
        {
            isInteractingWithEquipmentPanel = false;
            currentPanelIndex = -1;
            
            Debug.Log($"Equipment Panels deactivated. Returning to inventory selection at index {currentSlotIndex}.");
        }
        else
        {
            if (currentPanelIndex == -1)
            {
                currentPanelIndex = -1;
            }

            ResetSlotIndex();
            UpdateSelectedSlot(false);
            Debug.Log("Equipment Panels activated.");
        }
    }
    private void ToggleEquipmentPanel()
    {
        if (currentPanelIndex >= 0 && currentPanelIndex < EquipmentPanels.Count)
        {
            UpdateSelectedSlot(false);
        }

        if (currentPanelIndex == -1)
        {
            currentPanelIndex = 0;
        }
        else
        {
            currentPanelIndex++;
        }

        if (currentPanelIndex > EquipmentPanels.Count - 1)
        {
            currentPanelIndex = -1;
        }

        isInteractingWithEquipmentPanel = (currentPanelIndex != -1);

        if (isInteractingWithEquipmentPanel)
        {
            ResetSlotIndex();
            UpdateSelectedSlot(true);
            _inventoryMenu.DisableSelectionImage();
        }
        else
        {
            _inventoryMenu.EnableSelectionImage(_inventoryMenu.SelectedSlotIndex);
        }
    }



    private void HandleSlotSelection()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            MoveSelection(-1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            MoveSelection(1);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            MoveSelection(-columns);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            MoveSelection(columns);
        }
    }

    private void MoveSelection(int direction)
    {
        if (currentPanelIndex < 0 || currentPanelIndex >= EquipmentPanels.Count) return;

        Transform currentPanel = EquipmentPanels[currentPanelIndex];
        int slotCount = currentPanel.childCount;

        UpdateSelectedSlot(false);
        currentSlotIndex += direction;

        if (currentSlotIndex % columns < 0) currentSlotIndex += columns;
        else if (currentSlotIndex % columns >= columns) currentSlotIndex -= columns;

        if (currentSlotIndex < 0) currentSlotIndex = slotCount - 1;
        else if (currentSlotIndex >= slotCount) currentSlotIndex = 0;

        UpdateSelectedSlot(true);
    }

    public void CreateEquipmentSlots()
    {
        int[] slotCounts = { 1, 1, 5, 10 };
        int[] categoryIds = { 2, 3, 0, 1 };

        for (int i = 0; i < EquipmentPanels.Count; i++)
        {
            Transform panel = EquipmentPanels[i];
            for (int j = 0; j < slotCounts[i]; j++)
            {
                GameObject slotObj = Instantiate(EquipmentSlotPrefab, panel);
                EquipmentSlot slot = slotObj.GetComponent<EquipmentSlot>();
                if (slot != null)
                {
                    slot.categoryId = categoryIds[i];
                    slot.slotIndex = j;
                }
            }
        }
    }

    private void ResetSlotIndex()
    {
        if (currentPanelIndex >= 0 && currentPanelIndex < EquipmentPanels.Count)
        {
            int slotCount = EquipmentPanels[currentPanelIndex].childCount;
            currentSlotIndex = Mathf.Clamp(currentSlotIndex, 0, slotCount - 1);
        }
        else
        {
            currentSlotIndex = 0;
        }
    }

    private void UpdateSelectedSlot(bool activate = true)
    {
        if (currentPanelIndex < 0 || currentPanelIndex >= EquipmentPanels.Count) return;

        Transform currentPanel = EquipmentPanels[currentPanelIndex];
        int slotCount = currentPanel.childCount;

        currentSlotIndex = Mathf.Clamp(currentSlotIndex, 0, slotCount - 1);

        Transform selectedSlot = currentPanel.GetChild(currentSlotIndex);
        EquipmentSlot equipmentSlot = selectedSlot.GetComponent<EquipmentSlot>();

        if (equipmentSlot != null)
        {
            equipmentSlot.selectionImage.SetActive(activate);
        }
    }

    public void ClearSelectedSlot()
    {
        if (currentPanelIndex < 0 || currentPanelIndex >= EquipmentPanels.Count) return;

        Transform currentPanel = EquipmentPanels[currentPanelIndex];
        Transform selectedSlot = currentPanel.GetChild(currentSlotIndex);
        EquipmentSlot equipmentSlot = selectedSlot.GetComponent<EquipmentSlot>();

        if (equipmentSlot != null)
        {
            equipmentSlot.ClearSlot();
            playerInventory.UnequipEquipment((EquipmentSlots)equipmentSlot.slotIndex);
            Debug.Log($"Slot cleared: Category {equipmentSlot.categoryId}, Index {equipmentSlot.slotIndex}");
        }
    }

    private void TriggerSlotAction()
    {
        if (currentPanelIndex < 0 || currentPanelIndex >= EquipmentPanels.Count) return;

        Transform currentPanel = EquipmentPanels[currentPanelIndex];
        Transform selectedSlot = currentPanel.GetChild(currentSlotIndex);
        EquipmentSlot equipmentSlot = selectedSlot.GetComponent<EquipmentSlot>();

        if (equipmentSlot != null)
        {
            Debug.Log($"Slot action triggered: Category {equipmentSlot.categoryId}, Index {equipmentSlot.slotIndex}");
        }
    }
}
