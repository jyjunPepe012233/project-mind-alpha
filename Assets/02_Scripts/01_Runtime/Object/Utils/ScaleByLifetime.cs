using UnityEditor;
using UnityEngine;

namespace MinD.Runtime.Object.Utils
{

	public class ScaleByLifeTime : MonoBehaviour
	{
		private enum StopAction
		{
			None,
			Disable,
			Destroy
		}
		
		[SerializeField] private float startScale = 1f;
		[SerializeField] private float finalScale = 3f;
		[SerializeField] private float lifeTime = 1f;
		[SerializeField] private StopAction stopAction;
		
		private float timer;

		public void Update()
		{
			if (timer > lifeTime)
			{
				switch (stopAction)
				{
					case StopAction.None:
						break;
					case StopAction.Disable:
						gameObject.SetActive(false);
						break;
					case StopAction.Destroy:
						Destroy(gameObject);
						break;
				}
			}
			else
			{
				timer += Time.deltaTime;
				transform.localScale = Mathf.Lerp(startScale, finalScale, timer / lifeTime) * Vector3.one;
			}
		}
	}

}