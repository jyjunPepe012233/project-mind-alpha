using MinD.Runtime.Entity;
using MinD.Runtime.Object.Utils;
using MinD.Runtime.Utils;
using UnityEngine;

[RequireComponent(typeof(LifeTimeObject))]
public class ForgeGolemWhirlwindMain : MonoBehaviour
{
	[SerializeField] private GameObject projectilePrefab;
	[SerializeField] private float minThrowSpeed;
	[SerializeField] private float maxThrowSpeed;
	[SerializeField] private float rateOverTime;
	[SerializeField] private Vector3 shootPositionOffset;

	private float timeStack;
	private BaseEntity owner;

	public void SetOwner(BaseEntity owner)
	{
		this.owner = owner;
	}

	private void Update()
	{
		timeStack += Time.deltaTime;

		float shootRate = 1 / rateOverTime;
		if (timeStack > shootRate)
		{
			while (timeStack > shootRate)
			{
				timeStack -= shootRate;
				ShootProjectile();
			}
		}
	}

	private void ShootProjectile()
	{
		Vector2 shoot2DDirection = Random.insideUnitCircle;
		Vector3 shootDirection = new Vector3(shoot2DDirection.x, 0, shoot2DDirection.y);
		var projectile = Instantiate(projectilePrefab, transform.position + shootPositionOffset, Quaternion.identity).GetComponent<ForgeGolemWhirlwindProjectile>();
		projectile.Init(owner, Random.Range(minThrowSpeed, maxThrowSpeed), shootDirection);
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawSphere(transform.position + shootPositionOffset, 0.3f);
	}
}
