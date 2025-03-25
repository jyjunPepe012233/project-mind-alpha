using MinD.Utility;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace MinD.Runtime.Entity {

public class PlayerAnimationHandler : BaseEntityAnimationHandler {
	
	private float moveBlendLerpSpeed = 6;
	private float runBlendDampTime = 0.35f;
	
	private Vector2 moveBlend;
	private float runBlend;




	public void HandleAllParameter() {
		HandleLocomotionParameter();
	}

	private void HandleLocomotionParameter() {

		if (!((Player)owner).canRotate || !((Player)owner).canMove) {
			return;
		}
		
		
		// SET HORIZONTAL AND VERTICAL PARAMETER
		Vector3 localMoveDirx = transform.InverseTransformDirection(((Player)owner).locomotion.moveDirx);
		if (((Player)owner).isMoving) {
			moveBlend = Vector2.Lerp(moveBlend, new Vector2(localMoveDirx.x, localMoveDirx.z), Time.deltaTime * moveBlendLerpSpeed);
		} else {
			moveBlend = Vector2.Lerp(moveBlend, Vector2.zero, Time.deltaTime * moveBlendLerpSpeed);
		}

		owner.animator.SetFloat("Horizontal", moveBlend.x);
		owner.animator.SetFloat("Vertical", moveBlend.y);
		
		
		
		// SET RUN BLEND PARAMETER
		runBlend += (((Player)owner).locomotion.isSprinting ? 1 : -1) / runBlendDampTime * Time.deltaTime;
		runBlend = Mathf.Clamp01(runBlend);
			
		owner.animator.SetFloat("RunBlend", runBlend);
	}



	public void PlayTargetAction(string stateName,
		bool isPerformingAnimation,
		bool applyRootMotion = false,
		bool canRotate = true,
		bool canMove = true) {
		
		owner.animator.CrossFadeInFixedTime(stateName, 0.2f);

		
		owner.animator.applyRootMotion = applyRootMotion;

		((Player)owner).isPerformingAction = isPerformingAnimation;
		((Player)owner).canRotate = canRotate;
		((Player)owner).canMove = canMove;
		// THOSE FLAGS RESET WHEN STATE IS BACK TO 'DEFAULT MOVEMENT'

	}
	
	public void PlayTargetAction(string stateName, float transitionDuration,
		bool isPerformingAnimation,
		bool applyRootMotion = false,
		bool canRotate = true,
		bool canMove = true,
		bool colliderEnable = true) {
		
		owner.animator.CrossFadeInFixedTime(stateName, transitionDuration, 0);

		owner.animator.applyRootMotion = applyRootMotion;

		((Player)owner).isPerformingAction = isPerformingAnimation;
		((Player)owner).canRotate = canRotate;
		((Player)owner).canMove = canMove;
		
		PhysicUtility.SetActiveChildrenColliders(owner.transform, colliderEnable, LayerMask.GetMask("Damageable Entity"));
		// THOSE FLAGS RESET WHEN STATE IS BACK TO 'DEFAULT MOVEMENT' BY ResetPlayerFlags the StateBehaviour

	}
	
	public string GetPoiseBreakAnimation(int poiseBreakAmount, float hitAngle) {
		
	
		// DECIDE DIRECTION OF POISE BREAK ANIMATION BY HIT DIRECTION
		string hitDirection;
		// SET HIT DIRECTION	
		if (hitAngle > -45 && hitAngle < 45) {
			hitDirection = "F";

		} else if (hitAngle > 45 && hitAngle < 135) {
			hitDirection = "R";
		
		}  else if (hitAngle > 135 && hitAngle < -135) {
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
		
		} else {
			return null; // IF POISE BREAK AMOUNT IS BELOW TO 20, POISE BREAK DOESN'T OCCUR
		}

		return stateName;
	}
	
}

}