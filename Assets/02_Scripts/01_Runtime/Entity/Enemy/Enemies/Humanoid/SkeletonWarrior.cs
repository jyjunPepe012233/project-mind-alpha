using MinD.Runtime.Object;
using MinD.Runtime.Utils;
using MinD.SO.Object;
using Unity.VisualScripting;
using UnityEngine;

namespace MinD.Runtime.Entity.Enemies {

public class SkeletonWarrior : HumanoidEnemy {

	private static readonly string OBJECTNAME_RunAttackObj = "SkeletonWarrior_RunAttack_Obj";
	private static readonly string OBJECTNAME_FireStorm = "SkeletonWarrior_FireStorm_Obj";
	private static readonly string OBJECTNAME_WaveSlash = "SkeletonWarrior_WaveSlash_Obj";

	
	[Header("[ Skeleton Warrior ]")]
	[SerializeField] private Transform fireStormReferenceTransform;
	[SerializeField] private Transform waveSlashReferenceTransform;
	[Space(15)]
	[SerializeField] private DamageData slashDamageDefault;
	[SerializeField] private DamageData slashDamageFinal;

	




	public void SummonRunAttackObj() {
		var obj = utility.InstantiatePrefab(OBJECTNAME_RunAttackObj);
		
		obj.transform.position = transform.position;
		obj.transform.rotation = transform.rotation;

		obj.GetComponentInChildren<DamageCollider>().blackList.Add(this);
	}
	
	public void SummonFireStorm() {
		var obj = utility.InstantiatePrefab(OBJECTNAME_FireStorm);
		
		obj.transform.position = fireStormReferenceTransform.position;
		obj.transform.rotation = fireStormReferenceTransform.rotation;

		obj.GetComponentInChildren<DamageCollider>().blackList.Add(this);
	}

	public void SummonWaveSlash(AnimationEvent parameter) {
		float slashAngleZ = parameter.floatParameter;
		bool isFinalAttack = parameter.intParameter != 0;
		
		var obj = utility.InstantiatePrefab(OBJECTNAME_WaveSlash);
		
		obj.transform.position = fireStormReferenceTransform.position;
		
		// SET SLASH ANGLE
		Vector3 angle = fireStormReferenceTransform.eulerAngles;
		angle.z += slashAngleZ;
		obj.transform.eulerAngles = angle;

		// INCREASE SCALE WHEN THIS iS FINAL ATTACK
		if (isFinalAttack) {
			obj.transform.localScale *= 2f;
		}

		obj.GetComponent<SkeletonWarriorWaveSlash>().Shoot(transform.forward * 10, isFinalAttack ? slashDamageFinal : slashDamageDefault);
		obj.GetComponent<DamageCollider>().blackList.Add(this);
	}
}

}