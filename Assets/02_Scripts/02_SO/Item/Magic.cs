using MinD.Runtime.Entity;
using UnityEngine;

namespace MinD.SO.Item {

public abstract class Magic : Item {
	
	[Space(50), Header("[ Magic Status ]")]
	[Range(1, 3)] public int memoryCost = 1;
	public int mpCost;
	public int staminaCost;

	[Header("[ Optional Status ]")]
	public int mpCostDuring;
	public int staminaCostDuring;

	
	[HideInInspector] public Player castPlayer;

	public bool isInputReleased;

	

	// WRITE THE CODE WHEN RUNNING MAGIC IS STARTED (ONCE)
	public abstract void OnUse();

	// WRITE THE CODE WHEN RUNNING IN EVERY FRAME
	public abstract void Tick();

	// WRITE THE CODE WHEN RUNNING IF PLAYER'S USE MAGIC INPUT IS RELEASED (ONCE)
	public abstract void OnReleaseInput();

	// WRITE THE CODE WHEN RUNNING IF MAGIC WAS CANCELED BY FORCE (ONCE)
	public abstract void OnCancel();

	// WRITE THE CODE WHEN RUNNING MAGIC IS PERFECTLY END
	// AND IF YOU WANT TO END THE MAGIC, USE EndCurrentMagic FUNCTION IN PLAYER COMBAT HANDLER
	// THAT FUNCTION WILL CALL THIS FUNCTION ONCE
	public abstract void OnExit();
	
	



	public virtual void InstantiateWarmupFX() {
	}

	public virtual void OnSuccessfullyCast() {
	}

	public virtual void ComboStandbyStart() {
	}
	
	public virtual void ComboStandbyEnd() {
	}

	public virtual void UseComboAttack() {
	}
	
	public virtual void OnDamageCollider() {
	}
	public virtual void OffDamageCollider() {
	}
}

}