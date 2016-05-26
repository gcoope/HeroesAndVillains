using UnityEngine;
using System.Collections;

public class WebCommunicator : MonoBehaviour {

	private string URL = "";
	private string hashcode = "smoothstudio123"; // TODO implement security methods
	private bool useOnlineScores;

	#region singleton
	private static WebCommunicator _instance;
	public static WebCommunicator instance {
		get	{
			if (_instance == null) {
				_instance = Object.FindObjectOfType<WebCommunicator>();
				if (!_instance)	{
					GameObject singleton = new GameObject();
					singleton.name = "WebCommunicator";
					_instance = singleton.AddComponent<WebCommunicator>();
				}
				DontDestroyOnLoad(_instance.gameObject);
			}
			return _instance;
		}
	}
	#endregion

	void Awake(){
		if (_instance == null) {
			_instance = this;
			DontDestroyOnLoad(this);
		}
		else if (this != _instance)	Destroy(gameObject);

		URL = Settings.OnlineScoresURL;
		useOnlineScores = Settings.UseOnlineScores;
	}

	public void SendMessage(string msg) {
		if(useOnlineScores) {
			StartCoroutine(TrySendMessage(msg));
		}
	}

	IEnumerator TrySendMessage(string message) {
		WWWForm form = new WWWForm();
		form.AddField("message", message);
		form.AddField("hashcode", hashcode);

		WWW request = new WWW(URL, form);
		yield return request;

		if(request.error != null) Debug.Log(request.error);
		else {
			Debug.Log("\"" + message + "\" - sent succesfully"); // Any returned data
			request.Dispose();
		}
	}

	public void SendPlayerJSON(PlayerInfoPacket[] playerPackets) {
		PlayerInfoPacketJSON[] packets = new PlayerInfoPacketJSON[playerPackets.Length];
		OnlineJSONPacket finalJSON = new OnlineJSONPacket(playerPackets.Length);

		for(int i = 0; i < playerPackets.Length; i++) {
			PlayerInfoPacketJSON player = new PlayerInfoPacketJSON();
			player.playerName = playerPackets[i].playerName;
			player.playerTeam = playerPackets[i].playerTeam == Settings.HeroTeam ? "Hero" : "Villain";
			player.playerScore = playerPackets[i].score;
			finalJSON.players[i] = player;
		}

		string json = JsonUtility.ToJson(finalJSON);
		StartCoroutine(TrySendPlayerJSON(json));
	}

	IEnumerator TrySendPlayerJSON(string json) {
		WWWForm form = new WWWForm();
		form.AddField("playerPackets", json);
		form.AddField("hashcode", hashcode);

		WWW request = new WWW(URL, form);
		yield return request;

		if(request.error != null) Debug.Log(request.error);
		else {
//			Debug.Log(request.text);
			Debug.Log("JSON send success");
			request.Dispose();
		}
	}

}
