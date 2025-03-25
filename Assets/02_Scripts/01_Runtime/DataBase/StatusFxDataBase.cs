using MinD.Enums;
using MinD.SO.StatusFX;
using UnityEngine;
using UnityEngine.Serialization;


namespace MinD.Runtime.DataBase {

public class StatusFxDataBase : Singleton<StatusFxDataBase> {

	[SerializeField]
	private StatusFXList effectSOList;


	public InstantEffect InstantiateEffectData(InstantEffectType type) {

		foreach (InstantEffect effect in effectSOList.instantEffects) {

			if (effect.enumId == type)
				return Instantiate(effect);
		}

		Debug.LogError("Can't Find Effect In List As Parameter Type");
		return null;
	}

	public StaticEffect InstantiateEffectData(StaticEffectType type) {

		foreach (StaticEffect effect in effectSOList.staticEffects) {

			if (effect.enumId == type)
				return Instantiate(effect);
		}

		Debug.LogError("Can't Find Effect In List As Parameter Type");
		return null;
	}

	public TimedEffect InstantiateEffectData(TimedEffectType type) {

		foreach (TimedEffect effect in effectSOList.timedEffects) {

			if (effect.enumId == type)
				return Instantiate(effect);
		}

		Debug.LogError("Can't Find Effect In List As Parameter Type");
		return null;
	}

	public StackingEffect InstantiateEffectData(StackingEffectType type) {

		foreach (StackingEffect effect in effectSOList.stackingEffects) {

			if (effect.enumId == type)
				return Instantiate(effect);
		}

		Debug.LogError("Can't Find Effect In List As Parameter Type");
		return null;
	}

}

}