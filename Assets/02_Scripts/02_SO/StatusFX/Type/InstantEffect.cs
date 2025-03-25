using MinD.Enums;
using MinD.Runtime.Entity;
using UnityEngine;

namespace MinD.SO.StatusFX {

public abstract class InstantEffect : ScriptableObject {

	public InstantEffectType enumId;

	public abstract void OnInstantiate(BaseEntity victim);

}

}