using System.Collections;
using System.Collections.Generic;
using MinD.Runtime.Entity;
using UnityEngine;

public class SkeletonGuard : HumanoidEnemy
{
	[SerializeField] private GameObject swordTrailFlag;
	[SerializeField] private ParticleSystem swordTrail;
	
	private void Update()
	{
		base.Update();

		if (swordTrailFlag.activeInHierarchy && !swordTrail.isPlaying)
		{
			swordTrail.Play();
		}
		else if (!swordTrailFlag.activeInHierarchy && swordTrail.isPlaying)
		{
			swordTrail.Stop();
		}
	}
}
