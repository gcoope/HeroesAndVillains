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

public class StartMenuController : MonoBehaviour {


	private NetworkManager manager; 
	public InputField hostAddressText;
	public Text serverIPLabel;

	UdpClient udpSender;
	int remotePort = 7777;
	bool foundServer = false;
	bool connectedToServer = false;
	string serverIP = "";

	void Awake() {	
		gameObject.AddGlobalEventListener(MenuEvent.JoinLocal, JoinAsClient);
		gameObject.AddGlobalEventListener(MenuEvent.HostLocal, HostLan);
		gameObject.AddGlobalEventListener(MenuEvent.HostServer, StartAsServer);
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
		//StartRecievingIP();
	}

	public void HostLan() { HostLan(null); }
	public void HostLan(EventObject evt = null) {
		CheckHostAddress();
		Debug.Log("Hosting local on: " + manager.networkAddress);
		manager.StartHost();
	}

	public void StartAsServer() { StartAsServer(null); }
	public void StartAsServer(EventObject evt = null) {
		//CheckHostAddress ();
		manager.networkAddress = "localhost";
		manager.StartServer();
		//SetupBroadcastServer();
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
		if(!connectedToServer && foundServer) {
			manager.networkAddress = serverIP; 
			manager.StartClient();
			connectedToServer = true;
		}

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
				foundServer = false;
				connectedToServer = false;
			}
		}
	}

	// CLIENT ------------------------------------------------

	private UdpClient udpReceiver;

	private void StartRecievingIP() {
		Debug.Log("Starting receive IP");
		try {
			if (udpReceiver == null) {
				udpReceiver = new UdpClient (remotePort);
				udpReceiver.BeginReceive (new AsyncCallback (ReceiveData), null);
			}
		} catch (SocketException e) {
			Debug.Log (e.Message);
		}
	}

	private void ReceiveData(IAsyncResult result) {
		Debug.Log(foundServer);
		if(foundServer) return;
		Debug.Log("ReceiveData Check");
		IPEndPoint receiveIPGroup = new IPEndPoint (IPAddress.Any, remotePort);
		byte[] received;
		if (udpReceiver != null) {
			received = udpReceiver.EndReceive (result, ref receiveIPGroup);
		} else {
			return;
		}
		udpReceiver.BeginReceive (new AsyncCallback (ReceiveData), null);
		string receivedString = Encoding.ASCII.GetString (received);
		if(receivedString.Contains("UnityServer#")) {
			serverIP = receivedString.Remove(0, "UnityServer#".Length);
			Debug.Log("IP Parsed: " + serverIP);
			foundServer = true;
			udpReceiver.Close();
			udpReceiver = null;
		}
	}

	// SERVER -------------------------------------------------------

	private void SetupBroadcastServer() {
		udpSender = new UdpClient("localhost", 7777);
		IPEndPoint groupEP = new IPEndPoint(IPAddress.Broadcast, remotePort);
		udpSender.Connect(groupEP);
		InvokeRepeating("BroadcastServer", 0, 5f);
	}

	private void BroadcastServer() {
		// Get IP of this machine - the server
		string myIP = "";

		var host = Dns.GetHostEntry(Dns.GetHostName());
		foreach (IPAddress ip in host.AddressList)	{
			if (ip.AddressFamily == AddressFamily.InterNetwork)	{
				myIP = ip.ToString();
			}
		}

		if(myIP != "") {
			string msg = "UnityServer#"  + myIP;
			udpSender.Send(Encoding.ASCII.GetBytes(msg), msg.Length);
			Debug.Log("Dispatching on UDP: " + msg);
		}
	}
}