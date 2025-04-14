using UnityEngine;

namespace MinD.SO.Game
{
	[CreateAssetMenu(fileName = "Boss Fight Info", menuName = "MinD/Game/Boss Fight Info")]
	public class BossFightInfoSo : ScriptableObject
	{
		[SerializeField] private Item.Item m_reward;

		public Item.Item reward
		{
			get => m_reward;
		}
	}

}