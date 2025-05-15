using MinD.Runtime.Entity;
using MinD.SO.Game;
using UnityEngine;

namespace MinD.Runtime.Managers
{

	public class UserInfoManager : Singleton<UserInfoManager>
	{
		public UserInformationSo currentUser;
		

		public void Init()
		{
			currentUser = ScriptableObject.CreateInstance<UserInformationSo>();
			
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
			currentUser.totalPlayTime = Time.time;
		}
	}

}