using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using DG.Tweening;
using smoothstudio.heroesandvillains.player.events;
using System;

public class StartMenuController : MonoBehaviour {


	private NetworkManager manager; 
	public InputField hostAddressText;

	void Awake() {	
//		gameObject.AddGlobalEventListener(MenuEvent.JoinLocal, JoinAsClient);
		gameObject.AddGlobalEventListener(MenuEvent.HostLocal, HostLan);
//		gameObject.AddGlobalEventListener(MenuEvent.HostServer, StartAsServer);
		gameObject.AddGlobalEventListener(MenuEvent.CloseGame, CloseApp);

		if(hostAddressText == null) {
			Debug.Log("manually searching for IP input");
			hostAddressText = GameObject.Find("IP Input Field").GetComponent<InputField>();
		}
	}

	void Start() {
		manager = GetComponent<NetworkManager>();
		DOTween.Init(); // findme DOTween init
	}

	public void JoinAsClient() { JoinAsClient(null); }
	public void JoinAsClient(EventObject evt = null) {
		CheckHostAddress ();
		manager.StartClient();
	}

	public void HostLan() { HostLan(null); }
	public void HostLan(EventObject evt = null) {
		CheckHostAddress();
		manager.StartHost();
	}

	public void StartAsServer() { StartAsServer(null); }
	public void StartAsServer(EventObject evt = null) {
		CheckHostAddress ();
		manager.StartServer();
	}

	public void CloseApp() { CloseApp(null); }
	public void CloseApp(EventObject evt = null) {
		Debug.Log("Exiting Application");
		Application.Quit();
	}

	private void CheckHostAddress() {
		if(hostAddressText == null) { // Manual search if reference lost - TODO Fix this system
			hostAddressText = GameObject.Find("IP Input Field").GetComponent<InputField>();
		}

		manager.networkAddress = hostAddressText.text != "" ? hostAddressText.text : "localhost";
	}

	void Update() {
		if(Input.GetKeyDown(KeyCode.Escape)) {
			if(manager.IsClientConnected()) {
				Debug.Log("Stopping client");
				manager.StopClient();
				try {
					Debug.Log("Stopping host");
					manager.StopHost();
				} catch (Exception e) {
					Debug.Log(e.Message);
				}
			}
		}
	}


}