using MinD.Runtime.Entity;
using MinD.Runtime.Entity.Enemies;
using MinD.Runtime.Utils;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class BaalEclipseController : MonoBehaviour
{
	[Header("[ Moon ]")]
	[SerializeField] private GameObject moon;
	[SerializeField] private float moonActiveTime = 1.5f;
	[Header("[ Beam ]")]
	[SerializeField] private DamageCollider[] damageColliders;

	[Header("[ LifeCycle ]")]
	[SerializeField] private AnimationCurve rotationSpeedOverLifeTime;
	[SerializeField] private float maximumRotationSpeed;
	[SerializeField] private float angleBound; 
	[SerializeField] private float rotationTime;

	private Baal owner;
	private BaseEntity target;

	private float lifeTimer;
	private bool isStopped;

	public void Init(Baal owner, BaseEntity target)
	{
		this.owner = owner;
		this.target = target;
	}
	
	private void Update()
	{
		lifeTimer += Time.deltaTime;

		if (moon.activeInHierarchy)
		{
			HandleMoon();
		}
		else
		{
			if (lifeTimer >= moonActiveTime)
			{
				EnableMoon();
			}
		}

		if (lifeTimer > moonActiveTime + rotationTime & !isStopped)
		{
			isStopped = true;
			owner.animation.PlayTargetAnimation("Eclipse_End", 0.02f, true, true);
		}

		foreach (var dc in damageColliders)
		{
			dc.ResetToHitAgain();
		}
	}

	private void EnableMoon()
	{
		moon.SetActive(true);
		Vector3 angle = moon.transform.eulerAngles;
		angle.x = angleBound;
		moon.transform.eulerAngles = angle;
	}

	private void HandleMoon()
	{
		Vector3 angle = moon.transform.eulerAngles;
		angle.y += maximumRotationSpeed * rotationSpeedOverLifeTime.Evaluate((lifeTimer - moonActiveTime) / rotationTime) * Time.deltaTime;
		angle.x -= angleBound * 2 * Time.deltaTime / rotationTime;
		moon.transform.eulerAngles = angle;
	}
	
}
