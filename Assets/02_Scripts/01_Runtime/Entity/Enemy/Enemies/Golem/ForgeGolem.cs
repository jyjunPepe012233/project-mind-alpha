using System;
using MinD.Runtime.Object;
using UnityEngine;

namespace MinD.Runtime.Entity.Enemies {

	public class ForgeGolem : GolemEnemy
	{
		public void LavaSmash()
		{
			GameObject obj = utility.InstantiatePrefabOnSelf("ForgeGolem_LavaSmash_Main");
			obj.GetComponent<ForgeGolemLavaSmashMain>().SetOwner(this);
		}

		public void Whirlwind()
		{
			GameObject obj = utility.InstantiatePrefabOnSelf("ForgeGolem_Whirlwind_Main");
			obj.GetComponent<ForgeGolemWhirlwindMain>().SetOwner(this);
		}
	}
}