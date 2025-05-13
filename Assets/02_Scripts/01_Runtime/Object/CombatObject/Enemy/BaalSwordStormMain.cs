using MinD.Runtime.Entity;
using MinD.Runtime.Object.Utils;
using MinD.Runtime.Utils;
using MinD.Utility;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.Serialization;

public class BaalSwordStormMain : MonoBehaviour
{
	[SerializeField] private GameObject projectilePrefab;
	[SerializeField] private Vector3 summonOffset;
	[SerializeField] private Vector3 summonArea;
	
	[Space(15)]
	[SerializeField] private float summonCount;
	[SerializeField] private float summonDuration;

	private BaseEntity owner;
	private BaseEntity target;

	private float summonStack = 1;
	private float currentSummonCount;

	public void Init(BaseEntity owner, BaseEntity target)
	{
		this.owner = owner;
		this.target = target;
	}

	private void Update()
	{
		summonStack += Time.deltaTime / summonDuration * summonCount;
		while (summonStack > 1)
		{
			summonStack -= 1;
			var obj = Instantiate(projectilePrefab, GetRandomSummonPosition(), Quaternion.identity).GetComponent<BaalFlyingSword>();
			obj.Init(owner, target);
			currentSummonCount += 1;
		}

		if (currentSummonCount >= summonCount)
		{
			Destroy(gameObject);
		}
	}

	/// <summary>
	/// 설정한 소환 영역 내의 랜덤한 월드좌표 반환
	/// </summary>
	private Vector3 GetRandomSummonPosition()
	{
		Vector3 result = Vector3.zero;
		result.x = Random.Range(-summonArea.x, summonArea.x) / 2;
		result.y = Random.Range(-summonArea.y, summonArea.y) / 2;
		result.z = Random.Range(-summonArea.z, summonArea.z) / 2;
		return (transform.rotation * result) + summonOffset + transform.position;
	}
	
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.matrix = transform.localToWorldMatrix;
		Gizmos.DrawWireCube(summonOffset, summonArea);
	}
}
