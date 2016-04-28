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
	[SerializeField] private GameObject scorchDecalPrefab;

	void Start() {
		ObjectPooler.instance.CreatePool(lightningSparkPrefab, 75);
		ObjectPooler.instance.CreatePool(fireBallPrefab, 75);
		ObjectPooler.instance.CreatePool(linePrefab, 75);
		ObjectPooler.instance.CreatePool(scorchDecalPrefab, 75);
	}

	// Primary attack explosions
	[Server]
	public void ServerSpawnExplosion(Vector3 position, string team) {
		GameObject explosion = ObjectPooler.instance.SpawnFromPool(team == Settings.HeroTeam ? lightningSparkPrefab : fireBallPrefab, transform);
		explosion.transform.position = position;
		RpcSpawnLocalExplosion(explosion.transform.position, team == Settings.HeroTeam);
	}
	[Server]
	public void ServerSpawnExplosionWithNormal(Vector3 position, Vector3 hitNormal, string team) {
		GameObject explosion = ObjectPooler.instance.SpawnFromPool(team == Settings.HeroTeam ? lightningSparkPrefab : fireBallPrefab, transform);
		explosion.transform.position = position + (hitNormal * 0.01f);
//		explosion.transform.position = position;
		explosion.transform.rotation = Quaternion.FromToRotation(explosion.transform.up, hitNormal);

		GameObject scorchDecal = ObjectPooler.instance.SpawnFromPool(scorchDecalPrefab, transform);
		scorchDecal.transform.position = position + (hitNormal * 0.01f);
		scorchDecal.transform.rotation = Quaternion.FromToRotation(scorchDecal.transform.up, hitNormal);

		RpcSpawnLocalExplosionWithNormal(explosion.transform.position, hitNormal, team == Settings.HeroTeam);
	}
	[ClientRpc]
	private void RpcSpawnLocalExplosion(Vector3 position, bool isHero) {
		if (isHero) {
			AudioKeys.HeroExplosion.PlaySound (position);
		} else {
			AudioKeys.VillainExplosion.PlaySound (position);
		}
		if(isServer) return;
		GameObject explosion = ObjectPooler.instance.SpawnFromPool(isHero ? lightningSparkPrefab : fireBallPrefab, transform);
		explosion.transform.position = position;
	}
	[ClientRpc]
	private void RpcSpawnLocalExplosionWithNormal(Vector3 position, Vector3 hitNormal, bool isHero) {
		if (isHero) {
			AudioKeys.HeroExplosion.PlaySound (position);
		} else {
			AudioKeys.VillainExplosion.PlaySound (position);
		}
		if(isServer) return;
		GameObject explosion = ObjectPooler.instance.SpawnFromPool(isHero ? lightningSparkPrefab : fireBallPrefab, transform);
		explosion.transform.position = position + (hitNormal * 0.01f);
//		explosion.transform.position = position;
		explosion.transform.rotation = Quaternion.FromToRotation(explosion.transform.up, hitNormal);

		GameObject scorchDecal = ObjectPooler.instance.SpawnFromPool(scorchDecalPrefab, transform);
		scorchDecal.transform.position = position + (hitNormal * 0.01f);
		scorchDecal.transform.rotation = Quaternion.FromToRotation(scorchDecal.transform.up, hitNormal);

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
