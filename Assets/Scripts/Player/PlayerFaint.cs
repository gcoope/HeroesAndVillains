using UnityEngine;
using System.Collections;
using smoothstudio.heroesandvillains.player.events;
using smoothstudio.heroesandvillains.player;
using UnityEngine.Networking;

public class PlayerFaint : NetworkBehaviour {

	private BasePlayerInfo playerInfo;

	void Awake() {
		playerInfo = gameObject.GetComponent<BasePlayerInfo>();
		gameObject.AddGlobalEventListener(PlayerEvent.PlayerFaint, MakePlayerFaint); 
		gameObject.AddGlobalEventListener(PlayerEvent.PlayerRespawn, MakePlayerRespawn); 
	}

	void OnDestroy() {
		gameObject.RemoveGlobalEventListener(PlayerEvent.PlayerFaint, MakePlayerFaint);
		gameObject.RemoveGlobalEventListener(PlayerEvent.PlayerRespawn, MakePlayerRespawn);
	}

	void Start () {

	}

	public void MakePlayerFaint(EventObject evt) {
		Debug.Log("Disabling stuff");
		GetComponent<PlanetPlayerMove>().enabled = false;
		GetComponent<PlayerAttack>().enabled = false;
		GetComponent<PlayerMeleeSwing>().enabled = false;
		Renderer[] playerRenderers = GetComponentsInChildren<Renderer>();
		for(int i = 0; i < playerRenderers.Length; i++) {
			playerRenderers[i].enabled = false;
		}
		
		playerInfo.isFainted = true;
	}

	public void MakePlayerRespawn(EventObject evt) {
		if(evt.Params[0] != null) {
			if((string)evt.Params[0] != playerInfo.playerName) return;
		}
		Debug.Log("Enabling stuff");
		GetComponent<PlanetPlayerMove>().enabled = true;
		GetComponent<PlayerAttack>().enabled = true;
		GetComponent<PlayerMeleeSwing>().enabled = true;
		Renderer[] playerRenderers = GetComponentsInChildren<Renderer>();
		for(int i = 0; i < playerRenderers.Length; i++) {
			playerRenderers[i].enabled = true;
		}
		
		playerInfo.isFainted = false;
	}
}
