using MinD.Runtime.Entity;
using UnityEngine;

namespace MinD.SO.Item {

public abstract class Tool : Equipment {

	[Space(50), Header("[ Tool ]")]
	public bool isConsumable = true;
	public float usingCoolTime = 1;
	[HideInInspector] public float remainingCoolTime;

	
	
	public abstract void OnUse(Player owner);
	
}

}