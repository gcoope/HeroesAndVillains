using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour {

	public NetworkManager manager;
	public InputField nameBox;

	void Awake() {
		manager = GetComponent<NetworkManager>();
	}

	public void HostLan() {
		manager.StartHost();
	}

	public void JoinAsClient() {
		manager.StartClient();
	}

}