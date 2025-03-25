using System.Collections;
using System.Linq;
using MinD.Enums;
using MinD.Runtime.Entity;
using MinD.SO.Item;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace MinD.Runtime.UI
{
    public class ItemActionPanel : MonoBehaviour
    {
        public GameObject panel;
        public Button wearButton;
        public Button dropButton;
        public Button destroyButton;

        private Item currentItem;
        private PlayerInventoryHandler playerInventoryHandler;
        private InventoryMenu _inventoryMenu;
        private QuickSlotUIManager QuickSlotUIManager;

        public Button[] actionButtons;
        public int selectedButtonIndex = 0;

        public bool isActionPanelActive = false;

        private int equippedTalismanCount => playerInventoryHandler.talismanSlots.Count(t => t != null);

        private const int columns = 5; // 인벤토리의 열 개수
        private const float baseX = -20f; // 기본 x 위치
        private const float baseY = 100f; // 기본 y 위치
        private const float offsetX = 120f; // x 증가량
        private const float offsetY = -120f; // y 감소량
        
        void Start()
        {
            panel.SetActive(false);
            actionButtons = new Button[] { wearButton, dropButton, destroyButton };

            wearButton.onClick.AddListener(OnEquipButtonClicked);
            dropButton.onClick.AddListener(OnDropButtonClicked);
            destroyButton.onClick.AddListener(OnDestroyButtonClicked);

            playerInventoryHandler = FindObjectOfType<PlayerInventoryHandler>();
            _inventoryMenu = FindObjectOfType<InventoryMenu>();
            QuickSlotUIManager = FindObjectOfType<QuickSlotUIManager>();

            if (playerInventoryHandler == null)
                Debug.LogError("PlayerInventoryHandler not found in the scene!");

            if (_inventoryMenu == null)
                Debug.LogError("InventoryUI not found in the scene!");
        }

        void Update()
        {
            HandleInput();
        }

        private void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                HidePanel();
                return;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                ChangeSelection(-1);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                ChangeSelection(1);
            }

            // if (Input.GetKeyDown(KeyCode.E))
            // {
            //     Debug.Log(isActionPanelActive);
            //     StartCoroutine(HandleButtonClickAfterDelay());
            // }
        }

        public IEnumerator HandleButtonClickAfterDelay()
        {
            yield return null;
            
            if (selectedButtonIndex >= 0 && selectedButtonIndex < actionButtons.Length)
            {
                actionButtons[selectedButtonIndex].onClick.Invoke();
            }
        }

        private void ChangeSelection(int direction)
        {
            selectedButtonIndex += direction;
            if (selectedButtonIndex < 0)
            {
                selectedButtonIndex = actionButtons.Length - 1;
            }
            else if (selectedButtonIndex >= actionButtons.Length)
            {
                selectedButtonIndex = 0;
            }

            UpdateButtonSelection();
        }

        private void UpdateButtonSelection()
        {
            for (int i = 0; i < actionButtons.Length; i++)
            {
                ActionSlot actionSlot = actionButtons[i].GetComponent<ActionSlot>();
                if (actionSlot != null)
                {
                    actionSlot.SetSelected(i == selectedButtonIndex);
                }
            }
        }

        public void ShowPanel(Item item)
        {
            currentItem = item;
            UpdatePanelPosition(_inventoryMenu.SelectedSlotIndex);
            panel.SetActive(true);
            isActionPanelActive = true;
            UpdateButtonSelection();
        }

        public void HidePanel()
        {
            panel.SetActive(false);
            isActionPanelActive = false;
        }

        private void OnEquipButtonClicked()
        {
            if (currentItem is Equipment equipment)
            {
                EquipItemBasedOnCategory(equipment);
            }
            HidePanel();
            _inventoryMenu.UpdateInventoryUI();
            QuickSlotUIManager.InitializeQuickSlots();
        }

        private void EquipItemBasedOnCategory(Equipment equipment)
        {
            switch (equipment.categoryId)
            {
                case 0:
                    EquipTalisman(equipment);
                    break;
                case 1:
                    EquipTool(equipment);
                    break;
                case 2:
                    EquipProtection(equipment);
                    break;
                case 3:
                    EquipWeapon(equipment);
                    break;
                default:
                    Debug.LogWarning("Unknown equipment category!");
                    break;
            }
        }

        private void EquipTalisman(Equipment equipment)
        {
            if (playerInventoryHandler.talismanSlots.Contains(equipment))
            {
                Debug.LogWarning("이 탈리스만은 이미 장창되어 있습니다.");
                return;
            }
            for (int i = 0; i < 5; i++)
            {
                if (playerInventoryHandler.talismanSlots[i] == null)
                {
                    playerInventoryHandler.talismanSlots[i] = (Talisman)equipment;
                    Debug.Log($"착용: {equipment.itemName} (탈리스만 {i + 1})");

                    var talismanSlot = FindObjectsOfType<EquipmentSlot>().FirstOrDefault(slot => slot.categoryId == 0 && slot.slotIndex == i);
                    talismanSlot?.UpdateSlot(equipment);
                    break;
                }
            }
        }

        private void EquipTool(Equipment equipment)
        {
            if (playerInventoryHandler.toolSlots.Contains(equipment))
            {
                Debug.LogWarning("이 도구는 이미 장착되어 있습니다.");
                return;
            }
            for (int i = 0; i < 10; i++)
            {
                if (playerInventoryHandler.toolSlots[i] == null)
                {
                    playerInventoryHandler.toolSlots[i] = (Tool)equipment;
                    Debug.Log($"착용: {equipment.itemName} (도구 {i + 1})");

                    var toolSlot = FindObjectsOfType<EquipmentSlot>().FirstOrDefault(slot => slot.categoryId == 1 && slot.slotIndex == i);
                    toolSlot?.UpdateSlot(equipment);
                    break;
                }
            }
        }

        private void EquipProtection(Equipment equipment)
        {
            if (playerInventoryHandler.protectionSlot == null)
            {
                playerInventoryHandler.protectionSlot = (Protection)equipment;
                Debug.Log($"착용: {equipment.itemName} (방어구)");

                var protectionSlot = FindObjectsOfType<EquipmentSlot>().FirstOrDefault(slot => slot.categoryId == 2);
                protectionSlot?.UpdateSlot(equipment);
            }
            else
            {
                Debug.LogWarning("방어구 슬롯이 이미 가득 차 있습니다.");
            }
        }

        private void EquipWeapon(Equipment equipment)
        {
            if (playerInventoryHandler.weaponSlot == null)
            {
                playerInventoryHandler.weaponSlot = (Weapon)equipment;
                Debug.Log($"착용: {equipment.itemName} (무기)");

                var weaponSlot = FindObjectsOfType<EquipmentSlot>().FirstOrDefault(slot => slot.categoryId == 3);
                weaponSlot?.UpdateSlot(equipment);
            }
            else
            {
                Debug.LogWarning("무기 슬롯이 이미 가득 찼습니다.");
            }
        }

        private void OnDropButtonClicked()
        {
            if (currentItem != null)
            {
                currentItem.itemCount--;
                Debug.Log($"Dropped: {currentItem.itemName}");
                UpdateEquipmentSlotCount();

                if (currentItem.itemCount <= 0)
                {
                    currentItem = null;
                    HidePanel();
                }
            }

            _inventoryMenu.UpdateInventoryUI();
        }

        private void UpdateEquipmentSlotCount()
        {
            var equipmentSlots = FindObjectsOfType<EquipmentSlot>();
            foreach (var slot in equipmentSlots)
            {
                if (slot.categoryId == currentItem.categoryId)
                {
                    slot.UpdateSlot(currentItem);
                    break;
                }
            }
        }

        private void OnDestroyButtonClicked()
        {
            if (currentItem != null)
            {
                currentItem.itemCount--;
                Debug.Log($"Destroyed: {currentItem.itemName}");
                UpdateEquipmentSlotCount();

                if (currentItem.itemCount <= 0)
                {
                    currentItem = null;
                    HidePanel();
                }
            }

            _inventoryMenu.UpdateInventoryUI();
        }

        public bool IsActive()
        {
            return panel.activeSelf;
        }
        public void UpdatePanelPosition(int index)
        {
            // 열(row)과 행(column) 계산
            int row = index / columns;
            int column = index % columns;

            // 새로운 위치 계산
            float newX = baseX + (column * offsetX);
            float newY = baseY + (row * offsetY);

            // 패널의 위치 업데이트
            panel.transform.localPosition = new Vector3(newX, newY, panel.transform.localPosition.z);
        }
    }
}
