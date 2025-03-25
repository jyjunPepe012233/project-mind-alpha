using MinD.Runtime.Entity;
using MinD.Utility;
using MinD.Runtime.Utils;
using MinD.SO.Object;
using UnityEngine;

namespace MinD.Runtime.Object.Magics {

public class LghtOfDrgnSlyngProjectile : MonoBehaviour {

	public DamageCollider damageCollider;

	private Vector3 direction;
	private float speed;
	private float distance;

	private float currentDistance;

	public void Shoot(Vector3 origin, Vector3 direction, float speed, float distance, DamageData damageData, BaseEntity caster) {

		transform.position = origin;
		this.direction = direction;
		this.speed = speed;
		this.distance = distance;

		currentDistance = 0;

		damageCollider.soData = damageData;
		
		PhysicUtility.IgnoreCollisionUtil(caster, GetComponent<Collider>());
	}


	void FixedUpdate() {

		transform.position += direction * (speed * Time.fixedDeltaTime);
		currentDistance += speed * Time.fixedDeltaTime;

		if (currentDistance > distance) {
			Destroy(gameObject); // AFTER, SWITCH TO DESTROY WITH OBJECT POOLING
		}

	}

}

}