using MinD.SO.Item;
using UnityEngine;

namespace MinD.Runtime.Entity {

public class PlayerEquipmentHandler : EntityOwnedHandler {
	
	public Transform rightHand;

	private GameObject currentInstantiatedWeapon;


	public void ChangeWeapon(Weapon weapon) {

		if (currentInstantiatedWeapon != null) {
			Destroy(currentInstantiatedWeapon);
			currentInstantiatedWeapon = null;
		}

		if (weapon != null) {
			GameObject obj = Instantiate(weapon.weaponPrefab, rightHand);

			obj.transform.localPosition = weapon.weaponPositionOffset;
			obj.transform.localEulerAngles = weapon.weaponAngleOffset;

			currentInstantiatedWeapon = obj;
		}
	}

}

}