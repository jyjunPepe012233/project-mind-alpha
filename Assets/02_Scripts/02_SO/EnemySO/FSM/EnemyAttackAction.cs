using System;
using UnityEngine;

namespace MinD.SO.EnemySO {

[CreateAssetMenu(menuName = "MinD/Enemy SO/FSM/Attack Action", fileName = "EnemyType ActionName", order = int.MinValue)]
public class EnemyAttackAction : ScriptableObject {

	
	public float actionWeight;
	
	[Header("[ Action Setting ]")]
	public string motionStateName;
	public float actionRecoveryTime = 0.5f;
	[Space(10)]
	public bool rotateInAttackAction; 
	
	
	
	[Space(10), Header("[ Requires To Perform ]")]
	
	[Range(-180, 180)] public float minAngle = -35f;
	[Range(-180, 180)] public float maxAngle = 35f;
	public float minDistance = 0f;
	public float maxDistance = 2f;
	
	
	
	[Space(30), Header("[ After Performing ]")]
	
	public bool canRepeatAction = false;
	[Space(10)]
	public bool canPerformCombo = false;
	[Range(0, 1)] public float chanceToCombo = 0.5f;
	public EnemyAttackAction comboAttack;
	
}

}