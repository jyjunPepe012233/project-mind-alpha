using UnityEngine;
using UnityEngine.SceneManagement;

namespace MinD.Utility {

public static class WorldUtility {
	
	public const string SCENENAME_dungeon = "PrototypeDungeon";
	public static bool IsWorldScene(Scene scene) {
		return scene.name == SCENENAME_dungeon;
	}
	public static bool IsThisWorldScene() {
		return SceneManager.GetActiveScene().name == SCENENAME_dungeon;
	}
	
	public const string LAYERNAME_environment = "Default";
	public const string LAYERNAME_damageable = "Damageable Entity";
	public static LayerMask environmentLayerMask => LayerMask.GetMask(LAYERNAME_environment);
	public static LayerMask damageableLayerMask => LayerMask.GetMask(LAYERNAME_damageable);

}

}