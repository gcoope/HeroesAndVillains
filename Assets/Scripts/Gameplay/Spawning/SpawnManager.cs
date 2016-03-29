using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
		List<SpawnPoint> freeSpawns = new List<SpawnPoint>();
		for(int i = 0; i < checkArray.Length; i++) {
			if(checkArray[i].IsFree) {
//				checkArray[i].SpawnedOn();
				freeSpawns.Add(checkArray[i]);
//				return checkArray[i].transform;
			}
		}
		return freeSpawns[Random.Range(0, freeSpawns.Count)].transform; // Get random from the free spawns
	}

}
