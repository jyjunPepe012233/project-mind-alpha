using MinD.Runtime.Entity;
using UnityEngine;

public class BaalDarkTorrentMain : MonoBehaviour
{ 
	[SerializeField] private GameObject laserPrefab;
	[SerializeField] private float shootingRateOverTime;
	[SerializeField] private float lifeTime;
	
	private float lifeTimer;
	private float shootingStack;

	private BaseEntity owner;

	public void Init(BaseEntity owner)
	{
		this.owner = owner;
		shootingStack = 1;
	}

	private void Update()
	{
		lifeTimer += Time.deltaTime;

		shootingStack += Time.deltaTime * shootingRateOverTime;
		while (shootingStack > 1)
		{
			shootingStack -= 1;
			Shoot();
		}

		if (lifeTimer > lifeTime)
		{
			Destroy(gameObject);
		}
	}

	private void Shoot()
	{
		BaalTinyLaser obj = Instantiate(laserPrefab, transform.position, transform.rotation).GetComponent<BaalTinyLaser>();
		obj.Init(owner);
	}
}
