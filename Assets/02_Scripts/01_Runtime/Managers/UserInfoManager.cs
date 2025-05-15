using MinD.Runtime.Entity;
using MinD.SO.Game;
using UnityEngine;


namespace MinD.Runtime.Managers
{

	public class UserInfoManager : Singleton<UserInfoManager>
	{
		public UserInformationSo currentUser;

		private float playTimeStack;
		

		public void Init()
		{
			currentUser = new UserInformationSo(0, 0, 1, 1, 1);
			
			var placers = FindObjectsOfType<WorldEnemyPlacerAI>();
			foreach (var placer in placers)
			{
				placer.example = currentUser;
			}

			Player.player.dieAction += () =>
			{
				currentUser.deadCount += 1;
			};
		}

		private void Update()
		{
			playTimeStack += Time.deltaTime;
			if (playTimeStack > 1)
			{
				currentUser.totalPlayTime += 1;
				playTimeStack -= 1;
			}
		}
	}

}