using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Net;

public class ServerOnlyPlayerDisplay : NetworkBehaviour {
	
	public static ServerOnlyPlayerDisplay instance {
		get {
			if(_instance == null) {
				_instance = GameObject.FindObjectOfType<ServerOnlyPlayerDisplay>();
			}
			return _instance;
		}
	}
	private static ServerOnlyPlayerDisplay _instance;

	private string bigConsoleString;
	[SerializeField] private Text consoleText;

	private bool isVisible = false;

	public override void OnStartServer ()
	{
		base.OnStartServer ();
		isVisible = true;
		transform.GetChild(0).gameObject.SetActive(true);
		Log("Server started...");
		string localIp = "";
		foreach(IPAddress ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList) {
			if(ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork){
				localIp = ip.ToString();
			}
		}
		Log("IP: " + localIp);

		Log("Registering server callbacks");
		//NetworkServer.RegisterHandler(MsgType.Connect, ClientConnected);
//		NetworkServer.RegisterHandler(MsgType.Command, CommandSent);

	}

	[Server]
	public void Log(string msg) {
		if(isServer) {
			bigConsoleString = ("> " + msg + "\n") + bigConsoleString;
			UpdateText();
		}
	}

	private void UpdateText() {
		consoleText.text = bigConsoleString;
	}

	void Update() {
		if(isServer) {
			if(Input.GetKeyDown(KeyCode.F4)) {
				isVisible = !isVisible;
				transform.GetChild(0).gameObject.SetActive(isVisible);
			}

			if(Input.GetKeyDown(KeyCode.H)) {
				Log("Hacking!");
			}
		}
	}

	private void ClientConnected(NetworkMessage msg) {
		Log("Connection ID: " + msg.conn.connectionId + " \nIP: " + msg.conn.address);
	}
}
