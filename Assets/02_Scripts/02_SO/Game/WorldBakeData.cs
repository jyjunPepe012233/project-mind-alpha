using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MinD.SO.Game {

[Obsolete("World baking system is obsolete. Bake world method were just indexing IDs of objects", true), Serializable]
public class WorldBakeData : ScriptableObject {

	[Serializable]
	public struct SerializableKeyValuePair<TKey, TValue> {
		public readonly TKey bakeId;
		public readonly TValue instanceId;
		public SerializableKeyValuePair(TKey bakeId, TValue instanceId) {
			this.bakeId = bakeId;
			this.instanceId = instanceId;
		}
	}
	
	// WHY WE DON'T USE DICTIONARY? BECAUSE WE SHOULD USE A SERIALIZED OBJECT(SO WE MAKE CUSTOM SERIALIZABLE PAIR)   
	public List<SerializableKeyValuePair<int, int>> enemyBakeIdWithInstanceId = new();
	public List<SerializableKeyValuePair<int, int>> anchorBakeIdWithInfoInstanceId = new();
	

	public void AddEnemyWorldData(int enemyId, int enemyObjectInstanceId) {
		if (enemyBakeIdWithInstanceId.Any(i => i.bakeId == enemyId)) { 
			throw new UnityException("Duplicated enemy bake id was tried insert");
		}
		if (enemyBakeIdWithInstanceId.Any(i => i.bakeId == enemyObjectInstanceId)) { 
			throw new UnityException("Duplicated instance id was tried insert");
		}
		
		enemyBakeIdWithInstanceId.Add(new(enemyId, enemyObjectInstanceId));
	}

	public void AddGuffinsAnchorWorldData(int anchorId, int infoSoInstanceId) {
		if (anchorBakeIdWithInfoInstanceId.Any(i => i.bakeId == anchorId)) {
			throw new UnityException("Duplicated anchor id was tried insert");
		}
		if (anchorBakeIdWithInfoInstanceId.Any(i => i.instanceId == infoSoInstanceId)) {
			throw new UnityException("Duplicated info instance id was tried insert");
		}
		
		anchorBakeIdWithInfoInstanceId.Add(new(anchorId, infoSoInstanceId));
	}
	
}

}