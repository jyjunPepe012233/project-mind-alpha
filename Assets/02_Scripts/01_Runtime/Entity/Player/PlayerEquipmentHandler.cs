using System.Collections.Generic;
using MinD.SO.Item;
using UnityEngine;

namespace MinD.Runtime.Entity {

public class PlayerEquipmentHandler : EntityOwnedHandler {
	
	public Transform rightHand;

	private GameObject currentInstantiatedWeapon;
	private Dictionary<Weapon, GameObject> weaponSoObjectPair = new Dictionary<Weapon, GameObject>();


	public void ChangeWeapon(Weapon weapon) {

		if (weapon != null)
		{
			// 현재 활성화된 무기 오브젝트를 비활성화
			if (currentInstantiatedWeapon != null)
			{
				currentInstantiatedWeapon.SetActive(false);
			}

			// 딕셔너리에 저장되어 있는 Weapon인지 확인한 후, 아니면 생성
			if (weaponSoObjectPair.ContainsKey(weapon))
			{	
				weaponSoObjectPair[weapon].SetActive(true);
				currentInstantiatedWeapon = weaponSoObjectPair[weapon];
			}
			else
			{
				currentInstantiatedWeapon = InstantiateWeapon(weapon);
			}
		}
		else
		{
			// 현재 활성화된 무기 오브젝트를 비활성화
			if (currentInstantiatedWeapon != null)
			{
				currentInstantiatedWeapon.SetActive(false);
				currentInstantiatedWeapon = null;
			}
		}
	}

	private GameObject InstantiateWeapon(Weapon weapon)
	{
		float playerScale = 100;
		GameObject weaponObject = Instantiate(weapon.weaponPrefab, rightHand);
		weaponObject.transform.localPosition = weapon.weaponPositionOffset;
		weaponObject.transform.localEulerAngles = weapon.weaponAngleOffset;
		weaponObject.transform.localScale = Vector3.one * (1 / playerScale);
		
		weaponSoObjectPair.Add(weapon, weaponObject);
		return weaponObject;
	}

}

}