using MinD.Runtime.Entity;
using MinD.Runtime.Managers;
using UnityEngine;
using UnityEngine.AI;

namespace MinD.Runtime.Object
{
	public class EnemySpawner : MonoBehaviour
	{
		[SerializeField] private Transform[] bounds;
		[SerializeField] private Enemy[] enemies;

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.H))
			{
				SpawnEnemy();
			}
		}

		public void SpawnEnemies(int count)
		{
			for (int i = 0; i < count; i++)
			{
				SpawnEnemy();
			}
		}
		
		public void SpawnEnemy()
		{
			
			Transform bound = bounds[Random.Range(0, bounds.Length)];
			Vector3 randomPosition = bound.transform.position;
			randomPosition.x += Random.Range(-bound.localScale.x / 2, bound.localScale.x / 2);
			randomPosition.y += Random.Range(-bound.localScale.y / 2, bound.localScale.y / 2);
			randomPosition.z += Random.Range(-bound.localScale.z / 2, bound.localScale.z / 2);

			if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, Mathf.Infinity, NavMesh.AllAreas))
			{
				PlayerHUDManager.Instance.PlayPopup("안개 속에서 적이 나타났다.");
				Instantiate(enemies[Random.Range(0, enemies.Length)], hit.position, Quaternion.LookRotation(Player.player.transform.position, hit.position));
			}
		}

		private void OnDrawGizmos()
		{
			for (int i = 0; i < bounds.Length; i++)
			{
				Gizmos.DrawWireCube(bounds[i].position, bounds[i].localScale);
			}
		}
	}

}