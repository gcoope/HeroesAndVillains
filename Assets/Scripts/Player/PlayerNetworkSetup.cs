using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using smoothstudio.heroesandvillains.physics;
using smoothstudio.heroesandvillains.player.events;

public class PlayerNetworkSetup : NetworkBehaviour {

	public Camera playerCamera;
    private Rigidbody playerRigidbody;	
	public GameObject playerHUD;
	public GameObject playerNameText;
	public GameObject modelObj;

	void Awake() { 
		playerRigidbody = GetComponent<Rigidbody>();
		if (playerRigidbody == null) playerRigidbody = gameObject.AddComponent<Rigidbody>();
		playerCamera = gameObject.GetComponentInChildren<Camera>();
		gameObject.AddGlobalEventListener(UIEVent.RequestLocalCamera, (EventObject evt)=>{
			if(isLocalPlayer) gameObject.DispatchGlobalEvent(UIEVent.GotLocalCamera, new object[]{transform});
		});
	}	

	public override void OnStartLocalPlayer() {
		if(isLocalPlayer) {			
			playerNameText.SetActive(false);
			modelObj.SetActive(false);
			playerCamera.enabled = true;
			playerRigidbody.isKinematic = false;
			gameObject.GetComponent<PlayerGravityBody>().enabled = true;
			gameObject.GetComponent<AudioListener>().enabled = true;		
			if(Camera.main != null) Camera.main.enabled = false;
		}
	}

	void Start() {
		if(!isLocalPlayer && !isServer) {
			gameObject.DispatchGlobalEvent(PlayerEvent.UpdateScoreboard);
		}
	}

	void Update() {
//		if(Input.GetKeyDown(KeyCode.Escape)) {
//			if(isLocalPlayer) {
//				Debug.Log("Trying to d/c");
//				gameObject.DispatchGlobalEvent(MenuEvent.ClientDisconnect); // Not working
//			}
//		}
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

