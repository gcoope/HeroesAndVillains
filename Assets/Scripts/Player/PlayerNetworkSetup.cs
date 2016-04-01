using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using smoothstudio.heroesandvillains.physics;
using smoothstudio.heroesandvillains.player.events;
using smoothstudio.heroesandvillains.player;

public class PlayerNetworkSetup : NetworkBehaviour {

	AudioListener audioListener;
	[SerializeField] PlayerGravityBody gravityBody;
	[SerializeField] PlayerHUD playerHUD;
	[SerializeField] ScoreUIController scoreUIController;
	[SerializeField] GameObject playerNameText;
	[SerializeField] ModelMaterialFader modelMaterialManager;
	[SerializeField] Camera playerCamera;
	[SerializeField] PlayerAnimator playerAnimator;

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
			playerAnimator.enabled = true;
			gravityBody.Setup(gameObject.AddComponent<Rigidbody>());
			playerHUD.enabled = true;
			scoreUIController.enabled = true;
			playerNameText.SetActive(false);
			modelMaterialManager.HideModel();

			// Playing game music here
			AudioKeys.GameSong1.PlayMusic();
		}
	}

	void Start() {
		if(!isLocalPlayer && !isServer) {
			gameObject.DispatchGlobalEvent(PlayerEvent.UpdateScoreboard);
			modelMaterialManager.SetModelShowing(true);
			modelMaterialManager.ShowModel();
		}
	}

	void OnDisable() {
		if(isServer) ServerDisable();
	}

	void ServerDisable() {
		if(ServerPlayerManager.instance != null && netId != null) ServerPlayerManager.instance.UnregisterPlayer(netId);
	}
}

