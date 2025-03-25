using System.Collections.Generic;
using MinD.Runtime.Entity;
using MinD.SO.Item;
using UnityEngine;

namespace MinD.Runtime.UI
{
    public class QuickSlotUIManager : MonoBehaviour
    {
        public MagicQuickSlot magicQuickSlotUI;
        public ToolQuickSlot toolQuickSlotUI;

        private PlayerInventoryHandler inventoryHandler;

        private void Start()
        {
            inventoryHandler = FindObjectOfType<PlayerInventoryHandler>();
            InitializeQuickSlots();
        }

        public void InitializeQuickSlots()
        {
            if (inventoryHandler == null)
            {
                Debug.LogError("PlayerInventoryHandler not found!");
                return;
            }

            // MagicQuickSlot 초기화
            magicQuickSlotUI.Initialize(new List<Magic>(inventoryHandler.magicSlots), inventoryHandler.currentMagicSlot);

            // ToolQuickSlot 초기화
            toolQuickSlotUI.Initialize(new List<Tool>(inventoryHandler.toolSlots), inventoryHandler.currentToolSlot);
        }

        public void UpdateMagicQuickSlot(int newMagicIndex)
        {
            magicQuickSlotUI.SetCurrentMagicSlot(newMagicIndex);
        }

        public void UpdateToolQuickSlot(int newToolIndex)
        {
            toolQuickSlotUI.SetCurrentToolSlot(newToolIndex);
        }
    }
}