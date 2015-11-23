using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour {

	public SpawnPoint[] heroSpawnPoints;
	public SpawnPoint[] villainSpawnPoints;

	// Singeltoning
	private static SpawnManager _instance;
	public static SpawnManager instance {
		get {
			if(_instance != null) return _instance;
			_instance = Object.FindObjectOfType<SpawnManager>();
			if(_instance != null) return _instance;
			Debug.LogError("SpawnManager instance was null");
			return null;
		}
	}

	void Awake() {
		if(_instance == null) _instance = this;
	}

	public Transform GetFreeSpawn(bool hero) {
		SpawnPoint[] checkArray = hero ? heroSpawnPoints : villainSpawnPoints;
		for(int i = 0; i < checkArray.Length; i++) {
			if(checkArray[i].IsFree) {
				checkArray[i].SpawnedOn();
				return checkArray[i].transform;
			}
		}
		Debug.Log("[SpawnManager] No free spawns found!");
		return transform; // As long as there are 5 spawn points this shouldn't happen
	}

}
