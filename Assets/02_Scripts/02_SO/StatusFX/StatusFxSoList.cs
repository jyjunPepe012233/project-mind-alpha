using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MinD.SO.StatusFX {

[CreateAssetMenu(fileName = "Status Effect SO List", menuName = "MinD/Status Effect/SO List")]
public class StatusFXList : ScriptableObject {

	public List<InstantEffect> instantEffects;
	public List<StaticEffect> staticEffects;
	public List<TimedEffect> timedEffects;
	public List<StackingEffect> stackingEffects;
}

}