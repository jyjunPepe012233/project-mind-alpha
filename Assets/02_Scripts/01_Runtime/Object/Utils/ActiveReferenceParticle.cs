using UnityEditor;
using UnityEngine;

namespace MinD.Runtime.Object.Utils
{

	public class ActiveReferenceParticle : MonoBehaviour
	{
		private enum UpdateTime
		{
			Update,
//			OnAnimatorMove
		}

		[SerializeField] private UpdateTime updateTime;
		
		[SerializeField] private GameObject[] activeReference; // 이 오브젝트들 중 하나라도 활성화되어 있다면 파티클을 재생시킴
		[SerializeField] private ParticleSystem particle;

		private void Update()
		{
			if (updateTime == UpdateTime.Update)
			{
				UpdateActiveBasedOnReferences();
			}
		}

//		private void OnAnimatorMove()
//		{
//			if (updateTime == UpdateTime.OnAnimatorMove)
//			{
//				UpdateActiveBasedOnReferences();
//			}
//		}
		
		private void UpdateActiveBasedOnReferences(){
			
			if (activeReference == null)
			{
				return;
			}

			bool isReferenceActive = false;
			for (int i = 0; i < activeReference.Length; i++)
			{
				if (activeReference[i] == null)
				{
					continue;
				}

				if (activeReference[i].activeInHierarchy)
				{
					isReferenceActive = true;
					break;
				}
			}

			if (isReferenceActive && !particle.isPlaying)
			{
				particle.Play();
			}

			if (!isReferenceActive && particle.isPlaying)
			{
				particle.Stop();
			}
		}

		#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			if (activeReference == null)
			{
				return;
			}
			
			Gizmos.color = new Color(1, 0.5f, 0);
			Gizmos.DrawSphere(particle.transform.position, 0.5f);

			
			Handles.color = Color.blue;
			for (int i = 0; i < activeReference.Length; i++)
			{
				if (activeReference[i] == null)
				{
					continue;
				}

				if (activeReference[i].transform.position == particle.transform.position)
				{
					continue;
				}
				
				Vector3 directionWithSize = particle.transform.position - activeReference[i].transform.position;
				
				Gizmos.color = Color.blue;
				Gizmos.DrawSphere(activeReference[i].transform.position, 0.5f);
				Gizmos.DrawLine(particle.transform.position, activeReference[i].transform.position);
				Handles.ArrowHandleCap(
					0, 
					activeReference[i].transform.position, 
					Quaternion.LookRotation(directionWithSize), 
					Mathf.Min(3, directionWithSize.magnitude - 0.5f), 
					EventType.Repaint
					);
			}
		}
		#endif
	}

}