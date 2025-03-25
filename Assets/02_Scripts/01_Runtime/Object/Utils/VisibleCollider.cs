using System.Linq;
using MinD.Enums;
using MinD.Utility;
using UnityEngine;

namespace MinD.Runtime.Object.Utils {

public class VisibleCollider : MonoBehaviour {
	
	[SerializeField] private ShowGizmoMode showGizmoMode;
	[SerializeField] private LayerMask showLayers = ~0;
	[SerializeField] private bool showChildCollider;
	
	[Space(10)] 
	public bool useCustomColor;
	public Color color;
	
	

	public void OnDrawGizmos() {

		if (showGizmoMode != ShowGizmoMode.Always)
			return;
		
		DrawColliderGizmo();
	}
	
	public void OnDrawGizmosSelected() {

		if (showGizmoMode != ShowGizmoMode.Selected)
			return;

		DrawColliderGizmo();
	}

	private void DrawColliderGizmo() {
		
		if (showChildCollider) {

			Collider[] cols = GetComponentsInChildren<Collider>(true);
			if (cols.Length == 0) {
				return;
			}
			
			DrawColliderGizmoFinal(cols, color);
			
		} else {

			Collider col = GetComponent<Collider>();
			if (col == null) {
				return;
			}
			
			DrawColliderGizmoFinal(col, color);
		}
		
	}

	
	private void DrawColliderGizmoFinal(Collider[] cols, Color? color) {

		if (!useCustomColor) {
			color = null;
		}
			
		foreach (Collider col in cols) {
			if (col.gameObject.activeInHierarchy &&
			    col.enabled &&
			    showLayers == (showLayers | (1 << col.gameObject.layer))) {
				// IS OBJECT IS ENABLE AND IN ALLOWED LAYERS
				
				GizmosUtility.DrawColliderGizmo(col, color);
			}
		}
		
	}
	private void DrawColliderGizmoFinal(Collider col, Color? color) {

		if (!useCustomColor) {
			color = null;
		}
		
		if (col.gameObject.activeInHierarchy &&
		    col.enabled &&
			showLayers == (showLayers | (1 << col.gameObject.layer))) {
			
			GizmosUtility.DrawColliderGizmo(col, color);
		}
		
	}
	
	

}

}