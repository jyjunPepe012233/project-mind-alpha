using System;
using System.Collections;
using System.Linq;
using MinD.Runtime.DataBase;
using MinD.Runtime.Managers;
using MinD.Utility;
using MinD.SO.Item;
using UnityEditor;
using UnityEngine;
using Tool = MinD.SO.Item.Tool;


namespace MinD.Runtime.Entity {

public class PlayerCombatHandler : EntityOwnedHandler {
	
	// LOCKING ON ENTITY
	public BaseEntity target;
	
	[Header("[ Flags ]")]
	public bool usingMagic;
	public bool usingDefenseMagic;
	public bool isParrying;
	
	
	[HideInInspector] public Magic currentCastingMagic;
	
	

	public void HandleAllCombatAction() {

		if (owner.isDeath) {
			return;
		}

		HandleUsingMagic();
		HandleUsingTool();
		HandleDefenseMagic();
	}

	private void HandleUsingMagic() {

		if (currentCastingMagic != null) {
			currentCastingMagic.Tick();
		}

		
		
		if (((Player)owner).inventory.weaponSlot == null)
			return;
		


		if (PlayerInputManager.Instance.useMagicInput) {

			// CHECK BASIC FLAGS
			if (usingMagic || ((Player)owner).isPerformingAction) {
				return;
			}


			Magic useMagic = ((Player)owner).inventory.magicSlots[((Player)owner).inventory.currentMagicSlot];

			if (useMagic == null) {
				return;
			}

			// CANCEL IF PLAYER HASN'T ENOUGH MP OR STAMINA
			if (((Player)owner).CurMp < useMagic.mpCost) {
				return;
			}

			if (((Player)owner).CurStamina < useMagic.staminaCost) {
				return;
			}


			usingMagic = true;

			((Player)owner).CurMp -= useMagic.mpCost;
			((Player)owner).CurStamina -= useMagic.staminaCost;

			useMagic.castPlayer = ((Player)owner);

			useMagic.OnUse();
			currentCastingMagic = useMagic;



		} else if (currentCastingMagic != null && !currentCastingMagic.isInputReleased) {

			// IF INPUT IS NULL AND DURING CASTING => USE MAGIC INPUT IS END
			currentCastingMagic.OnReleaseInput();
			currentCastingMagic.isInputReleased = true;

		}
	}

	private void HandleUsingTool() {

		// HANDLE TOOL USING COOL TIME
		Tool[] tools = ((Player)owner).inventory.toolSlots.Where(i => i != null && i.remainingCoolTime > 0).ToArray();
		for (int i = 0; i < tools.Length; i++) {
			tools[i].remainingCoolTime = Mathf.Max((tools[i].remainingCoolTime - Time.deltaTime), 0); // SET MINIMUM TO 0
		}
		
		
		// USING TOOL
		if (PlayerInputManager.Instance.useToolInput) {

			if (((Player)owner).isPerformingAction) {
				return;
			}
			if (!((Player)owner).isGrounded) {
				return;
			}

			Tool useTool = ((Player)owner).inventory.toolSlots[((Player)owner).inventory.currentToolSlot];

			if (useTool == null)
			{
				return;
			}
			
			if (useTool.remainingCoolTime > 0) {
				return;
			}
			
			useTool.OnUse(((Player)owner));
			

			foreach (var __tool in ((Player)owner).inventory.toolSlots)
			{
				if (__tool == null)
				{
					continue;
				}
				Debug.Log(__tool);
				if (__tool.itemTypeNumber == useTool.itemTypeNumber)
				{
					__tool.remainingCoolTime = useTool.usingCoolTime;
				}
			}
			
			var quickSlot = FindObjectOfType<QuickSlot>();
			quickSlot.StartCooldownOnToolSlot(useTool.usingCoolTime);
			
		}
	}

	private void HandleDefenseMagic() {
		
		if (!usingDefenseMagic && PlayerInputManager.Instance.defenseMagicInput) {
			
			if (((Player)owner).isPerformingAction) {
				return;
			}
			if (!((Player)owner).isGrounded) {
				return;
			}
			
			ActivateDefenseMagic();
		}
		
		
		
		if (usingDefenseMagic && !PlayerInputManager.Instance.defenseMagicInput) { 
			ReleaseDefenseMagic();
		}
		
	}

	private void ActivateDefenseMagic() {
		
		usingDefenseMagic = true; // IF THIS FLAG IS ENABLED, DAMAGE WILL CALCULATE SPECIAL
		PhysicUtility.SetActiveChildrenColliders(transform, false, WorldUtility.damageableLayerMask, false);
		
		// PLAY DEFENSE ANIMATION (LOOPING)
		((Player)owner).animation.PlayTargetAction("Defense_Action_Start", 0.2f, true, true, true, false);
		
		// ACTIVE DEFENSE MAGIC
		((Player)owner).defenseMagic.EnableShield();
		

		
		
		
	}
	public void ReleaseDefenseMagic(bool playAnimation = true, bool parrying = true) {

		usingDefenseMagic = false;
		PhysicUtility.SetActiveChildrenColliders(transform, true, WorldUtility.damageableLayerMask, false);


		
		if (playAnimation) {
			if (parrying) {
				((Player)owner).animation.PlayTargetAction("Defense_Action_Parry", 0.2f, true, true, true, false);
			} else {
				((Player)owner).animation.PlayTargetAction("Default Movement", 0.3f, false);
			}
		}

		// DISABLE DEFENSE MAGIC
		((Player)owner).defenseMagic.DisableShield();
	}

	public void StartParrying() => isParrying = true;
	public void EndParrying() => isParrying = false;
	
	
	
	public void ExitCurrentMagic() {

		if (currentCastingMagic == null) {
			return;
		}

		usingMagic = false;

		currentCastingMagic.OnExit();
		currentCastingMagic = null;
	}
	
	public void CancelMagicOnGetHit() {

		// CANCEL MAGIC
		if (currentCastingMagic != null) {
			currentCastingMagic.OnCancel();
		}
		
	}



	public void OnInstantiateWarmUpFx() {
		if (currentCastingMagic != null) {
			currentCastingMagic.InstantiateWarmupFX();
		}
	}
	public void OnSuccessfullyCast() {
		if (currentCastingMagic != null) {
			currentCastingMagic.OnSuccessfullyCast();
		}
	}
	public void OnDamageCollider() {
		if (currentCastingMagic != null) {
			currentCastingMagic.OnDamageCollider();
		}
	}public void OffDamageCollider() {
		if (currentCastingMagic != null) {
			currentCastingMagic.OffDamageCollider();
		}
	}
	public void ComboStandbyStart() {
		if (currentCastingMagic != null) {
			currentCastingMagic.ComboStandbyStart();
		}
	}

	public void ComboStandbyEnd()
	{
		if (currentCastingMagic != null)
		{
			currentCastingMagic.ComboStandbyEnd();
		}
	}

}

}