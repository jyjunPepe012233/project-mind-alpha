using System;
using MinD.Runtime.Entity;
using MinD.Runtime.UI;
using MinD.SO.Game;
using MinD.SO.Item;
using MinD.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MinD.Runtime.Managers
{

	public class BossFightManager : Singleton<BossFightManager>
	{
		public event Action OnBossFightStart;
		public event Action<bool> OnBossFightFinish; // bool1: is boss felled?
		
		public bool IsBossFightStarted => currentBossFight != null;
		
		private BossFightInfoSo currentBossFight;
		private Enemy currentBoss;

		public void Instantiate()
		{
		}
		
		
		protected override void OnSceneChanged(Scene oldScene, Scene newScene) {

			if (WorldUtility.IsWorldScene(newScene))
			{
				BossRoomEntrance[] allEntrances = FindObjectsOfType<BossRoomEntrance>();
				foreach (BossRoomEntrance e in allEntrances)
				{
					e.OnEnter += Instance.StartBossFight;
				}
			}
		}

		private void StartBossFight(Enemy instantiatedBoss, BossFightInfoSo bossFightInfo)
		{
			if (instantiatedBoss == null)
			{
				return;
			}

			if (bossFightInfo == null)
			{
				return;
			}
			
			instantiatedBoss.dieAction += () => FinishBossFight(true);
			Player.player.dieAction += () => FinishBossFight(false);
			currentBossFight = bossFightInfo;
			currentBoss = instantiatedBoss;
			
			OnBossFightStart?.Invoke();
		}

		private void FinishBossFight(bool isBossFelled)
		{
			if (currentBossFight == null)
			{
				return;
			}
			
			if (isBossFelled)
			{
				if (currentBossFight.reward != null)
				{
					Player.player.inventory.AddItem(currentBossFight.reward.itemId, 1);
				}
				PlayerHUDManager.Instance.PlayBurstPopup(PlayerHUDManager.playerHUD.legendFelledPopup);
			}

			currentBossFight = null;
			currentBoss = null;

			OnBossFightFinish?.Invoke(isBossFelled);
		}

		public Enemy GetCurrentBoss()
		{
			if (currentBossFight != null)
			{
				return null;
			}

			return (currentBoss != null) ? currentBoss : null;
		}
	}

}