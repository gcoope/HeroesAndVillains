using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using DG.Tweening;
using smoothstudio.heroesandvillains.player.events;
using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using UnityEngine.SceneManagement;
using UnityEngine.Networking.Match;

public class StartMenuController : MonoBehaviour {

	private string ipAddress = "localhost";
	private bool isHost = false;
	private Text infoText;

	void Awake() {	
		gameObject.AddGlobalEventListener(MenuEvent.JoinLocal, JoinAsClient);
		gameObject.AddGlobalEventListener(MenuEvent.HostLocal, HostLan);
		gameObject.AddGlobalEventListener(MenuEvent.HostServer, StartAsServer);
		gameObject.AddGlobalEventListener(MenuEvent.ClientDisconnect, Disconnect);
		gameObject.AddGlobalEventListener(MenuEvent.CloseGame, CloseApp);
		gameObject.AddGlobalEventListener(MenuEvent.InputFieldChange, HandleInputFieldChange);

		gameObject.AddGlobalEventListener(MenuEvent.StartMatchMaker, StartMatchMaker);
		gameObject.AddGlobalEventListener(MenuEvent.StopMatchMaker, StopMatchMaker);
		gameObject.AddGlobalEventListener(MenuEvent.CreateOnlineRoom, CreateRoom);
		gameObject.AddGlobalEventListener(MenuEvent.JoinDefaultRoom, JoinRoom);
	}

	void Start() {
		DOTween.Init(); // findme DOTween init
	}

	private void HandleInputFieldChange(EventObject evt) {
		if(evt.Params[0] != null) {
			InputFieldVO data = (InputFieldVO)evt.Params[0];

			if(data.inputKey == InputFieldKeys.MenuIPInput) {
				ipAddress = data.inputValue;
				if(NetworkManager.singleton != null) NetworkManager.singleton.networkAddress = ipAddress;
			}
		}
	}

	public void JoinAsClient() { JoinAsClient(null); }
	public void JoinAsClient(EventObject evt = null) {
		CheckHostAddress ();
		NetworkManager.singleton.StartClient();
	}

	public void HostLan() { HostLan(null); }
	public void HostLan(EventObject evt = null) {
		isHost = true;
		CheckHostAddress();
		NetworkManager.singleton.StartHost();
	}

	public void StartAsServer() { StartAsServer(null); }
	public void StartAsServer(EventObject evt = null) {
		NetworkManager.singleton.StopServer();
		NetworkManager.singleton.networkAddress = "localhost";
		NetworkManager.singleton.StartServer();
	}

	public void CloseApp() { CloseApp(null); }
	public void CloseApp(EventObject evt = null) {
		Application.Quit();
	}

	public void Disconnect(EventObject evt) {
		if(isHost) {
			NetworkManager.singleton.StopHost();
			isHost = false;
		} else {
			if(NetworkManager.singleton.IsClientConnected()) {
				NetworkManager.singleton.client.Disconnect();
				SceneManager.LoadScene(0);
			}
		}
	}

	// Matchmaker
	public void StartMatchMaker() {StartMatchMaker(null);}
	public void StartMatchMaker(EventObject evt = null) {
		NetworkManager.singleton.StartMatchMaker();
	}

	public void StopMatchMaker() {StopMatchMaker(null);}
	public void StopMatchMaker(EventObject evt = null) {
		NetworkManager.singleton.StopMatchMaker();
	}

	public void CreateRoom() {CreateRoom(null);}
	public void CreateRoom(EventObject evt = null) {
		if(NetworkManager.singleton.matches == null || NetworkManager.singleton.matches.Count == 0) {
			NetworkManager.singleton.matchMaker.CreateMatch("default", NetworkManager.singleton.matchSize, true, "", NetworkManager.singleton.OnMatchCreate);	
			SetInfoText("Creating game...");
		}
	}

	public void JoinRoom() {JoinRoom(null);}
	public void JoinRoom(EventObject evt = null) {
		NetworkManager.singleton.matchMaker.ListMatches(0, 20, "", delegate(ListMatchResponse response) {
			NetworkManager.singleton.OnMatchList(response);
			foreach(MatchDesc m in response.matches) {
				Debug.Log(m.name);
				if(m.name == "default") {
					NetworkManager.singleton.matchName = m.name;
					NetworkManager.singleton.matchSize = (uint)m.currentSize;
					NetworkManager.singleton.matchMaker.JoinMatch(m.networkId, "", NetworkManager.singleton.OnMatchJoined);
					SetInfoText("Found game, joining...");
				}
			}
			if(response.matches == null || response.matches.Count == 0) {
				SetInfoText("No online games found, create one!");
			}
		});
	}

	void SetInfoText(string txt) {
		if(infoText == null) infoText = GameObject.Find("InfoText").GetComponent<Text>();
		infoText.text = txt;
		StartCoroutine("ResetInfoText");
	}

	IEnumerator ResetInfoText() {
		yield return new WaitForSeconds(4);
		if(infoText != null) infoText.text = "";
	}

	private void CheckHostAddress() {
		if(string.IsNullOrEmpty(NetworkManager.singleton.networkAddress)) NetworkManager.singleton.networkAddress = "localhost";
	}
}