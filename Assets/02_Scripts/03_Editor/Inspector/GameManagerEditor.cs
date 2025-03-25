using MinD.Runtime.Managers;
using MinD.Runtime.Utils;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace MinD.Editor.Inspector {

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : UnityEditor.Editor {

	private GameManager component;
	


	void OnEnable() {
		
		component = target as GameManager;

	}

	public override void OnInspectorGUI() {
		
		base.OnInspectorGUI();
		
		if (GUILayout.Button("Bake World")) {
			component.BakeWorld();
		}
		if (GUILayout.Button("Clear Bake Data")) {
			component.ClearBakeData();
		}
	}


}

}
#endif