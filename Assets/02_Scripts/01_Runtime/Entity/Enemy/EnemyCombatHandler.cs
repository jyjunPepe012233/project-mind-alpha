using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using MinD.SO.EnemySO;
using MinD.Utility;
using Unity.VisualScripting;
using UnityEngine.Serialization;


namespace MinD.Runtime.Entity {

public class EnemyCombatHandler : EntityOwnedHandler {
	
	public float attackActionRecoveryTimer; 
	public EnemyAttackAction latestAttack;

	[HideInInspector] public bool willPerformCombo;
	[HideInInspector] public EnemyAttackAction comboAttack;
	
	public List<EnemyAttackAction> reservedAttackQueue; // HP 리미트가 해제되어 사용할 수 있는 공격
	public List<EnemyAttackAction> liftedAttacks; // HP 리미트가 해제된 공격 



	public void HandleLimitedAttacks()
	{
		foreach (var a in ((Enemy)owner).combatStanceState.GetLiftedAttacksByHealthLimit((Enemy)owner, liftedAttacks, owner.CurHp))
		{
			reservedAttackQueue.Add(a);
			liftedAttacks.Add(a); 
		}
	}
	
	public float DistanceToTarget() {
		return Vector3.Distance( ((Enemy)owner).currentTarget.transform.position, owner.transform.position);
	}
	public float AngleToTarget() {
		return Vector3.SignedAngle(owner.transform.forward, (((Enemy)owner).currentTarget.transform.position - transform.position), Vector3.up);
	}
	public float AngleToDesireDirection() {
		return Vector3.SignedAngle(transform.forward, ((Enemy)owner).navAgent.desiredVelocity, Vector3.up);
	}

	
	
	public BaseEntity FindTargetBySight(float detectRadius, float absoluteDetectRadius, float detectAngle) {
		
		Collider[] colliders = Physics.OverlapSphere(transform.position, detectRadius, WorldUtility.damageableLayerMask);
		if (colliders.Length == 0) {
			return null;
		}
		
		
		List<BaseEntity> potentialTargets = new List<BaseEntity>();
		for (int i = 0; i < colliders.Length; i++) {

			var potentialTargetEntity = colliders[i].GetComponentInParent<BaseEntity>();

			if (potentialTargetEntity == null) {
				continue;
			}
			if (potentialTargets.Contains(potentialTargetEntity)) {
				continue;
			}
			if (potentialTargetEntity == owner) {
				continue;
			}
			if (potentialTargetEntity.isInvincible) {
				continue;
			}
			if (potentialTargetEntity.isDeath) {
				continue;
			}
			if (potentialTargetEntity is Enemy) {
				continue;
			}
			
			potentialTargets.Add(potentialTargetEntity);
		}
		if (potentialTargets.Count == 0) {
			return null;
		}

		
		// SELECT POTENTIAL TARGETS THAT AVAILABLE 
		List<BaseEntity> availableTargets  = new List<BaseEntity>();
		for (int i = 0; i < potentialTargets.Count; i++) {
			
			// CHECK OBSTACLE BETWEEN POTENTIAL TARGET
			if (Physics.Linecast(potentialTargets[i].targetOptions[0].transform.position, owner.targetOptions[0].transform.position, WorldUtility.environmentLayerMask)) {
				continue;
			}

			// IF TARGET IS IN ABSOLUTE DETECT RANGE:
			if (Vector3.Distance(potentialTargets[i].transform.position, owner.transform.position) < absoluteDetectRadius) {
				// MAKE TARGET TO AVAILABLE
				
				availableTargets.Add(potentialTargets[i]);
				continue;
			}
			
			// CHECK ANGLE TO TARGET
			if (Vector3.SignedAngle(owner.transform.forward, (potentialTargets[i].transform.position - owner.transform.position), Vector3.up) > detectAngle) {
				continue;
			}
			
			availableTargets.Add(potentialTargets[i]);
		}
		if (availableTargets.Count == 0) {
			return null;
		}
		
		
		// ORDER BY PROXIMITY
		return availableTargets.OrderBy(i => Vector3.Distance(i.targetOptions[0].transform.position, owner.targetOptions[0].transform.position)).ToArray()[0];
	}
}

}