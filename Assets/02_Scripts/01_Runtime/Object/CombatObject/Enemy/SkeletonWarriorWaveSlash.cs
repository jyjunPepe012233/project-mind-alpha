using System.Collections;
using MinD.Runtime.Utils;
using MinD.SO.Object;
using UnityEngine;

namespace MinD.Runtime.Object {

public class SkeletonWarriorWaveSlash : MonoBehaviour {

	private Vector3 velocity;
	[SerializeField] private DamageCollider dCollider;
	[SerializeField] private ParticleSystem pSystem;
	
	private Rigidbody rigidbody;
	private float flightTimer;

	private bool isDestroying;
	
	private void OnEnable() {
		rigidbody = GetComponent<Rigidbody>();
	}
	
	public void Shoot(Vector3 velocity, DamageData damageData) {
		this.velocity = velocity;
		dCollider.soData = damageData;
	}
	
	private void Update() {
		
		if (flightTimer > 1f && !isDestroying) {
			isDestroying = true;
			StartCoroutine(DestroyCoroutine());
			
		} else {
			flightTimer += Time.deltaTime;
			rigidbody.velocity = velocity;
		}
	}

	private void OnTriggerEnter(Collider other) {
		StartCoroutine(DestroyCoroutine());
	}

	private IEnumerator DestroyCoroutine() {
		
		float timer = 0;
		
		pSystem.Stop();
		dCollider.enabled = false;
		rigidbody.isKinematic = true;
		

		while (true) {
			timer += Time.deltaTime;
			yield return null;
			
			if (timer > 2) {
				Destroy(gameObject);
			}
		}
	}

}

}