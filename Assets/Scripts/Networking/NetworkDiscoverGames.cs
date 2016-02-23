using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using smoothstudio.heroesandvillains.player.events;

public class NetworkDiscoverGames : NetworkDiscovery {

	void Awake() {
//		gameObject.AddGlobalEventListener(MenuEvent.HostServer, StartServerBroadcasting);
//		gameObject.AddGlobalEventListener(MenuEvent.JoinLocal, StartListeningAsClient);
	}

	void OnDisable() {
		StopBroadcast();
	}

	public void StartServerBroadcasting(EventObject evt) {
		StartAsServer();
	}

	public void StopServerBroadcasting() {
		StopBroadcast();
	}

	public void StartListeningAsClient(EventObject evt) {
		StartAsClient();
	}

	public void StopListeningAsClient() {
		StopBroadcast();
	}

	public override void OnReceivedBroadcast (string fromAddress, string data)
	{
		Debug.Log("Found address: " + fromAddress + " with data: " + data);
		base.OnReceivedBroadcast (fromAddress, data);
	}

}
