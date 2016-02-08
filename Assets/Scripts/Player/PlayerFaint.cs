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
//	public void CmdFaint(NetworkInstanceId playerID) {
//		ClientScene.FindLocalObject(playerID).GetComponent<PlayerFaint>().RpcLocalFaint();
		player.GetComponent<PlayerFaint>().RpcLocalFaint();
	}

	[ClientRpc]
	public void RpcLocalFaint() {
		GetComponent<PlayerName>().DisableText();
		GetComponent<PlanetPlayerMove>().enabled = false;
		GetComponent<PlayerAttack>().enabled = false;
		GetComponent<PlayerMeleeSwing>().enabled = false;
		GetComponent<PlayerGravityBody>().enabled = false;
		GetComponent<Collider>().enabled = false;
		GetComponent<Rigidbody>().velocity = Vector3.zero;
		materialFader.FadeOut();
//		Renderer[] playerRenderers = GetComponentsInChildren<Renderer>();
//		for(int i = 0; i < playerRenderers.Length; i++) {
//			playerRenderers[i].enabled = false;
//		}	
		
		// TODO Killcam kind of thing?
	}


	[ClientRpc]
	public void RpcLocalRespawn() {
		Transform spawnPos = SpawnManager.instance.GetFreeSpawn(GetComponent<BasePlayerInfo>().playerTeam == Settings.HeroTeam); // TODO Team based spawn
		transform.position = spawnPos.position;
		transform.rotation = spawnPos.rotation;

		GetComponent<Collider>().enabled = true;
		GetComponent<Rigidbody>().velocity = Vector3.zero;
		GetComponent<PlanetPlayerMove>().enabled = true;
		GetComponent<PlayerGravityBody>().enabled = true;
		GetComponent<PlayerAttack>().enabled = true;
		GetComponent<PlayerMeleeSwing>().enabled = true;
		materialFader.FadeIn();

		StartCoroutine("WaitToVisuallyActivate");

//		GetComponent<PlayerName>().EnableText();
//		GetComponent<PlanetPlayerMove>().enabled = true;
//		GetComponent<PlayerAttack>().enabled = true;
//		GetComponent<PlayerMeleeSwing>().enabled = true;
//		GetComponent<PlayerGravityBody>().enabled = true;
//		GetComponent<Collider>().enabled = true;
//		GetComponent<Rigidbody>().velocity = Vector3.zero;
//		Renderer[] playerRenderers = GetComponentsInChildren<Renderer>();
//		for(int i = 0; i < playerRenderers.Length; i++) {
//			playerRenderers[i].enabled = true;
//		}
	}	 

	IEnumerator WaitToVisuallyActivate() {
		yield return new WaitForSeconds(0.5f);
		GetComponent<PlayerName>().EnableText();
//		Renderer[] playerRenderers = GetComponentsInChildren<Renderer>();
//		for(int i = 0; i < playerRenderers.Length; i++) {
//			playerRenderers[i].enabled = true;
//		}
	}

	[Command]
	public void CmdRespawn(GameObject player) {
		player.GetComponent<PlayerFaint>().RpcLocalRespawn();
	}

	// Unused
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
