using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using smoothstudio.heroesandvillains.physics;
using DG.Tweening;

public class PlayerNetworkSetup : NetworkBehaviour {

	[HideInInspector]
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
			gameObject.GetComponent<PlayerGravityBody>().localPlayer = true;
			gameObject.GetComponent<AudioListener>().enabled = true;
		}
	}
}
