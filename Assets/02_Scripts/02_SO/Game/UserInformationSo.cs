using MinD.Runtime.Managers;
using UnityEditor.Rendering;
using UnityEngine;

namespace MinD.SO.Game
{

	[CreateAssetMenu(fileName = "Custom User Information", menuName = "MinD/Game/CustomUserInformation")]
	public class UserInformationSo : ScriptableObject
	{
		[SerializeField] private float m_totalPlayTime;
		public float totalPlayTime
		{
			get => m_totalPlayTime;
			set => totalPlayTime = value;
		}
		
		private int m_deadCount;
		public int deadCount
		{
			get => m_deadCount;
			set => m_deadCount = value;
		}

		[SerializeField] private int m_damageDealt;
		public int damageDealt
		{
			get => m_damageDealt;
			set => damageDealt = value;
		}
		
		[SerializeField] private int m_damageTaken;
		public int damageTaken
		{
			get => m_damageTaken;
			set => damageTaken = value;
		}

		public int damageRatio
		{
			get => damageDealt / (damageTaken + 1);
		}

		[SerializeField] private int m_healingUsed;
		public int healingUsed
		{
			get => m_healingUsed;
			set => healingUsed = value;
		}
		
		public float[] GetDataForReference()
		{
			float[] result = new float[4];
			result[0] = this.totalPlayTime / (deadCount + 1); // 평균 생존 시간
			result[1] = this.damageRatio; // 데미지 비율
			result[2] = this.healingUsed; // 회복 사용 횟수
			return result;
		}
	}

}