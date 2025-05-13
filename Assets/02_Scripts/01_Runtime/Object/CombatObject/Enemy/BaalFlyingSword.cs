using MinD.Runtime.Entity;
using MinD.Runtime.Object.Utils;
using MinD.Runtime.Utils;
using MinD.Utility;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.TextCore.Text;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class BaalFlyingSword : MonoBehaviour
{ 
	[SerializeField] private LifeTimeObject explosionObject;
	[SerializeField] private float shootingDelay;
	[SerializeField] private float initialSpeed = 3;
	[SerializeField] private float acceleration = 2;
	[SerializeField] private float angularSpeed = 10;
	[SerializeField] private float lifeTime = 5;
	
	private Rigidbody rigidbody;
	private Collider collider;

	private BaseEntity target;

	private bool isFlying;
	private bool isExploded;
	private float lifeTimer;
	private float currentSpeed;

	public void OnEnable()
	{
		collider = GetComponent<Collider>();
		rigidbody = GetComponent<Rigidbody>();

		rigidbody.isKinematic = true;
		collider.enabled = false;
	}
	
	public void Init(BaseEntity owner, BaseEntity target)
	{
		this.target = target;
		
		transform.forward = target.targetOptions[0].position - transform.position;	

		foreach (var col in GetComponentsInChildren<Collider>(true))
		{
			PhysicUtility.IgnoreCollisionUtil(owner, col);
		}
	}

	private void Shoot()
	{
		isFlying = true;
		rigidbody.isKinematic = false;
		collider.enabled = true;
		currentSpeed = initialSpeed;
	}

	private void Update()
	{
		lifeTimer += Time.deltaTime;
		if (!isFlying && lifeTimer > shootingDelay)
		{
			Shoot();
		}
		
		if (!isExploded && isFlying)
		{
			if (lifeTimer > lifeTime)
			{
				Explode();
				return;
			}

			currentSpeed += acceleration * Time.deltaTime;
			
			Vector3 targetDirection = target.targetOptions[0].position - transform.position;
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetDirection), Time.deltaTime * angularSpeed);
			rigidbody.velocity = transform.forward * currentSpeed;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		Explode();
	}

	private void Explode()
	{
		isExploded = true;
		
		rigidbody.isKinematic = true;
		collider.enabled = false;

		explosionObject.gameObject.SetActive(true);
	}
}
