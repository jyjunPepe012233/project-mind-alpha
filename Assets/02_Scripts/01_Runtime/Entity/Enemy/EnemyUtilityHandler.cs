using System;
using System.Collections;
using MinD.Runtime.Utils;
using UnityEngine;

namespace MinD.Runtime.Entity {

public class EnemyUtilityHandler : EntityOwnedHandler {

	[Space(10)] [SerializeField] private GameObject[] ownedObjects = Array.Empty<GameObject>();
	[SerializeField] private GameObject[] prefabs = Array.Empty<GameObject>();




	public GameObject InstantiatePrefab(string targetObject) {

		foreach (GameObject prefab in prefabs) {
			if (prefab.name == targetObject) {
				return Instantiate(prefab);
			}
		}

		throw new UnityException("!! CAN'T FIND " + owner.name + " OWNED OBJECT THE NAMED " + targetObject);
	}

	public void EnableObject(string targetObjects) {
		// MULTIPLE OBJECTS ARE SEPARATE BY SPACE 

		string[] targetObjNames = targetObjects.Split();

		foreach (string targetObjName in targetObjNames) {

			bool findTarget = false;

			foreach (GameObject obj in ownedObjects) {
				if (obj.name == targetObjName) {
					obj.SetActive(true);
					findTarget = true;
				}
			}

			if (!findTarget) {
				throw new UnityException("!! CAN'T FIND " + owner.name + " OWNED OBJECT THE NAMED " + targetObjNames);
			}
		}

	}

	public void DisableObject(string targetObjects) {
		// MULTIPLE OBJECTS ARE SEPARATE BY SPACE 

		string[] targetObjNames = targetObjects.Split();

		foreach (string targetObjName in targetObjNames) {

			bool findTarget = false;

			foreach (GameObject obj in ownedObjects) {
				if (obj.name == targetObjName) {
					obj.SetActive(false);
					findTarget = true;
				}
			}

			if (!findTarget) {
				throw new UnityException("!! CAN'T FIND " + owner.name + " OWNED OBJECT THE NAMED " + targetObjNames);
			}
		}
	}


	public void ResetDamageColliderToHitAgain(string colliderName) {

		foreach (GameObject obj in ownedObjects) {

			if (obj.name == colliderName) {
				var cols = obj.GetComponentsInChildren<DamageCollider>();
				foreach (DamageCollider col in cols) {
					col.ResetToHitAgain();
				}

				return;
			}

		}

		throw new UnityException("!! CAN'T FIND " + owner.name + " OWNED DAMAGE COLLIDER THE NAMED " + colliderName);
	}
}

}