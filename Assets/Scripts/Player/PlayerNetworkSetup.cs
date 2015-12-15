using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using smoothstudio.heroesandvillains.physics;

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

//		GameObject manager = GameObject.Find ("NetworkManager");
//		NetworkManager m = manager.GetComponent<NetworkManager>();
//		LocalPlayerSetupInfo localPlayerInfo = manager.gameObject.GetComponent<LocalPlayerSetupInfo>();
		string _netID = gameObject.GetComponent<NetworkIdentity>().netId.ToString();
		BasePlayerInfo player = gameObject.GetComponent<BasePlayerInfo>();
//		if(localPlayerInfo != null) {
//			player.playerName = localPlayerInfo.LocalPlayerName;
//			player.playerTeam = localPlayerInfo.LocalPlayerTeam;
//		} else {
//			Debug.Log("It's null");
//		}
		
		ServerPlayerManager.RegisterPlayer(_netID, player);
	}

	void OnDisable() {
		ServerPlayerManager.UnregisterPlayer(transform.name);
	}
}

