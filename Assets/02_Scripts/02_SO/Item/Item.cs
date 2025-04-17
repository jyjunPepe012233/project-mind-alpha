using System.Collections;
using System.Collections.Generic;
using MinD;
using MinD.Enums;
using UnityEngine;

namespace MinD.SO.Item
{

	public abstract class Item : ScriptableObject
	{

		[HideInInspector] public int itemId; // ITEM'S ID IS GENERATE AUTOMATICALLY IN ItemDataList

		[Header("[ Setting ]")] public string itemName;
		[TextArea(20, 20)] public string itemDescription;
		public Sprite itemImage;

		[Space(5)] public ItemRarityEnum itemRarity;

		[Space(5)] public int itemMaxCount = 1;

		[Header("[ Runtime Data ]")] public int itemCount = 0;

		public int categoryId;

		public bool isEquipped;
		
		[HideInInspector] public int equippedOrder = -1;

	}
	
}
