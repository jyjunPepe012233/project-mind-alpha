using System.Collections.Generic;
using UnityEngine;

namespace MinD.SO.Item {

[CreateAssetMenu(fileName = "Item SO List", menuName = "MinD/Item/SO List", order = int.MinValue)]
public class ItemSoList : ScriptableObject {

	public List<Weapon> weaponList;
	public List<Protection> protectionList;
	public List<Talisman> talismanList;
	public List<Tool> toolList;
	public List<Magic> magicList;

}

}