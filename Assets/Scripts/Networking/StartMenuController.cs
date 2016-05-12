using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using DG.Tweening;
using smoothstudio.heroesandvillains.player.events;
using UnityEngine.Networking.Match;

public class StartMenuController : MonoBehaviour {

	private string ipAddress = "localhost";
	private bool isHost = false;
	private Text infoText;
	private NetworkManager netManager;

	void Awake() {	
		// Should be noted that events will be added and handled even if script is disabled
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

		if(!netManager) netManager = gameObject.GetComponent<NetworkManager>();

		// findme online scene setting
		switch(Settings.gameWorld) {
			case SettingsGameWorld.METROPOLIS:
				netManager.onlineScene = "DylanWorld";
				break;
			case SettingsGameWorld.BORG:
				netManager.onlineScene = "CyborgWorld";
				break;
			case SettingsGameWorld.CANDYLAND:
				netManager.onlineScene = "CandyLand";
				break;
			case SettingsGameWorld.LOBBY:
				netManager.onlineScene = "MainMenu";
				break;
		}
	}

	void Start() {
		DOTween.Init(); // findme DOTween init
	}

	private void HandleInputFieldChange(EventObject evt) {
		if(evt.Params[0] != null) {
			InputFieldVO data = (InputFieldVO)evt.Params[0];

			if(data.inputKey == InputFieldKeys.MenuIPInput) {
				ipAddress = data.inputValue;
				if(netManager != null) netManager.networkAddress = ipAddress;
			}
		}
	}

	public void JoinAsClient() { JoinAsClient(null); }
	public void JoinAsClient(EventObject evt = null) {
		CheckHostAddress ();
		netManager.StartClient();
	}

	public void HostLan() { HostLan(null); }
	public void HostLan(EventObject evt = null) {
		isHost = true;
		CheckHostAddress();
		netManager.StartHost();
	}

	public void StartAsServer() { StartAsServer(null); }
	public void StartAsServer(EventObject evt = null) {
//		netManager.StopServer();
//		netManager.StartServer();
		netManager.networkAddress = "localhost";
		netManager.StartServer();
	}

	public void CloseApp() { CloseApp(null); }
	public void CloseApp(EventObject evt = null) {
		Application.Quit();
	}

	public void Disconnect(EventObject evt) {
		if(isHost) {
			netManager.StopHost();
			isHost = false;
		} else {
			if(netManager.IsClientConnected()) {
				netManager.client.Disconnect();
				//SceneManager.LoadScene(0);
				Application.LoadLevel (0);
			}
		}
	}

	// Matchmaker
	public void StartMatchMaker() {StartMatchMaker(null);}
	public void StartMatchMaker(EventObject evt = null) {
		netManager.StartMatchMaker();
	}

	public void StopMatchMaker() {StopMatchMaker(null);}
	public void StopMatchMaker(EventObject evt = null) {
		netManager.StopMatchMaker();
	}

	public void CreateRoom() {CreateRoom(null);}
	public void CreateRoom(EventObject evt = null) {
		if(netManager.matches == null || netManager.matches.Count == 0) {
			netManager.matchMaker.CreateMatch("default", netManager.matchSize, true, "", netManager.OnMatchCreate);	
			SetInfoText("Creating game...");
		}
	}

	public void JoinRoom() {JoinRoom(null);}
	public void JoinRoom(EventObject evt = null) {
		netManager.matchMaker.ListMatches(0, 20, "", delegate(ListMatchResponse response) {
			netManager.OnMatchList(response);
			foreach(MatchDesc m in response.matches) {
				Debug.Log(m.name);
				if(m.name == "default") {
					netManager.matchName = m.name;
					netManager.matchSize = (uint)m.currentSize;
					netManager.matchMaker.JoinMatch(m.networkId, "", netManager.OnMatchJoined);
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
		if(string.IsNullOrEmpty(netManager.networkAddress)) netManager.networkAddress = "localhost";
	}
}