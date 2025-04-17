using System;
using System.Linq;
using MinD.Enums;
using MinD.Runtime.DataBase;
using MinD.Runtime.Managers;
using MinD.Runtime.UI;
using MinD.SO.Item;
using MinD.Structs;
using UnityEngine;

namespace MinD.Runtime.Entity {

public class PlayerInventoryHandler : EntityOwnedHandler {

	[Header("[ Equipment Slot ]")]
	public Weapon weaponSlot;
	public Protection protectionSlot;

	[Space(5)]
	public int allowedTalismanSlotCount;
	public Talisman[] talismanSlots = new Talisman[5];
	
	[Header("[ Tool Slot ]")]
	public int currentToolSlot;
	public Tool[] toolSlots = new Tool[10];
	

	[Header("[ Magic Slot ]")]
	public int currentMagicSlot;
	public Magic[] magicSlots = new Magic[1]; // CHANGE SLOT SIZE BY ATTRIBUTE IN RUNTIME
	
	private int usingMemory; // MEMORY AMOUNT OF CURRENT USING MAGICS


	[Header("[ Owned Item Array ]")]
	[SerializeField] public Item[] playerItemList;

	
	[Header("[ Debug ]")]
	public Magic equipMagic;

	[SerializeField] private QuickSlotPresenter quickSlotPresenter;

	private void Awake()
	{
		if (quickSlotPresenter == null)
			quickSlotPresenter = FindObjectOfType<QuickSlotPresenter>();	
	}


	public void OnValidate() {
		if (equipMagic != null) {
			EquipMagic(equipMagic, 0);
			equipMagic = null;
		}
	}



	public void HandleInventoryOpen()
	{
		if (PlayerInputManager.Instance.openInventoryInput ) {
			if (PlayerHUDManager.Instance.currentShowingMenu == null) {
				PlayerHUDManager.Instance.OpenMenu(PlayerHUDManager.playerHUD.inventoryMenu);
			} else if (PlayerHUDManager.Instance.currentShowingMenu == PlayerHUDManager.playerHUD.inventoryMenu) {
				// If current inventory menu is opened, close inventory
				PlayerHUDManager.Instance.CloseMenu(PlayerHUDManager.playerHUD.inventoryMenu);
			}

			PlayerInputManager.Instance.openInventoryInput = false;
		}
	}

	public void LoadItemData(PlayerInventoryData inventoryData) { // TODO: Need getting item data by parameter

		// SET DATA LIST LENGTH
		weaponSlot = inventoryData.weapon;
		protectionSlot = inventoryData.protection;
		talismanSlots = inventoryData.talismans ?? new Talisman[5];
		toolSlots = inventoryData.tools ?? new Tool[10];
		magicSlots = inventoryData.magics ?? magicSlots;
		playerItemList = inventoryData.allItems ?? new Item[ItemDataBase.Instance.GetAllItemsCount()];

//		FindObjectOfType<QuickSlotUIManager>().UpdateToolQuickSlot(currentToolSlot);
	}


	private Item CreateItem(int itemId) {

		Item newItem = Instantiate(ItemDataBase.Instance.GetItemSo(itemId));
		playerItemList[itemId] = newItem;

		return newItem;
	}

	public bool AddItem(int itemId, int amount = 1, bool deleteExceededItem = false) {

		if (amount < 0)
			amount = 0; // MIN(0) CLAMP

		// USE ID AS INDEX TO FIND THE TARGET ITEM INSTANCE IN LIST
		// CAUSE ID IS GENERATE BASED ON INDEX OF ITEM SO LIST
		Item itemInList = playerItemList[itemId];

		// IF ITEM INSTANCE IS NOT CREATED
		if (itemInList == null)
			itemInList = CreateItem(itemId);


		// IF ITEM COUNT WILL EXCEEDS MAX COUNT
		if (itemInList.itemCount + amount > itemInList.itemMaxCount) {

			if (deleteExceededItem) {

				itemInList.itemCount = itemInList.itemMaxCount;
				return true;
			} else
				return false;
		}
		
		#region UI_PROCESS
		InteractionPanelController panelController = FindObjectOfType<InteractionPanelController>();
		if (panelController != null)
		{
			panelController.ShowLootingPanel(itemInList);
		}
			
		// TODO: Temp. it just for make prototype successfully
		if (itemInList.categoryId == 3)
		{
			Player.player.inventory.magicSlots[Player.player.inventory.magicSlots.Count(i => i != null)] = itemInList as Magic;
			FindObjectOfType<MagicQuickSlot>().UpdateUI();
		}
		#endregion


		// WORKING NORMALLY
		itemInList.itemCount += amount;
		return true;
	}

