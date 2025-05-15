using MinD.Runtime.Utils;
using MinD.SO.Game;
using MinD.SO.Object;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace MinD.Editor.Inspector {

[CustomEditor(typeof(WorldEnemyPlacerAI))]
public class WorldEnemyPlacerAIEditor : UnityEditor.Editor {

	private WorldEnemyPlacerAI component;
	


	void OnEnable() {
		
		component = target as WorldEnemyPlacerAI;

	}

	public override void OnInspectorGUI() {
		
		base.OnInspectorGUI();

//		UserInformationSo example = null;
//		if (component != null)
//		{
//			example = component.example;
//		}
//
//		if (example != null)
//		{
//			example.totalPlayTime = EditorGUILayout.FloatField("평균 생존 시간", example.totalPlayTime);
//			example.deadCount = EditorGUILayout.IntField("죽은 횟수",example.deadCount);
//			example.damageDealt = EditorGUILayout.IntField("입힌 데미지",example.damageDealt);
//			example.damageTaken = EditorGUILayout.IntField("입은 데미지", example.damageTaken);
//			example.healingUsed = EditorGUILayout.IntField("회복 횟수", example.healingUsed);
//		}
	}


}

}
#endif