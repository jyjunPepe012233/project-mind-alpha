using MinD.Runtime.Entity;
using MinD.Runtime.Object.Utils;
using MinD.Runtime.Utils;
using MinD.Utility;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.Serialization;

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
