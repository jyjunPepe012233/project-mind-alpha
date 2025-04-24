using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MinD.Runtime.Entity;
using MinD.Runtime.Object.Interactables;
using MinD.Structs;
using MinD.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MinD.Runtime.Managers {

public class WorldDataManager : Singleton<WorldDataManager> {
	
	private Dictionary<int, GuffinsAnchor> _worldAnchors = new();
	private Dictionary<int, bool> _isAnchorsDiscovered = new(); // TODO: his is temp. Need change to referencing the save data
	public int latestUsedAnchorId;

	private Dictionary<int, DroppedItem> _worldPlacedItems = new();
	private Dictionary<int, bool> _isPlacedItemsCollected = new();

	private Dictionary<int, BossRoomEntrance> _worldEntrances = new();
	private Dictionary<int, bool> _isBossesFelled = new();

	
	private PlayerInventoryData _playerInventoryData;
	


	private AsyncOperation _currentReloadSceneAsync;
	public AsyncOperation currentReloadSceneAsync => _currentReloadSceneAsync;


	protected override void OnSceneChanged(Scene oldScene, Scene newScene) {
		if (WorldUtility.IsWorldScene(newScene)) {
			FindGuffinsAnchorOnWorld();
			FindPlacedItemOnWorld();
		}
	}
	private void FindGuffinsAnchorOnWorld() {
		GuffinsAnchor[] _searchedAnchors = FindObjectsOfType<GuffinsAnchor>();
		// Find anchors on world by key(anchor information id)

		for (int i = 0; i < _searchedAnchors.Length; i++) {
			if (!_searchedAnchors[i].hasBeenIndexed) {
				throw new UnityException("Hasn't been indexed Guffin's Anchor is exist!!");
			}
			_worldAnchors[_searchedAnchors[i].worldIndex] = _searchedAnchors[i];
		}
	}
	private void FindPlacedItemOnWorld() {
		DroppedItem[] _searchedItem = FindObjectsOfType<DroppedItem>();
		// Find anchors on world by key(anchor information id)

		for (int i = 0; i < _searchedItem.Length; i++) {
			if (!_searchedItem[i].hasBeenIndexed) {
				throw new UnityException("Hasn't been indexed Guffin's Anchor is exist!!");
			}
			_worldPlacedItems[_searchedItem[i].worldIndex] = _searchedItem[i];
		}
	}
	private void FindBossRoomEntranceOnWorld() {
		BossRoomEntrance[] _searchedEntrance = FindObjectsOfType<BossRoomEntrance>();
		// Find anchors on world by key(anchor information id)

		for (int i = 0; i < _searchedEntrance.Length; i++) {
			if (!_searchedEntrance[i].hasBeenIndexed) {
				throw new UnityException("Hasn't been indexed!");
			}
			_worldEntrances[_searchedEntrance[i].worldIndex] = _searchedEntrance[i];
		}
	}
	
	
	public AsyncOperation LoadWorldScene() {

		if (_currentReloadSceneAsync == null) {
			_currentReloadSceneAsync = SceneManager.LoadSceneAsync(WorldUtility.SCENENAME_dungeon);
			StartCoroutine(ProcessLoadWorldSceneAsync());
			
		} else {
			Debug.LogWarning("Try reload scene during reloading");
		}
		
		return _currentReloadSceneAsync;
		// Reloading scene is completed, Then game data will load as OnSceneChanged method(below) that call by game manager
	}
	private IEnumerator ProcessLoadWorldSceneAsync() {
		
		while (!_currentReloadSceneAsync.isDone) {
			yield return null;
		}
		_currentReloadSceneAsync = null;
	}
	

	public void LoadGameData() {
		
		if (!WorldUtility.IsThisWorldScene()) {
			throw new UnityException("This is not world scene. Can't save data");
		}
		
		LoadGuffinsAnchorData();
		LoadPlacedItemData();
		// TODO: Load world object data
		LoadBossData();
		Player.player.LoadData();
		Player.player.inventory.LoadItemData(_playerInventoryData);
	}
	private void LoadGuffinsAnchorData() {
		for (int i = 0; i < _worldAnchors.Count; i++) {
			// TODO: This code is temp. Assign isDiscovered dictionary references save data
			if (!_isAnchorsDiscovered.ContainsKey(i)) {
				_isAnchorsDiscovered[i] = false;
			}
			_worldAnchors[i].LoadData(_isAnchorsDiscovered[i]);
		}
	}
	private void LoadPlacedItemData() {
		for (int i = 0; i < _worldPlacedItems.Count; i++) {
			// TODO: This code is temp. should references save data
			if (!_isPlacedItemsCollected.ContainsKey(i)) {
				_isPlacedItemsCollected[i] = false;
			}
			_worldPlacedItems[i].LoadDataAsPlacedItem(_isPlacedItemsCollected[i]);
		}
	}

	private void LoadBossData()
	{
		for (int i = 0; i < _worldEntrances.Count; i++) {
			// TODO: This code is temp. should references save data
			if (!_isPlacedItemsCollected.ContainsKey(i)) {
				_isPlacedItemsCollected[i] = false;
			}
			_worldEntrances[i].LoadBossData(_isBossesFelled[i]);
		}
	}
	

	public void SaveGameData() {
		
		if (!WorldUtility.IsThisWorldScene()) {
			throw new UnityException("This is not world scene. Can't save data");
		}
	
		SaveGuffinsAnchorData();
		SavePlacedItemData(); // TODO: Save with item dropped by enemy
		// TODO: SAVE WORLD OBJECT DATA
		SaveBossData();
		// TODO: SAVE PLAYER DATA
		SavePlayerData();
	}
	private void SaveGuffinsAnchorData() { // TODO: Temp. SHOULD BE BASED ON WORLD BAKE DATA. COULDN'T SAVE DATA AT '_isAnchorDiscovered(CAUSE IT IS TEMP VARIABLE)'
		
		for (int i = 0; i < _worldAnchors.Count; i++) {
			_isAnchorsDiscovered[i] = _worldAnchors[i].isDiscovered;
		}
	}
	private void SavePlacedItemData() {
		
		for (int i = 0; i < _worldPlacedItems.Count; i++) {
			_isPlacedItemsCollected[i] = _worldPlacedItems[i] == null;
		}
	}
	private void SaveBossData()
	{
		for (int i = 0; i < _worldEntrances.Count; i++)
		{
			_isBossesFelled[i] = _worldEntrances[i].isFelled;
		}
	}
	
	private void SavePlayerData() {

		PlayerInventoryHandler inventory = Player.player.inventory; 
		
		_playerInventoryData.weapon = inventory.weaponSlot;
		_playerInventoryData.protection = inventory.protectionSlot;
		_playerInventoryData.talismans = inventory.talismanSlots;
		_playerInventoryData.tools = inventory.toolSlots;
		_playerInventoryData.magics = inventory.magicSlots;
		_playerInventoryData.allItems = inventory.playerItemList;
	}

	
	

	public int GetGuffinsAnchorIdToInstance(GuffinsAnchor anchor) {
		return _worldAnchors.First(a => a.Value == anchor).Key;
	}
	public GuffinsAnchor GetGuffinsAnchorInstanceToId(int id) {
		if (_worldAnchors.ContainsKey(id)) {
			return _worldAnchors[id];
		}
		return null;
	}
	public int GetDiscoveredGuffinsAnchorCount() {
		return _worldAnchors.Count(a => a.Value.isDiscovered);
	}
}

}