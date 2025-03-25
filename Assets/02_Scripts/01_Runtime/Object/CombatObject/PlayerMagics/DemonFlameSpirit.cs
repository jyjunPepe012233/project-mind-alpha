using System.Collections;
using MinD.Runtime.Entity;
using MinD.Utility;
using MinD.Runtime.Utils;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace MinD.Runtime.Object.Magics {

public class DemonFlameSpirit : MonoBehaviour {

	public Transform targetOption;
	[Space(10)]
	[SerializeField] private ParticleSystem flightFx; 
	[SerializeField] private ParticleSystem explosionFx;
	[Space(10)]
	[SerializeField] private DamageCollider explosionDamageCollider;

	
	private Vector3 startPosition;
	private Vector3 readyPosition;

	private Vector3 targetDirx;

	private Rigidbody rigidbody;
	private Collider collider;

	private Coroutine currentFlightCoroutine;

	private bool isExploded;


	private void OnEnable() {

		rigidbody = GetComponent<Rigidbody>();
		collider = GetComponent<Collider>();

	}

	public void Shoot(BaseEntity owner, BaseEntity target) {
		
		PhysicUtility.IgnoreCollisionUtil(owner, GetComponent<Collider>());
		explosionDamageCollider.blackList.Add(owner);

		// PREVENT NULL REFERENCE ERROR
		if (target != null) {
			targetOption = target.targetOptions[0];
		} else {
			targetOption = null;
		}

		currentFlightCoroutine = StartCoroutine(ShootCoroutine());
	}

	private IEnumerator ShootCoroutine() {

		startPosition = transform.position;
		readyPosition = transform.position + Random.onUnitSphere;

		transform.forward = readyPosition - transform.position;


		collider.enabled = false;


		float elapsedTime = 0;

		// READY FOR 1.5 SECOND
		while (elapsedTime < 1.5f) {

			transform.position = Vector3.Lerp(startPosition, readyPosition, elapsedTime / 1.5f);
			transform.position += transform.up * Mathf.Sin(Mathf.PI * elapsedTime / 1.5f);

			elapsedTime += Time.deltaTime;
			yield return null;
		}

		collider.enabled = true;
		float speed = 4;

		// SET FORWARD TO TARGET DIRECTION
		if (targetOption != null) {
			targetDirx = (targetOption.transform.position - transform.position).normalized;
		} else {
			targetDirx = (transform.forward);
		}

		transform.forward = targetDirx;

		// SHOOT
		while (true) {

			speed += Time.deltaTime * 5;


			if (targetOption != null) {
				targetDirx = (targetOption.transform.position - transform.position).normalized;
			}

			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetDirx), Time.deltaTime * 45);

			rigidbody.velocity = transform.forward * speed;


			// SET LIFETIME
			if (elapsedTime > 6) {
				StartCoroutine(Explode());
			}

			elapsedTime += Time.deltaTime;
			yield return null;
		}

	}

	private IEnumerator Explode() {
		
		explosionDamageCollider.gameObject.SetActive(true);

		flightFx.Stop();
		explosionFx.Play();

		StopCoroutine(currentFlightCoroutine);
		rigidbody.velocity = Vector3.zero;

		isExploded = true;
		


		// SET LIFETIME
		float elapsedTime = 0;
		while (true) {
			
			explosionDamageCollider.gameObject.SetActive(true);

			if (elapsedTime > 0.3f) {
				explosionDamageCollider.gameObject.SetActive(false);
			}
			
			if (elapsedTime > explosionFx.main.duration)
				break;

			elapsedTime += Time.deltaTime;
			yield return null;
		}

		Destroy(gameObject);
	}

	private void OnTriggerEnter(Collider other) {

		// WASN'T EXPLODE
		if (!isExploded) {
			StartCoroutine(Explode());
		}

	}
}

}