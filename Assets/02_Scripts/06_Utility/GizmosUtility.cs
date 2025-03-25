using UnityEngine;

namespace MinD.Utility {

public class GizmosUtility {

	private static Color defaultColor1 = new Color(.05f, 1, .05f, .65f);
	
	

	public static void DrawColliderGizmo(Collider collider, Color? color = null) {

		if (color == null) {
			Gizmos.color = defaultColor1;
		} else {
			Gizmos.color = color.Value;
		}
		
		
		switch (collider) {
			
			case BoxCollider box:
				DrawBoxCollider(box);
				break;
			
			case SphereCollider sphere:
				DrawSphereCollider(sphere);
				break;
			
			case CapsuleCollider capsule:
				DrawCapsuleCollider(capsule);
				break;
			
			case CharacterController cc:
				DrawCCCollider(cc);
				break;
		}

		Gizmos.color = default;
	}

	private static void DrawBoxCollider(BoxCollider collider) {

		Gizmos.matrix = collider.transform.localToWorldMatrix;
		Gizmos.DrawCube(collider.center, collider.size);

	}
	private static void DrawSphereCollider(SphereCollider collider) {
		
		Gizmos.matrix = collider.transform.localToWorldMatrix;
		Gizmos.DrawSphere(collider.center, collider.radius);
		
	}
	private static void DrawCapsuleCollider(CapsuleCollider collider) {

		Gizmos.matrix = collider.transform.localToWorldMatrix;

		// SET CAPSULE DIRECTION BY AXIS PARAMETER
		Vector3 upVector = Vector3.up;
		Vector3 downVector = Vector3.down;
		
		switch (collider.direction) {
			case 0:
				upVector = Vector3.left;
				downVector = Vector3.right;
				break;
			case 1:
				upVector = Vector3.up;
				downVector = Vector3.down;
				break;
			case 2:
				upVector = Vector3.forward;
				downVector = Vector3.back;
				break;
		}
		
		
		// DRAW CAPSULE --------------
		
		// CAPSULE HEAD(ROUND PART)
		float capsuleHeadPosition = Mathf.Clamp((collider.height / 2 - collider.radius), 0, Mathf.Infinity);
		Gizmos.DrawSphere(upVector * capsuleHeadPosition + collider.center, collider.radius);
		Gizmos.DrawSphere(downVector * capsuleHeadPosition + collider.center, collider.radius);
		
		// BETWEEN LERP
		var cl = Gizmos.color;
		cl.a *= 0.5f;
		Gizmos.color = cl;

		int lerpSphereCount = (int)(collider.height / collider.radius);
		
		for (int i = 1; i < lerpSphereCount; i++) {
			Vector3 position = Vector3.Lerp(upVector * capsuleHeadPosition, downVector * capsuleHeadPosition, 1 / (float)lerpSphereCount * i);
			Gizmos.DrawSphere(position + collider.center, collider.radius);
		}
		
		// CAPSULE STEM
		Gizmos.color = Color.white;
		Gizmos.DrawLine(upVector * capsuleHeadPosition + collider.center, downVector * capsuleHeadPosition + collider.center);
		
	}
	private static void DrawCCCollider(CharacterController cc) {

		Gizmos.matrix = cc.transform.localToWorldMatrix;

		// CAPSULE HEAD(ROUND PART)
		float capsuleHeadPosition = Mathf.Clamp((cc.height / 2 - cc.radius), 0, Mathf.Infinity);
		Gizmos.DrawSphere(Vector3.up * capsuleHeadPosition + cc.center, cc.radius);
		Gizmos.DrawSphere(Vector3.down * capsuleHeadPosition + cc.center, cc.radius);


		// BETWEEN LERP
		var cl = Gizmos.color;
		cl.a *= 0.5f;
		Gizmos.color = cl;

		int lerpSphereCount = (int)(cc.height / cc.radius);
		for (int i = 1; i < lerpSphereCount; i++) {
			Vector3 position = Vector3.Lerp(Vector3.up * capsuleHeadPosition, Vector3.down * capsuleHeadPosition, 1 / (float)lerpSphereCount * i);
			Gizmos.DrawSphere(position + cc.center, cc.radius);
		}


		// CAPSULE STEM
		Gizmos.color = Color.white;
		Gizmos.DrawLine(Vector3.up * capsuleHeadPosition + cc.center, Vector3.down * capsuleHeadPosition + cc.center);
		
	}

}

}