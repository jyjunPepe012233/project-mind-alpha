using MinD.Runtime.Entity;
using UnityEngine;
using UnityEngine.AI;

namespace MinD.SO.EnemySO {

[CreateAssetMenu(menuName = "MinD/Enemy SO/FSM/Combat Stance/Humanoid", fileName = "(Enemy)_CombatStance")]
public class HumanoidCombatStanceState : CombatStanceState {
	
	[SerializeField] private float combatPursueMinDistance;
	[SerializeField] private float combatPursueMaxDistance;

	[Space(10)]
	[SerializeField] private float maxStrafeTime; // WHEN STRAFE TIMER IS OVER MAXIMUM, START DASHING TO TARGET
	

	public override EnemyState Tick(Enemy self) {

		var castedSelf = (HumanoidEnemy)self;

		if (self.isPerformingAction) {
			return self.currentState;
		}

		if (self.currentTarget.isDeath) {
			return self.pursueTargetState;
		}

		if (self.combat.DistanceToTarget() > exitCombatStanceRadius) {
			return self.pursueTargetState;
		}
		
		self.navAgent.SetDestination(self.currentTarget.transform.position);
		
		
		// DASHING TO TARGET STATE
		if ((castedSelf.isDashingToTarget)) {
			self.locomotion.RotateToDesireDirection();
			self.locomotion.MoveToForward(true);

			if (self.combat.DistanceToTarget() < combatPursueMinDistance) {
				castedSelf.isDashingToTarget = false;
				castedSelf.strafeTimer = 0;
			}
		}
		
		
		

		// CHECK NEXT ATTACK
		if (self.combat.attackActionRecoveryTimer < (self.combat.latestAttack != null ? self.combat.latestAttack.actionRecoveryTime : 0) ) {
			// IS IN RECOVERY
			self.combat.attackActionRecoveryTimer += Time.deltaTime;
			
		} else {
			
			var nextAttack = GetAttackActionRandomly(self);
			if (nextAttack != null) {
				return AttemptAttackAction(self, nextAttack);
			}
		}
		
		
		
		// IF AVAILABLE STATE IS NOT EXISTS,
		// PURSUE PROPER DISTANCE TO COMBAT
		if (self.combat.DistanceToTarget() < combatPursueMinDistance) {
			castedSelf.strafeDirx.z = -1;
		} else if (self.combat.DistanceToTarget() > combatPursueMaxDistance) {
			castedSelf.strafeDirx.z = 1;
		} else {
			castedSelf.strafeDirx.z = 0;
		}

		
		Vector3 targetLocalMoveDirx = self.currentTarget.transform.InverseTransformDirection(self.currentTarget.cc.velocity.normalized);
		
		if (Mathf.Abs(targetLocalMoveDirx.x) > 0.5) { // TARGET IS ON RIGHT
			castedSelf.strafeDirx.x = Mathf.Sign(targetLocalMoveDirx.x); // MIRROR DIRECTION OF TARGET
			
			NavMeshPath path = new();
			if (!self.navAgent.CalculatePath(self.transform.TransformPoint(castedSelf.strafeDirx), path)) {
				castedSelf.isDashingToTarget = true;
				
			}
			
			
		} else if (self.combat.DistanceToTarget() > combatPursueMinDistance && 
		           self.combat.DistanceToTarget() < combatPursueMaxDistance) {
			// IF IN COMBAT PURSUE DISTANCE BUT PLAYER IS NOT MOVING
			// STRAFE TO WHERE THERE IS SPACE
			
			// LEFT DIRECTION IS PRIORITY
			if (castedSelf.strafeDirx.x == 0) {
				castedSelf.strafeDirx.x = -1;
			}
			
			// CALCULATE PATH AND ADJUST DIRECTION IF WALL IS EXISTS
			NavMeshPath path = new();
			if (!self.navAgent.CalculatePath(self.transform.TransformPoint(castedSelf.strafeDirx), path)) {
				castedSelf.strafeDirx.x *= -1; // INVERSE DIRECTION OF STRAFE 
				
			}
		} else {
			castedSelf.strafeDirx.x = 0;

		}
		
		
		// WHEN STRAFE TIMER IS OVER MAXIMUM, START DASHING TO TARGET
		((HumanoidEnemy)self).strafeTimer += Time.deltaTime;
		if (((HumanoidEnemy)self).strafeTimer > maxStrafeTime) {
			((HumanoidEnemy)self).isDashingToTarget = true;
		}
		
		
		if (castedSelf.strafeDirx.magnitude != 0) {
			self.locomotion.StrafeToward(castedSelf.strafeDirx);
		}
		self.locomotion.RotateToTarget();
		
		
		
		return self.currentState;
	}
	
}

}