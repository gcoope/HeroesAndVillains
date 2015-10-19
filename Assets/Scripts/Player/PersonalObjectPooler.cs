using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
using System;

public class PersonalObjectPooler : MonoBehaviour {
	
	private Dictionary<GameObject, List<GameObject>> pooledItems; // Will contain all types of pooled items, e.g. rock list, pickup list, enemey list etc.
	private Dictionary<GameObject, GameObject> spawnedItems; // Tracking what items are currently spawned to make it easy to recyle them

	private GameObject objPool;
	private PersonalObjectPooler personalInstance;

	public void Init (){
		if(personalInstance == null) {
			objPool = new GameObject("PersonalObjectPooler " + Guid.NewGuid().ToString());
			objPool.transform.position = Vector3.zero;
			objPool.transform.eulerAngles = Vector3.zero;
			objPool.transform.localScale = Vector3.one;
			personalInstance = objPool.AddComponent<PersonalObjectPooler>();
		}
	}
	
	void Awake() {
		this.pooledItems = new Dictionary<GameObject, List<GameObject>>(); // Dynamnic dict of all different pools
		this.spawnedItems = new Dictionary<GameObject, GameObject>(); // Dynamnic dict of all spawned prfabs (regardless of original pool list)
	}

	void OnDestroy() {
		Destroy(objPool);
	}
	
	// ---------------------------------------------------------------------------------
	// Public functions
	// ---------------------------------------------------------------------------------
	
	/// <summary>
	/// Creates a pool.
	/// </summary>
	/// <returns>The pool.</returns>
	/// <param name="prefab">Prefab.</param>
	/// <param name="amountToPool">Amount to pool.</param>
	public void CreatePool(GameObject prefab, int amountToPool) {
		if(prefab != null && !personalInstance.pooledItems.ContainsKey(prefab) && amountToPool > 0) {
			List<GameObject> tempList = new List<GameObject>(amountToPool);
			for(int i = 0; i < amountToPool; i++) {
				GameObject spawnedPrefab = Instantiate(prefab);
				spawnedPrefab.transform.parent = personalInstance.gameObject.transform;
				spawnedPrefab.SetActive(false);
				tempList.Add(spawnedPrefab);
			}
			personalInstance.pooledItems.Add(prefab, tempList);
		} 
	}
	
	public bool PoolExists(GameObject prefab) {
		return personalInstance.pooledItems.ContainsKey(prefab);
	}
	
	public void AppendToPool(GameObject prefab, int amountToPool) {
		if(prefab != null && amountToPool > 0) {
			for(int i = 0; i < amountToPool; i++) {
				GameObject spawnedPrefab = Instantiate(prefab);
				spawnedPrefab.transform.parent = personalInstance.gameObject.transform;
				spawnedPrefab.SetActive(false);
				personalInstance.pooledItems[prefab].Add(spawnedPrefab);
			}
		} 
	}
	
	/// <summary>
	/// Spawns from pool.
	/// </summary>
	/// <returns>Spawned item.</returns>
	/// <param name="poolList">Pool list.</param>
	/// <param name="parent">Parent.</param>
	/// <param name="position">Position.</param>
	public GameObject SpawnFromPool(GameObject prefab, Transform parent, Vector3 position = default(Vector3), Vector3 eulerAngles = default(Vector3)) {
		if(parent == null) parent = personalInstance.gameObject.transform;
		List<GameObject> pool;
		GameObject spawnedItem;
		if(personalInstance.pooledItems.TryGetValue(prefab, out pool)) {
			if(pool.Count > 0) {
				spawnedItem = pool[0];
				pool.RemoveAt(0);
				spawnedItem.SetActive(true);
				spawnedItem.transform.parent = parent;
				spawnedItem.transform.position = position;
				spawnedItem.transform.eulerAngles = eulerAngles;
				personalInstance.spawnedItems.Add(spawnedItem, prefab);
				return spawnedItem;
			} else { // If getting from pool fails somehow, instantiate a new one - Should never occur
				Debug.LogWarning("Pool size was not big enough - runtime instantiating: " + prefab.name);
				spawnedItem = Instantiate(prefab);
				spawnedItem.SetActive(true); // Would be true by default anyway
				spawnedItem.transform.parent = parent;
				spawnedItem.transform.position = position;
				spawnedItem.transform.eulerAngles = eulerAngles;
				personalInstance.spawnedItems.Add(spawnedItem, prefab);
				return spawnedItem;
			}
		} 
		return null; // Never hit but errors force my hand
	}
	
	/// <summary>
	/// Recycles to pool.
	/// </summary>
	/// <param name="obj">Object to reycle.</param>
	public void RecycleToPool(GameObject obj){
		GameObject prefab;
		if(personalInstance.spawnedItems.TryGetValue(obj, out prefab)) {
			personalInstance.pooledItems[prefab].Add (obj);
			personalInstance.spawnedItems.Remove (obj);
			obj.transform.parent = personalInstance.gameObject.transform;
			obj.transform.position = Vector3.zero;
			obj.SetActive(false);
			NetworkServer.UnSpawn(obj); // neccesary I think
		} else { // Force destroy - not preffered but will work the same
			Debug.LogWarning("Force destroying: " + obj + ", it wasn't found in spawnedItems");
			//Object.Destroy(obj);
		}
	}
	
	/// <summary>
	/// Recycles the item passed in event.
	/// </summary>
	/// <param name="evt">Evt.</param>
	private void RecycleItem(EventObject evt) {
		if(evt.Params[0] != null) {
			RecycleToPool((evt.Params[0] as GameObject));
		}
	}
	
	/// <summary>
	/// Recycles ALL spawned pooled items.
	/// </summary>
	/// <param name="evt">Evt.</param>
	public void RecycleAllItems(EventObject evt = null) {
		List<GameObject> tempList = new List<GameObject>();
		tempList.AddRange(personalInstance.spawnedItems.Keys);
		for(int i = 0; i < tempList.Count; i++) {
			RecycleToPool(tempList[i]);
		}
		tempList.Clear();
	}

	// Really custom and kinda hacky
	public List<GameObject> GetPrefabList(GameObject prefab) {
		if(prefab != null && personalInstance.pooledItems.ContainsKey(prefab)) {
			return personalInstance.pooledItems[prefab];
		}
		return null;
	}
	public void ReplacePrefabList(GameObject prefab, List<GameObject> newList) {
		if(prefab != null && newList != null && personalInstance.pooledItems.ContainsKey(prefab)) {
			personalInstance.pooledItems[prefab] = newList;
		}
	}
}
