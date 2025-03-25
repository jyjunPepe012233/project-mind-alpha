using MinD.Runtime.Entity;
using UnityEngine;
using UnityEngine.AI;

namespace MinD.SO.EnemySO {

[CreateAssetMenu(menuName = "MinD/Enemy SO/FSM/Pursue Target/Humanoid", fileName = "(Enemy)_PursueTarget")]
public class HumanoidPursueTargetState : PursueTargetState {
	
	public override EnemyState Tick(Enemy self) {

		// IF IN PIVOT, STAY IN PURSUE TARGET STATE
		if (self.isPerformingAction) {	
			return self.currentState;
		}
		
		self.currentTarget = self.combat.FindTargetBySight(self.idleState.detectAngle, self.idleState.detectRadius, self.idleState.absoluteDetectRadius);
		// IF TARGET IS NOT EXIST IN DETECT RANGE
		if (self.currentTarget == null) {
			return self.idleState;
		}
		

		self.navAgent.SetDestination(self.currentTarget.transform.position);

		// IF PATH IS NOT COMPLETED, GO TO IDLE
		if (self.navAgent.pathStatus != NavMeshPathStatus.PathComplete) {
			return self.idleState;
		}
		
		// SWITCH STATE TO IDLE REMAINING PATH DISTANCE IS LONGER THAN GIVE UP DISTANCE 
		if (self.navAgent.remainingDistance > giveUpDistance) {
			return self.idleState;
		}
		
		// PIVOT(TURN) TO DESIRE DIRECTION AND KEEP UP THIS STATE
		if (Mathf.Abs(self.combat.AngleToDesireDirection()) > 80) {
			self.locomotion.PivotTowards(self.navAgent.desiredVelocity);
			return self.currentState;
		}

		// SWITCH STATE TO COMBAT STANCE STATE IF TARGET IS IN COMBAT RANGE
		if (Vector3.Distance(self.transform.position, self.currentTarget.transform.position) < enterCombatStanceRadius) {
			return self.combatStanceState;
		}
		
		// CHASING
		self.locomotion.RotateToDesireDirection();
		self.locomotion.MoveToForward();
		return self.currentState;
	}
	
}

}