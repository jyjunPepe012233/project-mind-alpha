using MinD.Runtime.Entity;
using MinD.Structs;
using UnityEngine;

namespace MinD.SO.StatusFX.Effects {

[CreateAssetMenu(fileName = "TakeHealthDamage", menuName = "MinD/Status Effect/Effects/TakeHealthDamage")]
public class TakeHealthDamage : InstantEffect {

	public Damage damage;
	public int poiseBreakDamage;
	
	public float attackAngle;

	
	
	
	public TakeHealthDamage(Damage damage, int poiseBreakDamage, float attackAngle) {
		this.damage = damage;
		this.poiseBreakDamage = poiseBreakDamage;
		this.attackAngle = attackAngle;
	}
	
	public static int GetCalculatedDamage(Damage damage_, DamageNegation negation_) {
		
		
		int finalDamage = 0;
		finalDamage += (int)((1 - negation_.physical) * damage_.physical);
		finalDamage += (int)((1 - negation_.magic) * damage_.magic);
		finalDamage += (int)((1 - negation_.fire) * damage_.fire);
		finalDamage += (int)((1 - negation_.frost) * damage_.frost);
		finalDamage += (int)((1 - negation_.lightning) * damage_.lightning);
		finalDamage += (int)((1 - negation_.holy) * damage_.holy);

		return Mathf.Max(finalDamage, 1); // MINIMUM OF DAMAGE IS 1
	}

	public static int GetPoiseBreakAmount(int poiseBreakDamage, int poiseBreakResistance) {

		float resistanceValue = (float)poiseBreakResistance / 100;


		// get minPoiseBreak(poise break amount when resistance is minimum)
		// get maxPoiseBreak(poise break amount when resistance is maximum)
		// and lerp two value as Lerp Factor is poise break resistance

		float minPoiseBreak = (poiseBreakDamage); // 0 to 100
		float maxPoiseBreak = (1.3f * poiseBreakDamage) - 50; // -50 to 80

		return Mathf.Clamp((int)(Mathf.LerpUnclamped(minPoiseBreak, maxPoiseBreak, resistanceValue)), 0, 100);
		// ACTUALLY, POISE BREAK RESISTANCE CAN OUT OF RANGE 0-100.
	}


	
	public override void OnInstantiate(BaseEntity victim) {
		
		victim.CurHp -= GetCalculatedDamage(damage, victim.GetComponent<BaseEntityAttributeHandler>().DamageNegation);

		victim.OnDamaged(this);
		victim.getHitAction.Invoke();
		
	}
}

}