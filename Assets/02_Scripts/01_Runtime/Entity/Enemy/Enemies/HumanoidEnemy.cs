using System.Collections;
using MinD.Runtime.Managers;
using MinD.Utility;
using MinD.SO.StatusFX.Effects;
using UnityEngine;

namespace MinD.Runtime.Entity {

public abstract class HumanoidEnemy : Enemy {

	private bool isKnockedDown;
	
	[HideInInspector] public bool isDashingToTarget; 
	[HideInInspector] public float strafeTimer;

	[HideInInspector] public Vector3 strafeDirx;



	public override void OnDamaged(TakeHealthDamage damage) {

		base.OnDamaged(damage);
		
		if (isDeath) {
			return;
		}

		float hitAngle = damage.attackAngle;
		int poiseBreakAmount = TakeHealthDamage.GetPoiseBreakAmount(damage.poiseBreakDamage, attribute.PoiseBreakResistance);

		// DECIDE DIRECTION OF POISE BREAK ANIMATION BY HIT DIRECTION
		string hitDirection;
		// SET HIT DIRECTION	
		if (hitAngle > -45 && hitAngle < 45) {
			hitDirection = "F";

		} else if (hitAngle > 45 && hitAngle < 135) {
			hitDirection = "R";

		} else if (hitAngle > 135 && hitAngle < -135) {
			hitDirection = "B";

		} else {
			hitDirection = "L";
		}



		string stateName = "Hit_";

		// DECIDE ANIMATION BY CALCULATED POISE BREAK AMOUNT
		if (poiseBreakAmount >= 80) {
			stateName += "KnockDown_Start";

			Vector3 angle = transform.eulerAngles;
			angle.y += hitAngle;
			transform.eulerAngles = angle;

		} else if (poiseBreakAmount >= 55) {
			stateName += "Large_";
			stateName += hitDirection;

		} else if (poiseBreakAmount >= 20) {
			stateName += "Default_";
			stateName += hitDirection;

		}
		
		animation.PlayTargetAnimation(stateName, 0.2f, true, true);
	}
	
	protected override void OnDeath() {
		base.OnDeath();
		StartCoroutine(Death());
	}

	private IEnumerator Death() {

		currentState = null;
		animation.PlayTargetAnimation("Death", 0.2f, true, true);
		
		PhysicUtility.SetActiveChildrenColliders(transform, false, WorldUtility.damageableLayerMask);

		yield return new WaitForSeconds(attribute.corpseFadeTime);
		Destroy(gameObject);
	}
	
}

}