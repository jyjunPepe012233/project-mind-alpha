using System.Linq;
using MinD.Runtime.Entity;
using UnityEngine;

namespace MinD.Utility {

public static class PhysicUtility {
	
	public static void IgnoreCollisionUtil(BaseEntity entity, Collider collider) {

		Collider[] cols = entity.GetComponentsInChildren<Collider>(true);

		foreach (Collider col in cols) {
			Physics.IgnoreCollision(col, collider);
		}

		if (entity is Player player) {
			Physics.IgnoreCollision(player.defenseMagic.defenseCollider, collider);
		}
	}
	
	public static void SetUpIgnoreBodyCollision(BaseEntity entity) {

		Collider[] cols = entity.GetComponentsInChildren<Collider>(true);

		foreach (Collider col1 in cols) {
			foreach (Collider col2 in cols) {
				Physics.IgnoreCollision(col1, col2);
			}
		}
	}

	public static void SetActiveChildrenColliders(Transform root, bool active, int layerMask = ~0, bool includeInactive = false) {
		
		// GET COLLIDER COMPONENTS IN CHILDREN WHAT LAYER IS INCLUDED IN LAYERMASK PARAMETER
		Collider[] cols = root.GetComponentsInChildren<Collider>(includeInactive) .Where(col => layerMask == (layerMask | 1 << col.gameObject.layer)) .ToArray();
		
		foreach (Collider col in cols) {
			col.enabled = active;
		}
		
	}
	
}

}