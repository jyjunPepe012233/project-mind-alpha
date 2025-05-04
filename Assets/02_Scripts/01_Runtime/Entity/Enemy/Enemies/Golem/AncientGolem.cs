using MinD.Runtime.Object;
using UnityEngine;

namespace MinD.Runtime.Entity.Enemies {

	public class AncientGolem : GolemEnemy
	{
		[Header("[ Ancient Golem ]")]
		[SerializeField] private GameObject[] scrapeActiveReference; // 이 오브젝트들 중 하나라도 활성화되어 있다면 파티클을 재생시킴
		[SerializeField] private ParticleSystem scrapeParticle;
		
		protected override void Update()
		{
			base.Update();

			if (scrapeActiveReference != null)
			{
				// 레퍼런스 오브젝트 중 하나라도 활성화된 것이 있는 경우에 파티클을 재생시킴.
				
				bool isReferenceActivated = false;
				foreach (var g in scrapeActiveReference)
				{
					if (g.activeInHierarchy)
					{
						isReferenceActivated = true;
						continue;
					}
				}

				if (isReferenceActivated && !scrapeParticle.isPlaying)
				{
					scrapeParticle.Play();
				}

				if (!isReferenceActivated && scrapeParticle.isPlaying)
				{
					scrapeParticle.Stop();
				}
			}
			
		}
	}
}