using System;
using MinD.Runtime.Entity;
using UnityEngine;

public class QuickSlotPresenter : MonoBehaviour
{
    [SerializeField] private QuickSlot quickSlotView;
    [SerializeField] private PlayerInventoryHandler inventoryHandler;

    private void Start()
    {
        if (quickSlotView == null)
            quickSlotView = FindObjectOfType<QuickSlot>();
        if (inventoryHandler == null)
            inventoryHandler = FindObjectOfType<PlayerInventoryHandler>();
        Initialize(quickSlotView, inventoryHandler);
    }

    public void Initialize(QuickSlot view, PlayerInventoryHandler handler)
    {
        this.quickSlotView = view;
        this.inventoryHandler = handler;
        
        quickSlotView.Initialize(inventoryHandler.magicSlots, inventoryHandler.toolSlots);
    }

    public void UpdateQuickSlots()
    {
        quickSlotView.UpdateMagicQuickSlot();
        quickSlotView.UpdateToolQuickSlot();
    }

    public void HandleInput(Vector2 input)
    {
        if (input.y > 0)
        {
            quickSlotView.HandleMagicSlotSwapping(-1); // 이전 마법
            UpdateQuickSlots();
        }
        else if (input.y < 0)
        {
            quickSlotView.HandleMagicSlotSwapping(1); // 다음 마법
            UpdateQuickSlots();
        }

        // 왼쪽, 오른쪽 방향 → 소모품 변경
        if (input.x < 0)
        {
            quickSlotView.HandleToolSlotSwapping(-1); // 이전 소모품
            UpdateQuickSlots();
        }
        else if (input.x > 0)
        {
            quickSlotView.HandleToolSlotSwapping(1); // 다음 소모품
            UpdateQuickSlots();
        }
    }
}