using UnityEngine;

namespace MinD.Runtime.Entity.Enemies {

	public class DreadDwarfMiner : HumanoidEnemy
	{
		[Header("Dread Dwarf Miner")]
		[SerializeField] private ParticleSystem _quakeSelfVfx;
		
		public void SummonQuake()
		{
			_quakeSelfVfx.Play();
			if (currentTarget != null)
			{
				GameObject quake = utility.InstantiatePrefab("DreadDwarfMiner_Quake");
				quake.transform.position = currentTarget.transform.position;
				quake.transform.forward = currentTarget.transform.position - transform.position;
				quake.GetComponent<DreadDwarfMinerQuake>().SetOwner(this);
			}
		}
	}

}