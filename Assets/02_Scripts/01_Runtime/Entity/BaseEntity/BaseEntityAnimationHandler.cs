using UnityEngine;

namespace MinD.Runtime.Entity {

public abstract class BaseEntityAnimationHandler : EntityOwnedHandler {

	[SerializeField] private float moveDirectionLerpSpeed = 3;
	private Vector2 locomotionParam;
	
	public void LerpMoveDirectionParameter(float horizontal, float vertical) {

		locomotionParam = Vector2.MoveTowards(locomotionParam, new Vector2(horizontal, vertical), Time.deltaTime * moveDirectionLerpSpeed * 0.33f);
		foreach (AnimatorControllerParameter parameter in owner.animator.parameters) {

			if (parameter.name == "Horizontal")
				owner.animator.SetFloat("Horizontal", locomotionParam.x);

			if (parameter.name == "Vertical")
				owner.animator.SetFloat("Vertical", locomotionParam.y);
		}

	}
	
}

}