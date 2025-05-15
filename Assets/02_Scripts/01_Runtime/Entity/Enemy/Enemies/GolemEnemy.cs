using MinD.SO.StatusFX.Effects;
using UnityEngine;	

namespace MinD.Runtime.Entity {

public abstract class GolemEnemy : HumanoidEnemy
{
	private bool isRised;
	public bool IsRised => isRised;
	private bool isPoiseBroked;
	private float poiseBreakTimer;
	private int currentPoiseBreakHp;

	[Header("[ Golem Attributes ]")]
	[SerializeField] private float poiseBreakDuration;
	[SerializeField] private int poiseBreakHp;

	protected override void Setup()
	{
		base.Setup();
		currentPoiseBreakHp = poiseBreakHp;
	}

	protected override void Update()
	{
		base.Update();

		if (isPoiseBroked)
		{
			poiseBreakTimer += Time.deltaTime;
			if (poiseBreakTimer > poiseBreakDuration)
			{
				isPoiseBroked = false;
				poiseBreakTimer = 0;
				animation.PlayTargetAnimation("Stun_End", 0.1f, true, true);
			}
		}
	}

	public override void OnDamaged(TakeHealthDamage damage)
	{
		base.OnDamaged(damage);
		
		currentPoiseBreakHp -= damage.poiseBreakDamage;
		
		if (currentPoiseBreakHp <= 0)
		{
			isPoiseBroked = true;
			currentPoiseBreakHp = poiseBreakHp;
			animation.PlayTargetAnimation("Stun_Start", 0.1f, true, true);
		}

		if (!isPerformingAction)
		{
			base.OnDamaged(damage);
		}
	}

	public void Rise()
	{
		isRised = true;
		animation.PlayTargetAnimation("Rise", 0.1f, true, true);
	}
}

}