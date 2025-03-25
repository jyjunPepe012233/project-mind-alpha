using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace MinD.Runtime.Object.Utils {

public class LifeTimeObject : MonoBehaviour {

	[Serializable]
	public struct TimedObject { 
		public float lifeTime;
		public GameObject @object;
	}
	[Serializable]
	public struct TimedParticleSystem { 
		public float lifeTime;
		public ParticleSystem system;
	}
	
	private float lifeTimer;
	[SerializeField] private List<TimedObject> timedDisables = new();
	[SerializeField] private List<TimedObject> timedDestroys = new(); 
	[SerializeField] private List<TimedParticleSystem> timedParticles = new(); 



	

	public void OnEnable() {
		lifeTimer = 0;
	}

	public void Update() {
		lifeTimer += Time.deltaTime;

		for (int i = 0; i < timedDisables.Count; i++) {
			var timedDisable = timedDisables[i];
			
			if (timedDisable.@object == null) {
				continue;
			}
			if (!timedDisable.@object.activeSelf) {
				continue;
			}
			if (lifeTimer > timedDisable.lifeTime) {
				timedDisable.@object.SetActive(false);
			}
		}
		
		for (int i = timedDestroys.Count-1; i >= 0; i--) {
			var timedDestroy = timedDestroys[i];

			if (timedDestroy.@object == null) {
				continue;
			}
			if (lifeTimer > timedDestroy.lifeTime) {
				timedDestroys.RemoveAt(i);
				Destroy(timedDestroy.@object);
			}
		}
		
		for (int i = timedParticles.Count-1; i >= 0; i--) {
			var timedParticle = timedParticles[i];

			if (timedParticle.system == null) {
				continue;
			}
			if (lifeTimer > timedParticle.lifeTime) {
				timedParticle.system.Stop();
			}
		}
	}
	
}

}