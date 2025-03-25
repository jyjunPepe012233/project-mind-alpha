using System;
using System.Collections;
using System.Runtime.ExceptionServices;
using MinD.Runtime.Managers;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace MinD.Runtime.Entity {

public class PlayerDefenseMagic : MonoBehaviour {

	[HideInInspector] public Player owner;

	
	public Collider defenseCollider;
	[SerializeField] private ParticleSystem defenseVfxSystem;
	[SerializeField] private ParticleSystem guardBreakVfxSystem;


	[Space(15)]
	[SerializeField] private float defenseRadius; 
	[SerializeField] private float hexagonMinDistance;
	[SerializeField] private float hitReactionAngleRadius;


	private ParticleSystem.Particle[] vfx_Particles;
	private int vfx_AliveCnt;


	private Coroutine vfxHandlingCoroutine;
	private WaitForSeconds emissionRateYield;





	public void EnableShield() {

		// IF PLAYER DOESN'T EQUIP PROTECTION, JUST PERFORMING ACTION IN GUARD 
		if (owner.inventory.protectionSlot != null) {

			// set negation
			owner.attribute.damageNegation *= owner.inventory.protectionSlot.negationBoost;


			defenseCollider.enabled = true;
			
			defenseVfxSystem.Play();
		}

	}


	public void DisableShield() {

		// ACTIVE DEFENSE MAGIC IF PLAYER EQUIPPING PROTECTION
		if (owner.inventory.protectionSlot != null) {

			// set negation
			owner.attribute.damageNegation /= owner.inventory.protectionSlot.negationBoost;


			defenseCollider.enabled = false;
			
			defenseVfxSystem.Stop();
		}
	}



	private void LateUpdate() {

		// NOT TO BE PLAYER'S CHILD OBJECT CAUSE INHERIT ONLY POSITION
		transform.position = owner.transform.position;

		

		if (defenseVfxSystem.isPlaying) {
			vfx_Particles = new ParticleSystem.Particle[defenseVfxSystem.main.maxParticles];

			// GetParticles is allocation free because we reuse the m_Particles buffer between updates (BY UNITY MANUAL)
			vfx_AliveCnt = defenseVfxSystem.GetParticles(vfx_Particles);

			// SET FIRE POSITION OF A PARTICLES
			for (int i = 0; i < vfx_AliveCnt; i++) {
				if (vfx_Particles[i].position == Vector3.zero) {

					int loopTime = 0;
					while (true) {

						vfx_Particles[i].position = Random.onUnitSphere * defenseRadius;


						// COMPARE PARTICLES BETWEEN ALL EACH OTHER
						// OVERLAPPED PARTICLE IS EXISTS, TRY GET RANDOM POSITION AGAIN TO LOOP
						bool isOverlapped = false;
						for (int j = 0; j < vfx_AliveCnt; j++) {

							// PREVENT COMPARE WITH SELF
							if (j == i) {
								continue;
							}
							
							if (Vector3.Distance(vfx_Particles[i].position, vfx_Particles[j].position) < hexagonMinDistance) {
								isOverlapped = true;
								break;
							}
						}

						if (!isOverlapped) {
							break;
						}

						// PREVENT INFINITE LOOP
						if (loopTime++ > 100) {
							Debug.LogWarning("!! Loop Broke! ");
							break;
						}
					}
					
					vfx_Particles[i].rotation3D = Quaternion.LookRotation(vfx_Particles[i].position).eulerAngles;
				}
			}
			
			defenseVfxSystem.SetParticles(vfx_Particles);
		}
		
	}



	public void PlayHitVFX(Vector3 hitPoint) {
		
		vfx_AliveCnt = defenseVfxSystem.GetParticles(vfx_Particles);

		for (int i = 0; i < vfx_AliveCnt; i++) {

			if (Vector3.Angle(vfx_Particles[i].position, hitPoint) < hitReactionAngleRadius) {
				vfx_Particles[i].remainingLifetime = 1f;
				vfx_Particles[i].startColor = Color.white;
			}

		}
		
		defenseVfxSystem.SetParticles(vfx_Particles);
	}


	public void PlayGuardBreakVFX() {
		
		guardBreakVfxSystem.Play();

		
		
		vfx_AliveCnt = defenseVfxSystem.GetParticles(vfx_Particles);

		for (int i = 0; i < vfx_AliveCnt; i++) {
			
			vfx_Particles[i].remainingLifetime = 0.5f;
			vfx_Particles[i].startColor = Color.white;

		}
		
		defenseVfxSystem.SetParticles(vfx_Particles);
		
	}
	
	
}

}