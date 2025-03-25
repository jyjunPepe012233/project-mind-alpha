using MinD.Enums;
using MinD.Runtime.Entity;
using MinD.Structs;
using UnityEngine;
using UnityEngine.Serialization;

namespace MinD.SO.StatusFX.Effects {

[CreateAssetMenu(fileName = "AbsorbMagic", menuName = "MinD/Status Effect/Effects/TakeAbsorbMagic")]
public class AbsorbMagic : InstantEffect {

	public int absorbMp; 
	public Vector3 worldAttackDirx;
	

	
	public AbsorbMagic(int absorbMp, Vector3 worldAttackDirx) {
		this.absorbMp = absorbMp;
		this.worldAttackDirx = worldAttackDirx;
	}


	public override void OnInstantiate(BaseEntity victim) {
		throw new System.NotImplementedException();
	}
}

}