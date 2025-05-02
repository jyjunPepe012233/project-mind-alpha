using MinD.Runtime.Object;
using UnityEngine;

namespace MinD.Runtime.Entity.Enemies {

	public class AshenDemon : HumanoidEnemy
	{
		[Header("Ashen Demon")]
		[SerializeField] private ParticleSystem swordTrail;
		[SerializeField] private GameObject swordTrailActiveReference;
		
		private void CallOmen()
		{
			AshenDemonOmen[] omens = utility.InstantiatePrefabOnSelf("AshenDemon_Omens").GetComponentsInChildren<AshenDemonOmen>();
			
			foreach (var o in omens)
			{
				o.Setup(this, currentTarget);
			}
		}

		private void Update()
		{
			if (swordTrail.isPlaying && !swordTrailActiveReference.activeInHierarchy)
			{
				swordTrail.Stop();
			} else if (!swordTrail.isPlaying && swordTrailActiveReference.activeInHierarchy)
			{
				swordTrail.Play();
			}
		}
	}
}