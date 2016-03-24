using UnityEngine;
using System.Collections;
using smoothstudio.heroesandvillains.player.events;
using smoothstudio.heroesandvillains.player;
using UnityEngine.Networking;
using smoothstudio.heroesandvillains.physics;

public class PlayerFaint : NetworkBehaviour {

	[SyncVar] private bool isHero;
	[SerializeField] private ModelMaterialFader materialFader;

	void Start() {
		isHero = gameObject.GetComponent<BasePlayerInfo>().playerTeam == Settings.HeroTeam;
	}

	[Command]
	public void CmdFaint(GameObject player) {
		player.GetComponent<PlayerFaint>().RpcLocalFaint();
	}

	[ClientRpc]
	public void RpcLocalFaint() {
//		GetComponent<PlayerName>().DisableText();
		GetComponent<PlanetPlayerMove>().enabled = false;
		GetComponent<PlayerAttack>().enabled = false;
		GetComponent<PlayerMeleeSwing>().enabled = false;
		GetComponent<Collider>().enabled = false;
		materialFader.FadeOut();
		if(isLocalPlayer) {
			GetComponent<PlayerGravityBody>().enabled = false;
			if(GetComponent<Rigidbody>() != null) GetComponent<Rigidbody>().velocity = Vector3.zero;
		}
	}

	[Command]
	public void CmdRespawn(GameObject player) {
		player.GetComponent<PlayerFaint>().RpcLocalRespawn();
	}

	[ClientRpc]
	public void RpcLocalRespawn() {
		Transform spawnPos = SpawnManager.instance.GetFreeSpawn(GetComponent<BasePlayerInfo>().playerTeam == Settings.HeroTeam); // TODO Team based spawn
		transform.position = spawnPos.position;
		transform.rotation = spawnPos.rotation;

		GetComponent<Collider>().enabled = true;
		GetComponent<PlanetPlayerMove>().enabled = true;
		GetComponent<PlayerAttack>().enabled = true;
		GetComponent<PlayerMeleeSwing>().enabled = true;
//		GetComponent<PlayerName>().EnableText();

		if(isLocalPlayer) {
			GetComponent<PlayerGravityBody>().enabled = true;
			if(GetComponent<Rigidbody>() != null) GetComponent<Rigidbody>().velocity = Vector3.zero;
		}

		materialFader.FadeIn(true);
	}	



}
