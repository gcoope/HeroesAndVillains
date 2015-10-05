using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SetupLobbyController : NetworkBehaviour {

	// Network
	private NetworkManager manager;

	// UI
	public Button readyUpButton;
	public Button startGameButton; // Host only
	public GameObject listHolder; // Holder of player list
	public Text statusText;
	private GameObject listItem; // prefab


	void Awake() {
		manager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
	}
	
	public void HostLan() {
		manager.StartHost();
	}
	
	public void JoinAsClient() {
		manager.StartClient();
	}
}
