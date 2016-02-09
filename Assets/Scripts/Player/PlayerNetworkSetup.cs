using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using smoothstudio.heroesandvillains.physics;
using smoothstudio.heroesandvillains.player.events;
using smoothstudio.heroesandvillains.player;

public class PlayerNetworkSetup : NetworkBehaviour {

	AudioListener audioListener;
	[SerializeField] PlayerGravityBody gravityBody;
//	[SerializeField] PlayerAttack playerAttack;
//	[SerializeField] PlayerMeleeSwing playerMelee;
	[SerializeField] PlayerHUD playerHUD;
	[SerializeField] ScoreUIController scoreUIController;
	[SerializeField] GameObject playerNameText;
	[SerializeField] ModelMaterialFader modelMaterialManager;
	[SerializeField] Camera playerCamera;

	void Awake() { 
		audioListener = GetComponent<AudioListener>();
		if(playerCamera == null) playerCamera = gameObject.GetComponentInChildren<Camera>();
		gameObject.AddGlobalEventListener(UIEVent.RequestLocalCamera, (EventObject evt)=>{
			if(isLocalPlayer) gameObject.DispatchGlobalEvent(UIEVent.GotLocalCamera, new object[]{transform});
		});
	}	

	public override void OnStartLocalPlayer() {
		if(isLocalPlayer) {			
			if(Camera.main != null) Camera.main.enabled = false;
			playerCamera.enabled = true;
			audioListener.enabled = true;
			gravityBody.enabled = true;
//			playerAttack.enabled = true;
//			playerMelee.enabled = true;
			playerHUD.enabled = true;
			scoreUIController.enabled = true;
			playerNameText.SetActive(false);
			modelMaterialManager.HideModel();

		}
	}

	void Start() {
		if(!isLocalPlayer && !isServer) {
			gameObject.DispatchGlobalEvent(PlayerEvent.UpdateScoreboard);
		}
	}

	void OnDisable() {
		if(ServerPlayerManager.instance != null) ServerPlayerManager.instance.UnregisterPlayer(gameObject.GetComponent<NetworkIdentity>().netId.ToString());

		if(!isLocalPlayer && !isServer) {
			gameObject.DispatchGlobalEvent(PlayerEvent.UpdateScoreboard);
		}
	}

	[Command]
	private void CmdLogSomething(string msg) {
		ServerOnlyPlayerDisplay.instance.Log(msg);
	}
}

