using System.Collections.Generic;
using UnityEngine;

namespace MinD.Runtime.Entity {

public class EnemyAnimationHandler : BaseEntityAnimationHandler {

	private Dictionary<string, bool> isStatesExist = new();
	

	public bool PlayTargetAnimation(string stateName, float normalizedTransformDuration, bool applyRootMotion, bool isPerformingAction) {

		if (!isStatesExist.ContainsKey(stateName)) {
			isStatesExist.Add(stateName, owner.animator.HasState(0, Animator.StringToHash(stateName)));
		}

		if (isStatesExist[stateName]) {
			owner.animator.CrossFadeInFixedTime(stateName, normalizedTransformDuration);
		}
		
		
		owner.animator.applyRootMotion = applyRootMotion;
		((Enemy)owner).isPerformingAction = isPerformingAction;

		return isStatesExist[stateName];
	}

}

}