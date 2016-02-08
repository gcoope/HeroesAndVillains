﻿using UnityEngine;
using System.Collections;
using smoothstudio.heroesandvillains.player.events;
using smoothstudio.heroesandvillains.player;
using UnityEngine.Networking;
using smoothstudio.heroesandvillains.physics;

public class PlayerFaint : NetworkBehaviour {

	[SyncVar] private bool isHero;

	void Start() {
		isHero = gameObject.GetComponent<BasePlayerInfo>().playerTeam == Settings.HeroTeam;
	}

	[ClientRpc]
	public void RpcLocalFaint() {
//	public void LocalFaint() {
		GetComponent<PlayerName>().DisableText();
		GetComponent<PlanetPlayerMove>().enabled = false;
		GetComponent<PlayerAttack>().enabled = false;
		GetComponent<PlayerMeleeSwing>().enabled = false;
		GetComponent<PlayerGravityBody>().enabled = false;
		GetComponent<Collider>().enabled = false;
		GetComponent<Rigidbody>().velocity = Vector3.zero;
		Renderer[] playerRenderers = GetComponentsInChildren<Renderer>();
		for(int i = 0; i < playerRenderers.Length; i++) {
			playerRenderers[i].enabled = false;
		}	
		
		// TODO Killcam kind of thing?
//		CmdFaint(gameObject.GetComponent<NetworkIdentity>().netId);
	}

	[Command]
	public void CmdFaint(GameObject player) {

		player.GetComponent<PlayerFaint>().RpcLocalFaint();

//		ServerOnlyPlayerDisplay.instance.Log(gameObject.GetComponent<BasePlayerInfo>().playerName + " local fainted");
//		GameObject player = NetworkServer.FindLocalObject(id);
//		ServerOnlyPlayerDisplay.instance.Log(player.GetComponent<BasePlayerInfo>().playerName + " network fainted");
//		player.GetComponent<PlayerName>().DisableText();
//		player.GetComponent<PlanetPlayerMove>().enabled = false;
//		player.GetComponent<PlayerAttack>().enabled = false;
//		player.GetComponent<PlayerMeleeSwing>().enabled = false;
//		player.GetComponent<PlayerGravityBody>().enabled = false;
//		player.GetComponent<Collider>().enabled = false;
//		player.GetComponent<Rigidbody>().velocity = Vector3.zero;
//		Renderer[] playerRenderers = player.GetComponentsInChildren<Renderer>();
//		for(int i = 0; i < playerRenderers.Length; i++) {
//			playerRenderers[i].enabled = false;
//		}	
	}

	[ClientRpc]
	public void RpcLocalRespawn() {
//	public void LocalRespawn() {
		Transform spawnPos = SpawnManager.instance.GetFreeSpawn(GetComponent<BasePlayerInfo>().playerTeam == Settings.HeroTeam);// NetworkManager.singleton.GetStartPosition(); // TODO Team based spawn
		transform.position = spawnPos.position;
		transform.rotation = spawnPos.rotation;
//		GetComponent<PlayerName>().EnableText();
		GetComponent<PlanetPlayerMove>().enabled = true;
		GetComponent<PlayerAttack>().enabled = true;
		GetComponent<PlayerMeleeSwing>().enabled = true;
		GetComponent<PlayerGravityBody>().enabled = true;
		GetComponent<Collider>().enabled = true;
		GetComponent<Rigidbody>().velocity = Vector3.zero;
		Renderer[] playerRenderers = GetComponentsInChildren<Renderer>();
		for(int i = 0; i < playerRenderers.Length; i++) {
			playerRenderers[i].enabled = true;
		}

//		CmdRespawn(gameObject.GetComponent<NetworkIdentity>().netId);
//		CmdRespawnPlayer();
	}	                          

	[Command]
	public void CmdRespawn(GameObject player) {
		player.GetComponent<PlayerFaint>().RpcLocalRespawn();
		return;
//	public void CmdRespawn(NetworkInstanceId id) {
//		ServerOnlyPlayerDisplay.instance.Log(gameObject.GetComponent<BasePlayerInfo>().playerName + "local respawned");
//		GameObject player = NetworkServer.FindLocalObject(id);
		ServerOnlyPlayerDisplay.instance.Log(player.GetComponent<BasePlayerInfo>().playerName + "network respawned");
		Transform spawnPos = SpawnManager.instance.GetFreeSpawn(player.GetComponent<BasePlayerInfo>().playerTeam == Settings.HeroTeam); // TODO More advanced team based spawn
		player.transform.position = spawnPos.position;
		player.transform.rotation = spawnPos.rotation;
		player.GetComponent<PlayerName>().EnableText();
		player.GetComponent<PlanetPlayerMove>().enabled = true;
		player.GetComponent<PlayerAttack>().enabled = true;
		player.GetComponent<PlayerMeleeSwing>().enabled = true;
		player.GetComponent<PlayerGravityBody>().enabled = true;
		player.GetComponent<Collider>().enabled = true;
		player.GetComponent<Rigidbody>().velocity = Vector3.zero;
		Renderer[] playerRenderers = player.GetComponentsInChildren<Renderer>();
		for(int i = 0; i < playerRenderers.Length; i++) {
			playerRenderers[i].enabled = true;
		}
	}


	[Command]
	public void CmdRespawnPlayer() {
		Transform spawnPos = SpawnManager.instance.GetFreeSpawn(isHero);// TODO More advanced spawns to avoid spawn camp?
		GameObject newPlayer = Instantiate<GameObject>(NetworkManager.singleton.playerPrefab);
		newPlayer.transform.position = spawnPos.position;
		newPlayer.transform.rotation = spawnPos.rotation;
		NetworkServer.Destroy(this.gameObject);
		NetworkServer.ReplacePlayerForConnection(this.connectionToClient, newPlayer, this.playerControllerId);
	}
}
