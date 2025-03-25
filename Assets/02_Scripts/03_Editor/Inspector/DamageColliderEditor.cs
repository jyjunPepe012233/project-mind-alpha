using MinD.Runtime.Utils;
using MinD.SO.Object;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace MinD.Editor.Inspector {

[CustomEditor(typeof(DamageCollider))]
public class DamageColliderEditor : UnityEditor.Editor {

	private DamageCollider component;
	


	void OnEnable() {
		
		component = target as DamageCollider;

	}

	public override void OnInspectorGUI() {
		
		GUILayout.Space(10);

		// CHECK COLLIDER IS EXIST
		var collider = component.GetComponent<Collider>();
		
		if (collider == null) {
			EditorGUILayout.LabelField("Requires a Collider Component");
			GUILayout.Space(10);
			return;
			
		} else {
			collider.isTrigger = true;
		}
		
		
		
		component.soData = (DamageData)EditorGUILayout.ObjectField("Reference Data", component.soData, typeof(DamageData), false);
		GUILayout.Space(10);
		
		if (component.soData != null) {
			
			var soData = component.soData;
			
			EditorGUILayout.BeginFoldoutHeaderGroup(true, "Damage", EditorStyles.foldoutHeader);
			EditorGUI.indentLevel++;
			soData.damage.physical = EditorGUILayout.IntField("Physical", soData.damage.physical);
			soData.damage.magic = EditorGUILayout.IntField("Magic", soData.damage.magic);
			soData.damage.fire = EditorGUILayout.IntField("Fire", soData.damage.fire);
			soData.damage.frost = EditorGUILayout.IntField("Frost", soData.damage.frost);
			soData.damage.lightning = EditorGUILayout.IntField("Lightning", soData.damage.lightning);
			soData.damage.holy = EditorGUILayout.IntField("Holy", soData.damage.holy);
			EditorGUI.indentLevel--;
			EditorGUILayout.EndFoldoutHeaderGroup();
			
			EditorGUILayout.IntField("Total Damage", soData.totalDamage);
			
			
			EditorGUILayout.Space(15);
			soData.poiseBreakDamage = EditorGUILayout.IntSlider("Poise Break Damage", soData.poiseBreakDamage, 0, 100);

			EditorGUILayout.Space(15);
			soData.isDirectional = EditorGUILayout.Toggle("Is Directional", soData.isDirectional);
			if (soData.isDirectional) {
				soData.damageDirection = EditorGUILayout.Vector3Field("Damage Direction", soData.damageDirection);
			}

			EditorGUILayout.Space(15);
			if (GUILayout.Button("Reset To Hit Again")) {
				component.ResetToHitAgain();
			}
			
			
			
			soData.RefreshValue();
		}

	}


}

}
#endif