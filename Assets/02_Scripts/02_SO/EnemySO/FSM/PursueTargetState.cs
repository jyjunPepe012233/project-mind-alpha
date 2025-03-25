using UnityEngine;

namespace MinD.SO.EnemySO {

//[CreateAssetMenu(menuName = "MinD/Enemy SO/FSM/Pursue Target/", fileName = "(Enemy)_PursueTarget")]
public abstract class PursueTargetState : EnemyState {
	
	[SerializeField] protected float giveUpDistance;
	[SerializeField] protected float enterCombatStanceRadius;
	
}

}