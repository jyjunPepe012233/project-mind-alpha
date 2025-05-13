using System.Collections.Generic;
using System.Linq;
using MinD.Runtime.Entity;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MinD.SO.EnemySO {

public abstract class CombatStanceState : EnemyState {

	[SerializeField] private EnemyAttackAction[] attackActions;
	
	[Space(10)]
	[SerializeField] protected float exitCombatStanceRadius;
	
	

	protected AttackState AttemptAttackAction(Enemy self, EnemyAttackAction thisAttack) {

		if (thisAttack.isLimitedByHealth)
		{
			self.combat.reservedAttackQueue.Remove(thisAttack);
		}
		
		self.combat.attackActionRecoveryTimer = 0;
		((HumanoidEnemy)self).strafeTimer = 0;
		((HumanoidEnemy)self).isDashingToTarget = false;

		self.combat.latestAttack = thisAttack;

		// DECIDE WHETHER TO USE COMBO ON NEXT ATTACK
		self.combat.willPerformCombo = Random.value < thisAttack.chanceToCombo && thisAttack.canPerformCombo && thisAttack.comboAttack != null;
		self.combat.comboAttack = self.combat.willPerformCombo ? thisAttack.comboAttack : null;

		return self.attackState;
	}
	
	protected EnemyAttackAction GetAttackActionRandomly(Enemy self) {
		
		bool CanIUseThisAttack(EnemyAttackAction action) {

			if (action.minAngle > self.combat.AngleToTarget() ||
			    action.maxAngle < self.combat.AngleToTarget()) {
				return false;
			}
			if (action.minDistance > self.combat.DistanceToTarget() ||
			    action.maxDistance < self.combat.DistanceToTarget()) {
				return false;
			}
			
			// TRYING REPEAT ACTION
			if (!action.canRepeatAction && action == self.combat.latestAttack) {
				return false;
			}
			
			if (action.isLimitedByHealth && !self.combat.reservedAttackQueue.Contains(action))
			{
				return false;
			}

			return true;
		}


		// IF NEXT COMBO ATTACK IS RESERVED
		if (self.combat.willPerformCombo) {
			
			if (CanIUseThisAttack(self.combat.comboAttack)) {
				return self.combat.comboAttack;
			}
			return null;
 		}
		
		
		// IF NO COMBO IS RESERVED
		EnemyAttackAction[] availableAttacks = attackActions.Where(CanIUseThisAttack).ToArray();
		if (availableAttacks.Length == 0) {
			return null;
		}
		

		float totalWeight = availableAttacks.Sum(i => i.actionWeight);
		float randomPointOnWeight = Random.Range(0, totalWeight);
		
		float w = 0;
		for (int i = 0; i < availableAttacks.Length; i++) {
			
			w += availableAttacks[i].actionWeight;
			
			if (w > randomPointOnWeight) {
				return availableAttacks[i];
			}
		}
		
		throw new UnityException();
	}

	public List<EnemyAttackAction> GetLiftedAttacksByHealthLimit(Enemy self, List<EnemyAttackAction> usedLimitedAttacks, int currentHealth)
	{
		List<EnemyAttackAction> result = new(2);
		
		foreach (var attack in attackActions.Where(a => a.isLimitedByHealth))
		{
			if (attack.liftingPercents == null)
			{
				continue;
			}
			
			// usedLimitedAttack에 저장된 So의 개수를 통해 해제될 수 있는 공격인지 판단
			// 그 뒤 HP와 비교하여 반환 배열에 추가
			int currentThresholdNumber = usedLimitedAttacks.Count(a => a == attack);
			if (currentThresholdNumber < attack.liftingPercents.Length)
			{
				if ((float)currentHealth / self.attribute.MaxHp < attack.liftingPercents[currentThresholdNumber])
				{
					result.Add(attack);
				}
			}
		}

		return result;
	}
	
}

}