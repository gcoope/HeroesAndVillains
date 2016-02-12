using System;
using UnityEngine.Networking;
using UnityEngine;

public class LocalPrefabSpawner : NetworkBehaviour	{

	public static LocalPrefabSpawner instance {
		get {
			if(_instance == null) {_instance = GameObject.FindObjectOfType<LocalPrefabSpawner>();}
			return _instance;
		}
	}
	private static LocalPrefabSpawner _instance;

	[SerializeField] private GameObject lightningSparkPrefab;
	[SerializeField] private GameObject fireBallPrefab;
	[SerializeField] private GameObject linePrefab;

	void Start() {
		ObjectPooler.instance.CreatePool(lightningSparkPrefab, 50);
		ObjectPooler.instance.CreatePool(fireBallPrefab, 50);
		ObjectPooler.instance.CreatePool(linePrefab, 50);
	}

	// Primary attack explosions
	[Server]
	public void ServerSpawnExplosion(Vector3 position, string team) {
		GameObject explosion = ObjectPooler.instance.SpawnFromPool(team == Settings.HeroTeam ? lightningSparkPrefab : fireBallPrefab, transform);
		explosion.transform.position = position;
		RpcSpawnLocalExplosion(explosion.transform.position, team == Settings.HeroTeam);
	}
	[ClientRpc]
	private void RpcSpawnLocalExplosion(Vector3 position, bool isHero) {
		if(isServer) return;
		GameObject explosion = ObjectPooler.instance.SpawnFromPool(isHero ? lightningSparkPrefab : fireBallPrefab, transform);
		explosion.transform.position = position;
	}

	// Line
	[Server]
	public void ServerSpawnLine(Vector3 start, Vector3 end, string team) {
		GameObject line = ObjectPooler.instance.SpawnFromPool(linePrefab, transform);
		line.GetComponent<LocalLineDrawer>().DrawLine(start, end, team);
		RpcSpawnLocalLine(start, end, team);
	}
	[ClientRpc]
	private void RpcSpawnLocalLine(Vector3 start, Vector3 end, string team) {
		if(isServer) return;
		GameObject line = ObjectPooler.instance.SpawnFromPool(linePrefab, transform);
		line.GetComponent<LocalLineDrawer>().DrawLine(start, end, team);
	}
}
