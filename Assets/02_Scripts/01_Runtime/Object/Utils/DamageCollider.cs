using System.Collections.Generic;
using MinD.Runtime.Entity;
using MinD.SO.Object;
using MinD.SO.StatusFX;
using MinD.SO.StatusFX.Effects;
using UnityEngine;

namespace MinD.Runtime.Utils {

public class DamageCollider : MonoBehaviour {
	
	public DamageData soData;


	public List<BaseEntity> blackList = new List<BaseEntity>(); // IGNORE THIS DAMAGE COLLIDER
	
	private List<BaseEntity> damagedEntity = new List<BaseEntity>();
	
	
	
	private void OnTriggerEnter(Collider other) {
		GiveDamage(other);
	}

	private void GiveDamage(Collider other) {

		BaseEntity damageTarget = other.GetComponentInParent<BaseEntity>();
		if (damageTarget == null) {
			
			var playerDefenseMagic = other.GetComponent<PlayerDefenseMagic>();
			var entityComponentInCollider = other.GetComponent<BaseEntity>();

			if (playerDefenseMagic != null) {
				damageTarget = playerDefenseMagic.owner;
				
			} else if (playerDefenseMagic == null) {
				return;
			}
		}
		
		GiveDamage(damageTarget, (other.bounds.center) - transform.position);
	}

	private void GiveDamage(BaseEntity damageTarget, Vector3 contactDirection) {
		
		if (soData == null) { // IF NO DAMAGE DATA IS REGISTERED, THE DAMAGE COLLIDER WILL DISABLE
			gameObject.SetActive(false); 
			return;
		}

		if (damageTarget.isInvincible) {
			return;
		}
		if (damagedEntity.Contains(damageTarget)) {
			return;
		}
		if (blackList.Contains(damageTarget)) {
			return;
		}
		
		contactDirection.Normalize();
		damagedEntity.Add(damageTarget);


		float getHitAngle = 0;
		if (soData.isDirectional) {
			getHitAngle = Vector3.SignedAngle(damageTarget.transform.forward, transform.rotation * -soData.damageDirection, Vector3.up);
		} else {
			getHitAngle = Vector3.SignedAngle(damageTarget.transform.forward, -contactDirection, Vector3.up); // ATTACK ANGLE AS TARGET
		}

		InstantEffect damageEffect = null;
		if (damageTarget is Player player) {
			
			if (player.combat.isParrying) {
				damageEffect = new AbsorbMagic(soData.absorbMp, contactDirection);
				
			} else if (player.combat.usingDefenseMagic) {
				damageEffect = new TakeDefensedHealthDamage(soData.damage, soData.poiseBreakDamage, getHitAngle, contactDirection);

			} else {
				damageEffect = new TakeHealthDamage(soData.damage, soData.poiseBreakDamage, getHitAngle);
			}
			
		} else { 
			damageEffect = new TakeHealthDamage(soData.damage, soData.poiseBreakDamage, getHitAngle);
			
		}

		// GIVE EFFECT TO TARGET
		damageTarget.statusFx.AddInstantEffect(damageEffect);
	}
	



	private void OnDisable() {
		ResetToHitAgain();
	}

	public void ResetToHitAgain() {
		damagedEntity.Clear();
	}

	public void OnDrawGizmosSelected() {
		if (soData != null &&
			soData.isDirectional) {
			Gizmos.color = Color.yellow;
			Gizmos.DrawRay(transform.position, transform.rotation * soData.damageDirection.normalized);
		}
	}

}

}