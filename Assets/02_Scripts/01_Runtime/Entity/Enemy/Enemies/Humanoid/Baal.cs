using MinD.Runtime.Object;
using UnityEditor;
using UnityEngine;

namespace MinD.Runtime.Entity.Enemies {

	public class Baal : HumanoidEnemy
	{
		public void ShootTinyLazerForComboAttack3()
		{
			var obj = utility.InstantiatePrefabOnSelf("Baal_TinyLaser").GetComponentInChildren<BaalTinyLaser>();
			obj.Init(this);
		}
		
		private void CallOmen()
		{
			AshenDemonOmen[] omens = utility.InstantiatePrefabOnSelf("Baal_Omens").GetComponentsInChildren<AshenDemonOmen>();
			
			foreach (var o in omens)
			{
				o.Setup(this, currentTarget);
			}
		}
		
		public void SummonStalkingMoon()
		{
			var obj = utility.InstantiatePrefabOnSelf("Baal_StalkingMoon").GetComponentInChildren<BaalStalkingMoon>();
			obj.Init(this, currentTarget);
		}
		
		public void ShootDarkTorrent()
		{
			var obj = utility.InstantiatePrefabOnSelf("Baal_DarkTorrent").GetComponentInChildren<BaalDarkTorrentMain>();
			obj.Init(this);
		}
		
		public void SummonSwordStorm()
		{
			var obj = utility.InstantiatePrefabOnSelf("Baal_SwordStorm_Main").GetComponentInChildren<BaalSwordStormMain>();
			obj.Init(this, currentTarget);
		}
		
		public void SummonEclipse()
		{
			var obj = utility.InstantiatePrefabOnSelf("Baal_Eclipse_Main").GetComponentInChildren<BaalEclipseController>();
			obj.Init(this, currentTarget);
		}
	}
}