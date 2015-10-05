using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class StartMenuController : NetworkLobbyManager {


	// UI
	public GameObject startCanvas;
	public GameObject lobbyCanvas;
	private GameObject currentCanvas;

	public InputField nameBox;
	public Button readyUpButton;
	public Button startGameButton; // Host only
	public GameObject listHolder; // Holder of player list
	public Text statusText;
	private GameObject playerListItem; // prefab

	void Awake() {

		currentCanvas = startCanvas;
	}

	void Start() {
		//playerListItem = Resources.Load<GameObject>("Prefabs/Lobby/PlayerItem");
#if UNITY_EDITOR
		//HostLan();
#endif
	}

	public void HostLan() {
		base.StartHost();
		//SwapCanvas();
	}

	public void JoinAsClient() {
		base.StartClient();
		SwapCanvas();
	}

	public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
	{
		Debug.Log("OnLobbyServerSceneLoadedForPlayer");
		return true;
	}

	public override void OnLobbyClientConnect (NetworkConnection conn)
	{
		Debug.Log("On client connect");
		base.OnLobbyClientConnect (conn);
	}

	public void SetPlayerReady() {

	}

	private void SwapCanvas() {
		if(currentCanvas == startCanvas) {
			currentCanvas = lobbyCanvas;
			currentCanvas.SetActive(true);
			startCanvas.SetActive(false);
		} else {
			currentCanvas = startCanvas;
			currentCanvas.SetActive(true);
			lobbyCanvas.SetActive(false);
		}
	}
}