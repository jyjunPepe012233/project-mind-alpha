#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using MinD.Runtime.Object.Interactables;

namespace MinD.Editor
{
    [CustomEditor(typeof(Lift))]
    public class LiftEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Lift lift = (Lift)target;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Position Setup", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Set Current as Upper"))
            {
                Undo.RecordObject(lift, "Set Current as Upper Position");
                lift.SetCurrentAsUpper();
                EditorUtility.SetDirty(lift);
            }

            if (GUILayout.Button("Set Current as Lower"))
            {
                Undo.RecordObject(lift, "Set Current as Lower Position");
                lift.SetCurrentAsLower();
                EditorUtility.SetDirty(lift);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Test Controls", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Move To Upper"))
            {
                SerializedProperty upperPosProp = serializedObject.FindProperty("upperPosition");
                if (upperPosProp != null)
                {
                    Undo.RecordObject(lift.gameObject.transform, "Move Lift To Upper");
                    lift.gameObject.transform.position = upperPosProp.vector3Value;
                    EditorUtility.SetDirty(lift.gameObject.transform);
                }
            }

            if (GUILayout.Button("Move To Lower"))
            {
                SerializedProperty lowerPosProp = serializedObject.FindProperty("lowerPosition");
                if (lowerPosProp != null)
                {
                    Undo.RecordObject(lift.gameObject.transform, "Move Lift To Lower");
                    lift.gameObject.transform.position = lowerPosProp.vector3Value;
                    EditorUtility.SetDirty(lift.gameObject.transform);
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
#endif