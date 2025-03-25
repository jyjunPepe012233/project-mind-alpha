using MinD.Runtime.Entity;
using UnityEngine;

namespace MinD.SO.Item.Items {

[CreateAssetMenu(fileName = "Mp Fill Item", menuName = "MinD/Item/Items/Equipment/Tool/Mp Fill Item")]
public class MpFillItem : Tool {
	
	[Header("[ Hp Fill Item ]")]
	[SerializeField] private int mpFillAmount;
	[SerializeField] private GameObject healVfxPrefab;



	public override void OnEquip(Player owner) {
	}

	public override void Execute(Player owner) {
	}

	public override void OnUnequip(Player owner) {
	}

	
	
	public override void OnUse(Player owner) {

		owner.CurMp += mpFillAmount;

		if (healVfxPrefab != null) {
			GameObject healVfx = Instantiate(healVfxPrefab, owner.transform, true);
			healVfx.transform.position = owner.transform.position;
		}
	}
}

}