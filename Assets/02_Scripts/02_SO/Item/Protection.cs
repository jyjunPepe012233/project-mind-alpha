using MinD.Structs;
using UnityEngine;

namespace MinD.SO.Item {

public abstract class Protection : Equipment {

	[Space(50), Header("[ Protection Status ]")]
	public DamageNegation negationBoost;
}

}