using MinD.Runtime.Entity;
using MinD.Runtime.Object.Utils;
using MinD.Runtime.Utils;
using MinD.Utility;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.TextCore.Text;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class BaalTinyLaser : MonoBehaviour
{ 
	[SerializeField] private LifeTimeObject explosionObject;
	[SerializeField] private float speed = 30;
	
	private Rigidbody rigidbody;
	private Collider collider;

	private bool isExploded;

	public void OnEnable()
	{
		collider = GetComponent<Collider>();
		rigidbody = GetComponent<Rigidbody>();

		collider.enabled = false;
	}
	
	public void Init(BaseEntity owner)
	{
		collider.enabled = true;

		foreach (var col in GetComponentsInChildren<Collider>(true))
		{
			PhysicUtility.IgnoreCollisionUtil(owner, col);
		}
	}

	private void Update()
	{
		if (!isExploded)
		{
			rigidbody.velocity = transform.forward * speed;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		isExploded = true;
		
		rigidbody.isKinematic = true;
		collider.enabled = false;

		explosionObject.gameObject.SetActive(true);
	}
}
