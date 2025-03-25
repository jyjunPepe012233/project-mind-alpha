using UnityEngine;

namespace MinD.Runtime.Entity {

public class EnemyLocomotionHandler : EntityOwnedHandler {
	
	private bool isMoved;
	
	public void PivotTowards(Vector3 direction) {

		float pivotAngle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);
		// CLAMP MINIMUM THE ABSOLUTE VALUE OF PIVOT ANGLE TO 90
		if (Mathf.Abs(pivotAngle) < 90) {
			pivotAngle = Mathf.Sign(pivotAngle) * 90;
		}
		
		owner.animator.SetFloat("Pivot Lerp Factor", pivotAngle / 90);
		// DIRECTION OF PIVOT ANIMATION IS DECIDE BY SIGN OF FACTOR VALUE.
		// BLEND THRESHOLD OF 90 DEGREE TURN IS 1,
		// 180 DEGREE TURN IS 2.
		((Enemy)owner).animation.PlayTargetAnimation("Pivot_BlendTree", 0.2f, true, true);
		
	}

	public void RotateToDesireDirection() {

		if (((Enemy)owner).navAgent.desiredVelocity.Equals(Vector3.zero)) {
			return;
		}
		
		transform.rotation = Quaternion.RotateTowards(
			transform.rotation,
			Quaternion.LookRotation(((Enemy)owner).navAgent.desiredVelocity),
			Time.deltaTime * ((Enemy)owner).attribute.angularSpeed
			);
	}
	public void RotateToTarget() {

		transform.rotation = Quaternion.RotateTowards(
			transform.rotation,
			Quaternion.LookRotation(((Enemy)owner).currentTarget.transform.position - transform.position), 
			Time.deltaTime * ((Enemy)owner).attribute.angularSpeed
			);
	}
	
	public void ResetMoveDirectionParameter() {
		
		// LERP ANIMATION FACTOR TO ZERO IF ENEMY IS NOT MOVING
		if (!isMoved) {
			((Enemy)owner).animation.LerpMoveDirectionParameter(0, 0);
		}
		isMoved = false;
		
	}
	
	public void MoveToForward(bool run = false) {
		
		owner.animator.SetFloat("Base Locomotion Speed", ((Enemy)owner).attribute.moveSpeed);
		((Enemy)owner).animation.LerpMoveDirectionParameter(0, (run ? 2 : 1));

		isMoved = true;
	}

	public void StrafeToward(Vector3 strafeLocalDirection) {

		owner.animator.SetFloat("Base Locomotion Speed", ((Enemy)owner).attribute.moveSpeed);
		
		strafeLocalDirection.y = 0;
		strafeLocalDirection.Normalize();
		((Enemy)owner).animation.LerpMoveDirectionParameter(strafeLocalDirection.x, strafeLocalDirection.z);

		isMoved = true;
	}

}

}