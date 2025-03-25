using MinD.Structs;
using UnityEngine;

namespace MinD.SO.Object {

[CreateAssetMenu(fileName = "Damage Data", menuName = "MinD/Object/Damage Data")]
public class DamageData : ScriptableObject {
	
	public Damage damage; 
	public int totalDamage; // ONLY FOR SHOWING
	
	[Space(5)]
	[Range(0, 100)] public int poiseBreakDamage;

	[Space(10)]
	public int absorbMp;

	[Space(10)]
	public bool isDirectional;
	public Vector3 damageDirection;
	
	

	void OnValidate() {
		RefreshValue();
	}

	public void RefreshValue() {
		
		totalDamage = damage.physical + damage.magic + damage.fire + damage.frost + damage.lightning + damage.holy;
		
	}
	
}

}