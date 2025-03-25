using MinD.Structs;
using UnityEngine;
using UnityEngine.Serialization;

namespace MinD.SO.EnemySO {

[CreateAssetMenu(menuName = "MinD/Enemy SO/Enemy Attribute", fileName = "Enemy Attribute")]
public class EnemyAttribute : ScriptableObject {

	[Header("[ Status ]")]
	public int maxHp; 
	public float moveSpeed = 1;
	public float angularSpeed = 180;

	[Space(10)]
	public DamageNegation damageNegation;
	[Space(10)]
	[Range(0, 100)] public int poiseBreakResistance;

	[Header("[ Setting ]")]
	public float corpseFadeTime = 2;

}

}