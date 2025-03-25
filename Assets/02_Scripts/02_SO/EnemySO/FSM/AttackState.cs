using MinD.Runtime.Entity;
using UnityEngine;

namespace MinD.SO.EnemySO {

public class AttackState : EnemyState {
	
	public override EnemyState Tick(Enemy self) {

		if (!self.isPerformingAction && !self.isInAttack) {
			self.animation.PlayTargetAnimation(self.combat.latestAttack.motionStateName, 0.2f, true, true);

			self.isInAttack = true;
			
		} else if (!self.isPerformingAction && self.isInAttack) {
			// EXIT ATTACK
			self.isInAttack = false;
			return self.combatStanceState;
			
		}

		
		
		if (self.combat.latestAttack.rotateInAttackAction) {
			self.locomotion.RotateToTarget();
		}
		
		return self.currentState;
	}
	
}

}