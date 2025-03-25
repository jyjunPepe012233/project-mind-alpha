using UnityEngine;

namespace MinD.Runtime.Entity {

public abstract class EntityOwnedHandler : MonoBehaviour {

	protected BaseEntity owner {
		get {
			if (ownedEntity == null) {
				ownedEntity = GetComponent<BaseEntity>();
			}
			return ownedEntity;
		}
	}
	
	private BaseEntity ownedEntity;

}

}