using MinD.Runtime.Entity;
using UnityEngine;

namespace MinD.SO.Item.Items {

[CreateAssetMenu(fileName = "Dummy Staff", menuName = "MinD/Item/Items/Equipment/Weapons/Staffs/Dummy Staff")]
public class DummyStaff : Weapon {

	public override void OnEquip(Player owner) {
		Debug.Log("장비함!");
	}

	public override void Execute(Player owner) {

	}

	public override void OnUnequip(Player owner) {
		Debug.Log("장비 해제!");
	}
}

}