using MinD.Runtime.DataBase;
using MinD.Runtime.Object.Magics;
using MinD.SO.Object;
using UnityEngine;

namespace MinD.SO.Item.Items {

[CreateAssetMenu(menuName = "MinD/Item/Items/Magics/Light Of DragonSlaying")]
public class LghtOfDrgnSlyng : Magic {
	// 멸룡의 빛

	[Header("[ Settings ]")]
	[SerializeField] private DamageData damageData;
	[SerializeField] private float damageTick;
	[Space(10)]
	[SerializeField] private GameObject magicObject;
	
	[Header("[ Flags ]")]
	public bool isBlasting;
	
	private LghtOfDrgnSlyngMainObj currentMagic;

	private float blastTimer;



	public override void OnUse() {

		// START CHARGING
		castPlayer.animation.PlayTargetAction("LghtOfDrgnSlyng_Start", true, true, false, false);


		// INSTANTIATE MAGIC OBJECT
		currentMagic = Instantiate(magicObject).GetComponent<LghtOfDrgnSlyngMainObj>();
		currentMagic.SetUp(this, damageData, damageTick);



		Vector3 pivot = castPlayer.targetOptions[0].position;

		// SET MAGIC POSITION AND DIRECTION
		if (castPlayer.isLockOn) {

			// POSITION
			Vector3 targetDirx = (castPlayer.camera.currentTargetOption.position - pivot).normalized;
			currentMagic.transform.position = pivot + Quaternion.LookRotation(targetDirx) * new Vector3(0, 0.5f, 2);


			// DIRECTION
			currentMagic.transform.forward = targetDirx;
			Vector3 angle = currentMagic.transform.eulerAngles;

			if (angle.x > 180) {
				angle.x = Mathf.Clamp(angle.x, 340, 360);
			} else {
				angle.x = Mathf.Clamp(angle.x, 0, 20);
			}
			currentMagic.transform.eulerAngles = angle;


		} else {
			currentMagic.transform.position = pivot + (castPlayer.transform.rotation * new Vector3(0, 0.5f, 2));
			currentMagic.transform.forward = castPlayer.transform.forward;
		}


		currentMagic.PlayWarmUpVfx();
	}

	public override void Tick() {

		// ON ANIMATION IS END, END THE MAGIC
		if (castPlayer.isPerformingAction == false) {
			castPlayer.combat.ExitCurrentMagic();
			return;
		}


		// INPUT IS RELEASED DURING BLASTING
		if (isBlasting && isInputReleased) {

			blastTimer += Time.deltaTime;

			if (blastTimer > 0.3f) {
				// MINIMUM DURATION OF BLASTING
				EndBlasting();
			}
		}

	}

	public override void OnReleaseInput() {
	}

	public override void OnCancel() {
		
		// DESTROY MAGIC
		Destroy(currentMagic.gameObject);
		
		// FORCE EXIT
		castPlayer.combat.ExitCurrentMagic();
	}

	public override void OnExit() {

		currentMagic = null;

		// RESET FLAG
		isInputReleased = false;
		isBlasting = false;
		blastTimer = 0;
	}
	



	public override void OnSuccessfullyCast() {
		// WILL CALlED THIS FUNCTION WHEN WARM-UP ANIMATION IS ENDED

		// CAUSE THIS METHOD CALLED BY ANIMATION EVENT IN LOOP ANIMATION EVERY TIME
		if (isBlasting)
			return;

		// SET FLAGS
		isBlasting = true;

		currentMagic.StartBlasting();
	}

	
	public void EndBlasting() {

		isBlasting = false;
		currentMagic.StartCoroutine(currentMagic.EndBlastingCoroutine(2f));
		
		castPlayer.animation.PlayTargetAction("LghtOfDrgnSlyng_End", true, true, false, false);
		// AND WILL CALL OnCastIsEnd METHOD VIA ANIMATION EVENT
	}
	
	public bool TryDrainMpAndStaminaDuringBlasting() {

		// IF PLAYER HASN'T ENOUGH STATS, RETURN FALSE
		if (castPlayer.CurMp < mpCostDuring ||
		    castPlayer.CurStamina < staminaCostDuring) {
			return false;
		}

		// SUCCESSFULLY DRAIN MP AND STAMINA, RETURN TRUE
		castPlayer.CurMp -= mpCostDuring;
		castPlayer.CurStamina -= staminaCostDuring;

		return true;
	}

}

}