using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Net;
using System.Collections.Generic;

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
	private List<string> consoleMessages;

	[SerializeField] private Text consoleText;
	[SerializeField] private Text serverIPText;

	private bool isVisible = false;

	public override void OnStartServer () {
		base.OnStartServer ();
		if(!isClient) GameObject.Find("SceneCamera").GetComponent<AudioListener>().enabled = true;
		consoleMessages = new List<string>();
		transform.GetChild(0).gameObject.SetActive(isVisible);
		string localIp = "";
		foreach(IPAddress ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList) {
			if(ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork){
				localIp = ip.ToString();
			}
		}
		Log("Server started on " + localIp);
		Log("Registering server callbacks");
		Log("Press F4 to toggle console");

		serverIPText.text = "Server IP: " + localIp;
	}

	[Server]
	public void Log(string msg) {
		if(isServer) {
			if(consoleMessages.Count + 1 < 50) {
				consoleMessages.Add("> " + msg + "\n");
			} else {
				consoleMessages.RemoveAt(0);
				consoleMessages.Add("> " + msg + "\n");
			}
			UpdateText();
		}
	}

	private void UpdateText() {
		consoleText.text = "";
		for(int i = consoleMessages.Count-1; i > 0; i--) {
			consoleText.text += consoleMessages[i];
		}
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