	public bool ReduceItem(int itemId, int amount = 1) {

		if (amount < 0)
			amount = 0;

		// USE ID AS INDEX TO FIND THE TARGET ITEM INSTANCE IN LIST
		// CAUSE ID IS GENERATE BASED ON INDEX OF ITEM SO LIST
		Item itemInList = playerItemList[itemId];

		// IF ITEM INSTANCE IS NOT CREATED
		if (itemInList == null)
			return false;

		// CHECK IF THERE ARE ENOUGH ITEM TO REMOVE
		if (itemInList.itemCount < amount)
			return false;


		itemInList.itemCount -= amount;
		return true;
	}



	public void EquipEquipment(Equipment equipment, EquipmentSlots targetSlot) {

		// INSERT EQUIPMENT IN SLOT
		switch (targetSlot) {

			case EquipmentSlots.Weapon:
				weaponSlot = equipment as Weapon;
				break;

			case EquipmentSlots.Protection:
				protectionSlot = equipment as Protection;
				break;

			#region Talismans

			case EquipmentSlots.Talisman_01:
				talismanSlots[0] = equipment as Talisman;
				break;

			case EquipmentSlots.Talisman_02:
				talismanSlots[1] = equipment as Talisman;
				break;

			case EquipmentSlots.Talisman_03:
				talismanSlots[2] = equipment as Talisman;
				break;

			case EquipmentSlots.Talisman_04:
				talismanSlots[3] = equipment as Talisman;
				break;

			case EquipmentSlots.Talisman_05:
				talismanSlots[4] = equipment as Talisman;
				break;

			#endregion

			#region Tools

			case EquipmentSlots.Tool_01:
				toolSlots[0] = equipment as Tool;
				break;

			case EquipmentSlots.Tool_02:
				toolSlots[1] = equipment as Tool;
				break;

			case EquipmentSlots.Tool_03:
				toolSlots[2] = equipment as Tool;
				break;

			case EquipmentSlots.Tool_04:
				toolSlots[3] = equipment as Tool;
				break;

			case EquipmentSlots.Tool_05:
				toolSlots[4] = equipment as Tool;
				break;

			case EquipmentSlots.Tool_06:
				toolSlots[5] = equipment as Tool;
				break;

			case EquipmentSlots.Tool_07:
				toolSlots[6] = equipment as Tool;
				break;

			case EquipmentSlots.Tool_08:
				toolSlots[7] = equipment as Tool;
				break;

			case EquipmentSlots.Tool_09:
				toolSlots[8] = equipment as Tool;
				break;

			case EquipmentSlots.Tool_10:
				toolSlots[9] = equipment as Tool;
				break;

			#endregion
		}

		ReduceItem(equipment.itemId);
		equipment.OnEquip(((Player)owner));

		if (equipment is Weapon weapon)
			((Player)owner).equipment.ChangeWeapon(weapon);

	}

	public void UnequipEquipment(EquipmentSlots targetSlot) {

		Equipment unequipedItem = null;

		// SET unequipedItem AND NULL TARGET SLOT
		switch (targetSlot) {

			case EquipmentSlots.Weapon:
				unequipedItem = weaponSlot;

				if (weaponSlot != null) {

					if (unequipedItem is Weapon weapon)
						((Player)owner).equipment.ChangeWeapon(null);

					weaponSlot = null;
				}

				weaponSlot = null;
				break;

			case EquipmentSlots.Protection:
				unequipedItem = protectionSlot;
				protectionSlot = null;
				break;

			#region Talismans

			case EquipmentSlots.Talisman_01:
				unequipedItem = talismanSlots[0];
				protectionSlot = null;
				break;

			case EquipmentSlots.Talisman_02:
				unequipedItem = talismanSlots[1];
				talismanSlots[0] = null;
				break;

			case EquipmentSlots.Talisman_03:
				unequipedItem = talismanSlots[2];
				talismanSlots[2] = null;
				break;

			case EquipmentSlots.Talisman_04:
				unequipedItem = talismanSlots[3];
				talismanSlots[3] = null;
				break;

			case EquipmentSlots.Talisman_05:
				unequipedItem = talismanSlots[4];
				talismanSlots[4] = null;
				break;

			#endregion

			#region Tools

			case EquipmentSlots.Tool_01:
				unequipedItem = toolSlots[0];
				toolSlots[0] = null;
				break;

			case EquipmentSlots.Tool_02:
				unequipedItem = toolSlots[1];
				toolSlots[1] = null;
				break;

			case EquipmentSlots.Tool_03:
				unequipedItem = toolSlots[2];
				toolSlots[2] = null;
				break;

			case EquipmentSlots.Tool_04:
				unequipedItem = toolSlots[3];
				toolSlots[3] = null;
				break;

			case EquipmentSlots.Tool_05:
				unequipedItem = toolSlots[4];
				toolSlots[4] = null;
				break;

			case EquipmentSlots.Tool_06:
				unequipedItem = toolSlots[5];
				toolSlots[5] = null;
				break;

			case EquipmentSlots.Tool_07:
				unequipedItem = toolSlots[6];
				toolSlots[6] = null;
				break;

			case EquipmentSlots.Tool_08:
				unequipedItem = toolSlots[7];
				toolSlots[7] = null;
				break;

			case EquipmentSlots.Tool_09:
				unequipedItem = toolSlots[8];
				toolSlots[8] = null;
				break;

			case EquipmentSlots.Tool_10:
				unequipedItem = toolSlots[9];
				toolSlots[9] = null;
				break;

			#endregion
		}

		AddItem(unequipedItem.itemId);
		unequipedItem.OnUnequip(((Player)owner));
	}



