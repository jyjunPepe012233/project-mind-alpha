	using MinD.Runtime.Entity;
	using MinD.Runtime.Managers;
	using UnityEngine;

namespace MinD.SO.Item.Items {

[CreateAssetMenu(fileName = "Hp Fill Item", menuName = "MinD/Item/Items/Equipment/Tool/Hp Fill Item")]
public class HpFillItem : Tool {
	
	[Header("[ Hp Fill Item ]")]
	[SerializeField] private int hpFillAmount;
	[SerializeField] private GameObject healVfxPrefab;



	public override void OnEquip(Player owner) {
	}

	public override void Execute(Player owner) {
	}

	public override void OnUnequip(Player owner) {
	}

	
	
	public override void OnUse(Player owner)
	{

		UserInfoManager.Instance.currentUser.healingUsed += 1;
		
		owner.CurHp += hpFillAmount;

		if (healVfxPrefab != null) {
			GameObject healVfx = Instantiate(healVfxPrefab, owner.transform, true);
			healVfx.transform.position = owner.transform.position;
		}
	}
}

}