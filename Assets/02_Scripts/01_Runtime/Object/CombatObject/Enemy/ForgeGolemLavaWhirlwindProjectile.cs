using MinD.Runtime.Entity;
using MinD.Runtime.Object.Utils;
using MinD.Runtime.Utils;
using MinD.Utility;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class ForgeGolemWhirlwindProjectile : MonoBehaviour
{
	[SerializeField] private LifeTimeObject explosionObject;
	
	private Rigidbody rigidbody;
	private Collider collider;
	
	public void Init(BaseEntity owner, float throwSpeed, Vector3 throwDirection)
	{
		collider = GetComponent<Collider>();
		rigidbody = GetComponent<Rigidbody>();
		rigidbody.velocity = throwDirection.normalized * throwSpeed;

		foreach (var col in GetComponentsInChildren<Collider>(true))
		{
			PhysicUtility.IgnoreCollisionUtil(owner, col);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		rigidbody.isKinematic = true;
		collider.enabled = false;

		explosionObject.gameObject.SetActive(true);
	}
}
