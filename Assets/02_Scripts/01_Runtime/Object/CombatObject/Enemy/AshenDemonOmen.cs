using System;
using MinD.Runtime.Entity;
using MinD.Utility;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.Serialization;

namespace MinD.Runtime.Object
{
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(Collider))]
	public class AshenDemonOmen : MonoBehaviour
	{
		private Rigidbody rigidbody;
		private BaseEntity target;
		private float currentSpeed = 1;
		private float elapsedLifeTime;
		private bool isExploded;
		
		[SerializeField] private ParticleSystem explodeParticle;
		[SerializeField] private GameObject damageCollider; 
		[SerializeField] private GameObject explosionLifeTimer; // Object has Life Time Object component

		[Header("[ STATUS ]")]
		[SerializeField] private float angularSpeed = 60;
		[SerializeField] private float acceleration = 0.4f;
		[SerializeField] private float maxSpeed = 4 ;
		[SerializeField] private float lifeTime = 8;
		

		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody>();
		}

		public void Setup(BaseEntity owner, BaseEntity target)
		{
			this.target = target;
			PhysicUtility.IgnoreCollisionUtil(owner, GetComponent<Collider>());
		}
		
		public void Update()
		{
			if (!isExploded)
			{
				Vector3 targetDirection;
				if (target != null)
				{
					targetDirection = target.targetOptions[0].position - transform.position;

				}
				else
				{
					targetDirection = transform.forward;
				}

				transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetDirection), angularSpeed * Time.deltaTime);

				currentSpeed = Mathf.Min(currentSpeed + (acceleration * Time.deltaTime), maxSpeed);
				rigidbody.velocity = transform.forward * currentSpeed;
				
				elapsedLifeTime += Time.deltaTime;
				if (elapsedLifeTime > lifeTime && !isExploded)
				{
					Explode();
				}
			}
		}

		public void OnTriggerEnter(Collider other) => Explode();

		private void Explode()
		{
			isExploded = true;
			rigidbody.isKinematic = true; // stop flying
			explodeParticle.Play();
			damageCollider.SetActive(true);
			explosionLifeTimer.SetActive(true);
		}

		public void OnDrawGizmos()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawSphere(transform.position, 0.3f);
			#if UNITY_EDITOR
			Handles.color = Color.yellow;
			Handles.ArrowHandleCap(0, transform.position, transform.rotation, 1, EventType.Repaint);
			#endif
		}
	}

}