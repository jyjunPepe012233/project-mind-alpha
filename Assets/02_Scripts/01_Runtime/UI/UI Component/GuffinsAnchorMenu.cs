using System.Collections.Generic;
using MinD.Runtime.Entity;
using MinD.Runtime.Managers;
using MinD.Runtime.Object.Interactables;
using TMPro;
using UnityEngine;

namespace MinD.Runtime.UI {

public class GuffinsAnchorMenu : PlayerMenu {
	
	public enum OptionType {
		TeleportToOtherAnchor,
		LevelUp,
		Memorize,
		ReadTheStory,
		Leave
	}

	private static Dictionary<OptionType, string> optionTextDictionary = new Dictionary<OptionType, string>() {
		{ OptionType.TeleportToOtherAnchor, "다른 닻으로 이동한다"}, 
		{ OptionType.LevelUp, "레벨 업"}, 
		{ OptionType.Memorize, "기억"}, 
		{ OptionType.ReadTheStory, "이야기를 읽는다"}, 
		{ OptionType.Leave, "떠난다"}, 
	};

	private static Color normalOptionColor = new Color(1, 1, 1, 0.2f);
	private static Color focusedOptionColor = new Color(1, 1, 1, 1);
	
	

	private Dictionary<OptionType, bool> optionActiveDictionary = new Dictionary<OptionType, bool>() {
		{ OptionType.TeleportToOtherAnchor, default },
		{ OptionType.LevelUp, default },
		{ OptionType.Memorize, default },
		{ OptionType.ReadTheStory, default },
		{ OptionType.Leave, default }
	};
	
	[Header("[ Owned Objects ]")]
	[SerializeField] private TextMeshProUGUI anchorName;
	[SerializeField] private RectTransform optionGroup;
	
	[Header("[ Prefab ]")]
	[SerializeField] private GameObject optionPrefab;

	private List<GuffinsAnchorOption> instantiatedOptions;
	private int currentFocusOption;
	
	
	
	public override void Open() {
		
		InstantiateOptionMenu();
		
		SelectOption(currentFocusOption, 0);
		currentFocusOption = 0;

	}

	public override void Close() {

		foreach (GuffinsAnchorOption option in instantiatedOptions) {
			Destroy(option.gameObject);
		}
		instantiatedOptions.Clear();
	}

	public void ApplyGuffinsAnchorData(GuffinsAnchor anchor) {
		anchorName.text = anchor.anchorInfo.anchorName;

		optionActiveDictionary[OptionType.TeleportToOtherAnchor] = true;
		optionActiveDictionary[OptionType.LevelUp] = true;
		optionActiveDictionary[OptionType.Memorize] = true;
		optionActiveDictionary[OptionType.ReadTheStory] = anchor.anchorInfo.canReadStory;
		optionActiveDictionary[OptionType.Leave] = true;

	}

	private void InstantiateOptionMenu() {

		instantiatedOptions = new List<GuffinsAnchorOption>();
		
		
		foreach (KeyValuePair<OptionType, bool> optionData in optionActiveDictionary) {

			if (!optionData.Value) { // IF OPTION DISABLED
				continue;
			}
			
			var option = Instantiate(optionPrefab, optionGroup).GetComponent<GuffinsAnchorOption>();
			
			// SET ORDERED POSITION
			Vector2 localPosition = option.transform.localPosition;
			localPosition.y = instantiatedOptions.Count * -((RectTransform)option.transform).sizeDelta.y;
			option.transform.localPosition = localPosition;

			option.optionType = optionData.Key;
			option.text.text = optionTextDictionary[optionData.Key];
			option.hover.color = normalOptionColor;
			
			instantiatedOptions.Add(option);
		}
	}

	public override void OnInputWithDirection(Vector2 inputDirx) {
		int moveFocusDirection = (int)-Mathf.Sign(inputDirx.y); // -1 or 1
		
		int previousOption = currentFocusOption;
		currentFocusOption = Mathf.Clamp(currentFocusOption + moveFocusDirection, 0, instantiatedOptions.Count - 1);
		
		SelectOption(previousOption, currentFocusOption);
	}

	private void SelectOption(int previousOptionIndex, int newOptionIndex) {

		GuffinsAnchorOption previousOption = instantiatedOptions[previousOptionIndex];
		if (previousOption != null) {
			previousOption.hover.color = normalOptionColor;
		}

		GuffinsAnchorOption newOption = instantiatedOptions[newOptionIndex];
		if (newOption != null) {
			newOption.hover.color = focusedOptionColor;
		}
	}


	public override void OnSelectInput() {

		switch (instantiatedOptions[currentFocusOption].optionType) {
			
			case OptionType.LevelUp:
				// TODO: Open Level up menu;
				break;
			
			case OptionType.Memorize:
				// TODO: Open Memorize menu;
				break;
			
			case OptionType.ReadTheStory:
				// TODO: Open Story menu
				break;
			
			case OptionType.Leave:
				Leave();
				break;
			
		}
	}

	public override void OnQuitInput() {
		Leave();
	}



	private void Leave() {
		PlayerHUDManager.Instance.CloseMenu(this);
		Player.player.animation.PlayTargetAction("Anchor_End", 0.05f, true, true, false, false);
	}
}

}