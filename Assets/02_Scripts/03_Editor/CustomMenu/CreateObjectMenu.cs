using MinD.Runtime.Entity;
using MinD.Runtime.Object.Interactables;
using MinD.Runtime.Object.Utils;
using MinD.Runtime.Utils;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace MinD.Editor.CustomMenu {

public class CreateObjectMenu {
	
	[MenuItem("GameObject/Create With MinD/Create Damage Collider", false, int.MinValue + 0)]
	public static void CreateNewDamageCollider() {

		// CREATE NEW ITEM
		GameObject newItem = new GameObject("Damage Collider", typeof(DamageCollider), typeof(VisibleCollider), typeof(Rigidbody));

		GameObjectUtility.SetParentAndAlign(newItem, Selection.activeGameObject);

		Undo.RegisterCreatedObjectUndo(newItem, "Create Damage Collider");
		Selection.activeGameObject = newItem;

		newItem.layer = LayerMask.NameToLayer("DamageCollider");

		
		// HANDLE COMPONENT VALUE
		newItem.GetComponent<VisibleCollider>().useCustomColor = true;
		newItem.GetComponent<VisibleCollider>().color = new Color(1, 0.05f, 0.05f, 0.65f);
		newItem.GetComponent<Rigidbody>().isKinematic = true;
	}


	[MenuItem("GameObject/Create With MinD/Create Dropped Item", false, int.MinValue + 1)]
	public static void CreateNewDroppedItem() {

		// CREATE NEW ITEM
		GameObject newItem = new GameObject("Dropped Item", typeof(DroppedItem), typeof(SphereCollider));

		GameObjectUtility.SetParentAndAlign(newItem, Selection.activeGameObject);

		Undo.RegisterCreatedObjectUndo(newItem, "Create Dropped Item");
		Selection.activeGameObject = newItem;


		// SET PROPERTIES
		Collider newItemCollider = newItem.GetComponent<Collider>();
		newItemCollider.isTrigger = true;
	}

	[MenuItem("GameObject/Create With MinD/Create Target Option", false, int.MinValue + 2)]
	public static void CreateTargetOption() {

		if (Selection.activeGameObject == null) {
			foreach (SceneView sceneView in SceneView.sceneViews)
				sceneView.ShowNotification(new GUIContent("Target option should be child of some object"));
			return;
		}


		// COUNT TARGET OPTION ALREADY EXIST
		Transform root = Selection.activeGameObject.transform.root;
		Transform[] allChildren = root.GetComponentsInChildren<Transform>();
		int targetOptionCount = 1;

		foreach (Transform child in allChildren) {
			if (child.name.Contains("Target Option"))
				targetOptionCount++;
		}


		// CREATE NEW ITEM
		GameObject newItem = new GameObject("Target Option (" + targetOptionCount + ")");

		GameObjectUtility.SetParentAndAlign(newItem, Selection.activeGameObject);

		Undo.RegisterCreatedObjectUndo(newItem, "Target Option");
		Selection.activeGameObject = newItem;


		// BIND TARGET OPTION IN ENTITY
		BaseEntity entity = root.GetComponent<BaseEntity>();

		if (entity != null)
			entity.targetOptions.Add(newItem.transform);
		else
			foreach (SceneView sceneView in SceneView.sceneViews)
				sceneView.ShowNotification(new GUIContent("The new target option wasn't bound to entity"));
	}
	
	[MenuItem("GameObject/Create With MinD/Create Boss Room Entrance", false, int.MinValue + 3)]
	public static void CreateNewBossRoomEntrance() {

		// CREATE NEW ITEM
		GameObject newItem = new GameObject("Boss Room Entrance", typeof(BossRoomEntrance), typeof(BoxCollider));

		GameObjectUtility.SetParentAndAlign(newItem, Selection.activeGameObject);

		Undo.RegisterCreatedObjectUndo(newItem, "Create Boss Room Entrance");
		Selection.activeGameObject = newItem;
	}
	
}

}
#endif
