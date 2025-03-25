using System.Collections.Generic;
using MinD.Enums;
using MinD.SO.StatusFX;
using UnityEngine;

namespace MinD.Runtime.Entity {

public class EntityStatusFxHandler : EntityOwnedHandler {

	[Header("[ Owned Effect List ]")] 
	public InstantEffect instantEffectSlot; // LATEST EFFECT
	[SerializeField] private List<StaticEffect> staticEffects;
	[SerializeField] private List<TimedEffect> timedEffects;
	[SerializeField] private List<StackingEffect> stackingEffects;
	// 비정상적인 접근으로 인한 영구적인 스테이터스 변동을 막기 위해 PRIVATE으로 제한함
	
	[Header("[ Immune ]")]
	public bool isImmuneToAllEffect;
	[Space(5)]
	public List<TimedEffectType> timedFxImmune;
	public List<StackingEffectType> stackingFxImmune;

	
	
	public void HandleAllEffect() {

		// 능동적인 상태이상(TIMED, STACKING) 객체의 생명 관리 메소드
		HandleTimedEffect();
		HandleStackingEffect();

	}

	

	public void AddInstantEffect(InstantEffect effectInstance) {

		if (owner.isDeath) {
			return;
		}

		// MAKE SURE CAN APPLY INSTANT EFFECT
		if (effectInstance == null) {
			return;
		}

		// REFRESH THE PREVIOUS EFFECT SLOT
		if (instantEffectSlot != null) {
			Destroy(instantEffectSlot);
		}
		
		instantEffectSlot = effectInstance;
		instantEffectSlot.OnInstantiate(owner);
	}
	
	

	private void HandleTimedEffect() {

		foreach (TimedEffect effect in timedEffects) {

			effect.Execute();

			effect.remainTime -= Time.deltaTime;

			if (effect.remainTime < 0)
				RemoveTimedEffect(effect);
		}
	}
	public void AddTimedEffect(TimedEffect effect) {
		
		if (owner.isDeath) {
			return;
		}

		timedEffects.Add(effect);
		effect.OnInstantiate(owner);
	}
	public void RemoveTimedEffect(TimedEffect effect) {

		timedEffects.Remove(effect);

		effect.OnRemove();
		Destroy(effect);
	}
	public void RemoveTimedEffectByType(TimedEffectType type) {

		foreach (TimedEffect effect in timedEffects)
			if (effect.enumId == type)
				RemoveTimedEffect(effect);
	}
	

	
	public void AddStaticEffect(StaticEffect effect) {
		
		if (owner.isDeath) {
			return;
		}

		staticEffects.Add(effect);
		effect.OnInstantiate(owner);

	}
	public void RemoveStaticEffect(StaticEffect effect) {

		staticEffects.Add(effect);
		effect.OnRemove();

		Destroy(effect);
	}
	public void RemoveStaticEffectByType(StaticEffectType type) {

		foreach (StaticEffect effect in staticEffects)
			if (effect.enumId == type)
				RemoveStaticEffect(effect);

	}
	
	

	private void HandleStackingEffect() {
		
	}
	public void AddStackingEffect(StackingEffectType type, float stackAmount) {
		
		if (owner.isDeath) {
			return;
		}
		
	}
	public void RemoveStackingEffect(StackingEffectType type) {
	}
	

	
	public void RemoveAllEffect() {

		foreach (StaticEffect effect in staticEffects)
			RemoveStaticEffect(effect);

		foreach (TimedEffect effect in timedEffects)
			RemoveTimedEffect(effect);

		foreach (StackingEffect effect in stackingEffects)
			RemoveStackingEffect(effect.enumId);
	}
	
	
}

}