using MinD.Enums;
using MinD.Structs;
using UnityEngine;

namespace MinD.SO.Item {

public abstract class Weapon : Equipment {

	public GameObject weaponPrefab;
	public Vector3 weaponPositionOffset;
	public Vector3 weaponAngleOffset;

	[Header("[ Weapon Setting ]")]
	public WeaponType weaponType;

	[Header("[ Weapon Status ]")]
	public SpiritAffinity weaponRequiredAffinity;
	public Damage weaponDamage;

}

}