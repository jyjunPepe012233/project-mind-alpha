using MinD.Runtime.DataBase;
using MinD.Runtime.Object.Magics;
using UnityEngine;

namespace MinD.SO.Item.Items {

[CreateAssetMenu(menuName = "MinD/Item/Items/Magics/Demon Flame")]
public class DemonFlame : Magic {
	// 마염
	
	[Space(15)]
	[SerializeField] private GameObject spirit;
	

	public override void OnUse() {

		castPlayer.animation.PlayTargetAction("DemonFlame", true, true, false, false);

	}

	public override void Tick() {

		if (castPlayer.isPerformingAction == false) // ON ANIMATION IS END
			castPlayer.combat.ExitCurrentMagic();

	}

	public override void OnReleaseInput() {
	}

	public override void OnCancel() {
		castPlayer.combat.ExitCurrentMagic();
	}

	public override void OnExit() {
	}


	public override void OnSuccessfullyCast() {

		Vector3 summonPos = castPlayer.transform.position + castPlayer.transform.up * 2.3f;

		DemonFlameSpirit demonFlame = Instantiate(spirit).GetComponent<DemonFlameSpirit>();
		demonFlame.transform.position = summonPos;

		if (castPlayer.combat.target != null) {
			demonFlame.Shoot(castPlayer, castPlayer.combat.target);
		} else {
			demonFlame.Shoot(castPlayer, null);
		}

	}

}

}