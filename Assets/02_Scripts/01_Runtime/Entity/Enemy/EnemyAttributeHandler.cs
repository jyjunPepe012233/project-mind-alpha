using MinD.SO.EnemySO;
using MinD.Structs;
using UnityEngine;

namespace MinD.Runtime.Entity {

public class EnemyAttributeHandler : BaseEntityAttributeHandler {
	
	[SerializeField] private EnemyAttribute attributeSO;

	public override int MaxHp {
		get => attributeSO.maxHp;
		set {
			return;
		}
	}

	public override DamageNegation DamageNegation {
		get => attributeSO.damageNegation;
		set {
			return;
		}
	}

	public override int PoiseBreakResistance {
		get => attributeSO.poiseBreakResistance;
		set {
			return;
		}
	}

	public float moveSpeed {
		get => attributeSO.moveSpeed;
	}

	public float angularSpeed {
		get => attributeSO.angularSpeed;
	}

	public float corpseFadeTime {
		get => attributeSO.corpseFadeTime;
	}
}

}