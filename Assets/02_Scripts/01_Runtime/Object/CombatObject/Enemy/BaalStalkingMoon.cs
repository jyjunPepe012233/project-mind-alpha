using System.Collections;
using MinD.Runtime.Entity;
using MinD.Runtime.Object.Utils;
using MinD.Runtime.Utils;
using MinD.Utility;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class BaalStalkingMoon : MonoBehaviour
{
	[Header("[ Pre-Shooting ]")]
	[SerializeField] private float floatingSpeed = 3.5f;
	[SerializeField] private float floatingHeight = 2;
	[SerializeField] private float shootingDelay = 1;

	[Header("[ Flying ]")]
	[SerializeField] private float acceleration = 1;
	[SerializeField] private float maxSpeed = 5;
	[SerializeField] private float angularSpeed = 60;
	[SerializeField] private ParticleSystem shootingParticle;

	[Space(15)]
	[SerializeField] private float maxLifeTime;
	[SerializeField] private LifeTimeObject explosionObject;
	
	private Rigidbody rigidbody;
	private Collider collider;

	private BaseEntity target;

	private float lifeTimer;
	private bool isFlying;
	private float currentSpeed;

	private Vector3 instantiatedPosition;
	
	public void Init(BaseEntity owner, BaseEntity target)
	{
		rigidbody = GetComponent<Rigidbody>();
		collider = GetComponent<Collider>();
		
		PhysicUtility.IgnoreCollisionUtil(owner, collider);
		this.target = target;

		collider.enabled = false;
		explosionObject.gameObject.SetActive(false);
		
		instantiatedPosition = transform.position;
		StartCoroutine(LifeCycleCoroutine());
	}

	private void Update()
	{
		lifeTimer += Time.deltaTime;

		if (lifeTimer > maxLifeTime)
		{
			Explode();
		}

		if (isFlying)
		{
			currentSpeed += Mathf.Min(acceleration * Time.deltaTime, maxSpeed);
			
			Vector3 targetDirection = target.targetOptions[0].position - transform.position;
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetDirection), Time.deltaTime * angularSpeed);
			rigidbody.velocity = transform.forward * currentSpeed;
		}
	}

	private IEnumerator LifeCycleCoroutine()
	{
		Vector3 floatingTargetPosition = instantiatedPosition + Vector3.up * floatingHeight;
		
		while (Vector3.Distance(transform.position, floatingTargetPosition) > 0.02f)
		{
			transform.position = Vector3.Lerp(transform.position, floatingTargetPosition, Time.deltaTime * floatingSpeed);
			yield return null;
		}

		yield return new WaitForSeconds(shootingDelay);

		if (shootingParticle)
		{
			shootingParticle.Play();
		}
		collider.enabled = true;
		isFlying = true;
	}

	private void OnTriggerEnter(Collider other)
	{
		Explode();
	}

	private void Explode()
	{
		explosionObject.gameObject.SetActive(true);
		rigidbody.isKinematic = true;
		isFlying = false;
	}
}

