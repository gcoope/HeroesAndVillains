using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using smoothstudio.heroesandvillains.physics;
using smoothstudio.heroesandvillains.player.events;

public class PlayerNetworkSetup : NetworkBehaviour {

	public Camera playerCamera;
    private Rigidbody playerRigidbody;	
	public GameObject playerHUD;

	void Awake() { 
		playerRigidbody = GetComponent<Rigidbody>();
		if (playerRigidbody == null) playerRigidbody = gameObject.AddComponent<Rigidbody>();
		playerCamera = gameObject.GetComponentInChildren<Camera>();
	}	

	public override void OnStartLocalPlayer() {
		if(isLocalPlayer) {
			playerHUD.SetActive(true);
			playerCamera.enabled = true;
			playerRigidbody.isKinematic = false;
			gameObject.GetComponent<PlayerGravityBody>().enabled = true;
			gameObject.GetComponent<AudioListener>().enabled = true;
			if(Camera.main != null) Camera.main.enabled = false;
		}
	}

	public override void OnStartClient ()
	{		
		base.OnStartClient ();


		string _netID = gameObject.GetComponent<NetworkIdentity>().netId.ToString();
		BasePlayerInfo player = gameObject.GetComponent<BasePlayerInfo>();		
//		ServerPlayerManager.RegisterPlayer(_netID, player);
	}

	void Start() {
		LocalPlayerSetupInfo localPlayerInfo = NetworkManager.singleton.gameObject.GetComponent<LocalPlayerSetupInfo>();
		if(localPlayerInfo != null) {
			CmdLogSomething(localPlayerInfo.LocalPlayerName + " joined " + (localPlayerInfo.LocalPlayerTeam == Settings.HeroTeam ? "Heroes" : "Villains"));
		} else {
			Debug.Log("It's null");
		}
	}

	void OnDisable() {
		ServerPlayerManager.UnregisterPlayer(transform.name);
	}

	void Update() {
		if(Input.GetKeyDown(KeyCode.Escape)) {
			if(isLocalPlayer) {
				gameObject.DispatchGlobalEvent(MenuEvent.ClientDisconnect);
			}
		}
	}

	[Command]
	private void CmdLogSomething(string msg) {
		ServerOnlyPlayerDisplay.instance.Log(msg);
	}
}

