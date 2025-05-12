using MinD.Runtime.Entity;
using UnityEngine;

namespace MinD.SO.EnemySO {

[CreateAssetMenu(menuName = "MinD/Enemy SO/FSM/Idle/Golem", fileName = "(Enemy)_Idle")]
public class GolemIdleState : HumanoidIdleState {

	public override EnemyState Tick(Enemy self) {
		
		self.animation.LerpMoveDirectionParameter(0, 0);
		
		self.currentTarget = self.combat.FindTargetBySight(detectRadius, absoluteDetectRadius, detectAngle);
		
		if (self.currentTarget == null) {
			return this;
			
		} else if (Vector3.Distance(self.transform.position, self.currentTarget.transform.position) < absoluteDetectRadius)
		{
			GolemEnemy selfG = self as GolemEnemy;
			if (!selfG.IsRised)
			{
				selfG?.Rise();
			}
			return self.pursueTargetState;
		}
		else
		{
			return this;
		}
	}
	
}

}