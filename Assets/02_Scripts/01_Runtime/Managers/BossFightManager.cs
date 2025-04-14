using MinD.Runtime.Entity;
using MinD.Runtime.UI;
using MinD.SO.Game;
using MinD.SO.Item;

namespace MinD.Runtime.Managers
{

	public class BossFightManager : Singleton<BossFightManager>
	{
		private BossFightInfoSo currentBossFight;

		private void Awake()
		{
			base.Awake();
			
			BossRoomEntrance[] allEnterance = FindObjectsOfType<BossRoomEntrance>();
			foreach (BossRoomEntrance e in allEnterance)
			{
				e.OnEnter += StartBossFight;
			}
		}

		private void StartBossFight(Enemy instantiatedBoss, BossFightInfoSo bossFightInfo)
		{
			instantiatedBoss.dieAction += StopBossFight;
			currentBossFight = bossFightInfo;
		}

		private void StopBossFight()
		{
			if (currentBossFight.reward != null)
			{
				Player.player.inventory.AddItem(currentBossFight.reward.itemId, 1);
			}
			currentBossFight = null;
			
			PlayerHUDManager.Instance.PlayBurstPopup(PlayerHUDManager.playerHUD.legendFelledPopup);
		}
	}

}