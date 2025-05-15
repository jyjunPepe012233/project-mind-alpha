using MinD.Runtime.Managers;
using UnityEditor.Rendering;
using UnityEngine;

namespace MinD.SO.Game
{

	[CreateAssetMenu(fileName = "Custom User Information", menuName = "MinD/Game/CustomUserInformation")]
	public class UserInformationSo : ScriptableObject
	{
		[SerializeField] private float m_totalPlayTime; //(초)
		public float totalPlayTime
		{
			get => m_totalPlayTime;
			set => m_totalPlayTime = value;
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
			set => m_damageDealt = value;
		}
		
		[SerializeField] private int m_damageTaken;
		public int damageTaken
		{
			get => m_damageTaken;
			set => m_damageTaken = value;
		}

		public float damageRatio
		{
			get => m_damageDealt / ((float)m_damageTaken + 1);
		}

		[SerializeField] private int m_healingUsed;
		public int healingUsed
		{
			get => m_healingUsed;
			set => m_healingUsed = value;
		}
		
		public float[] GetDataForReference()
		{
			float[] result = new float[3];
			result[0] = this.totalPlayTime / (deadCount + 1) / 600f; // 평균 생존 시간
			result[1] = Mathf.Clamp(this.damageRatio / 3, -1, 1f); // 데미지 비율
			result[2] = this.healingUsed / 30f; // 회복 사용 횟수
			return result;
		}
	}

}