	public bool EquipMagic(Magic magic, int slotPos) {

		// CANCEL IF EXCEED THE MEMORY CAPACITY
		if (magic.memoryCost + usingMemory > ((Player)owner).attribute.memoryCapacity) {
			return false;
		}

		// CANCEL IF slotPos PARAMETER IS OVER THE LIST SIZE
		if (slotPos < 0 || slotPos >= magicSlots.Length) {
			return false;
		}


		usingMemory += magic.memoryCost;
		magicSlots[slotPos] = magic;

		return true;
	}

	public void UnequipMagic(int slotPos) {

		// CANCEL IF slotPos PARAMETER IS OVER THE LIST SIZE
		if (slotPos < 0 || slotPos >= magicSlots.Length) {
			return;
		}

		usingMemory -= magicSlots[slotPos].memoryCost;
		magicSlots[slotPos] = null;

	}



	public void HandleQuickSlotSwapping() {
		HandleMagicSlotSwapping();
		HandleToolSlotSwapping();
	}

	private void HandleMagicSlotSwapping() {

		// CHECK INPUT FLAG
		if (PlayerInputManager.Instance.swapMagicInput == 0) {
			return;
		}


		// CANCEL SWAP IF PLAYER HASN'T ANY MAGIC
		if (magicSlots.Count(i => i != null) == 0)
			return;


		if (PlayerInputManager.Instance.swapMagicInput == 1) {
			while (true) {
				// TO SKIP EMPTY MAGIC
				// REMAINDER OPERATING TO CYCLE THE LIST
				currentMagicSlot = (currentMagicSlot + 1) % magicSlots.Length;
				// 
				if (magicSlots[currentMagicSlot] != null) {
					break;
				}
			}
			quickSlotPresenter.HandleInput(Vector2.down);
		} else if (PlayerInputManager.Instance.swapMagicInput == -1) {
			while (true) {
				currentMagicSlot = (currentMagicSlot - 1 + magicSlots.Length) % magicSlots.Length;
				if (magicSlots[currentMagicSlot] != null) {
					break;
				}
			}
			quickSlotPresenter.HandleInput(Vector2.up);
		}
		PlayerInputManager.Instance.swapMagicInput = 0;
	}

	private void HandleToolSlotSwapping() {

		if (PlayerInputManager.Instance.swapToolInput == 0) {
			return;
		}

		
		// CANCEL SWAP IF PLAYER HASN'T ANY TOOL
		if (toolSlots.Count(i => i != null) == 0) {
			return;
		}
		
		
		if (PlayerInputManager.Instance.swapToolInput == 1) {
			while (true) {
				// TO SKIP EMPTY TOOL SLOT
				// REMAINDER OPERATING TO CYCLE THE LIST
				currentToolSlot = (currentToolSlot + 1) % toolSlots.Length;
				if (toolSlots[currentToolSlot] != null) {
					break;
				}
			}
			quickSlotPresenter.HandleInput(Vector2.right);
			
		} else if (PlayerInputManager.Instance.swapToolInput == -1) {
			while (true) {
				currentToolSlot = (currentToolSlot - 1 + toolSlots.Length) % toolSlots.Length;
				if (toolSlots[currentToolSlot] != null) {
					break;
				}
				
			}
			quickSlotPresenter.HandleInput(Vector2.left);
		}
		
		
		PlayerInputManager.Instance.swapToolInput = 0;
	}

	public void UpdateQuickSlots()
	{
		quickSlotPresenter.UpdateQuickSlots();
	}
	public void SortEquippedToolSlots()
	{
		var equippedTools = toolSlots
			.Where(tool => tool != null && tool.isEquipped)
			.ToList();
	
		for (int i = 0; i < toolSlots.Length; i++)
		{
			toolSlots[i] = i < equippedTools.Count ? equippedTools[i] : null;
		}
	}
	
	public void SortEquippedMagicSlots()
	{
		var equippedMagics = magicSlots
			.Where(magic => magic != null && magic.isEquipped)
			.ToList();
	
		for (int i = 0; i < magicSlots.Length; i++)
		{
			magicSlots[i] = i < equippedMagics.Count ? equippedMagics[i] : null;
		}
	}

}

}