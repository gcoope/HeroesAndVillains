﻿using UnityEngine;
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

	private string ipAddress = "localhost";

	void Awake() {	
		gameObject.AddGlobalEventListener(MenuEvent.JoinLocal, JoinAsClient);
		gameObject.AddGlobalEventListener(MenuEvent.HostLocal, HostLan);
		gameObject.AddGlobalEventListener(MenuEvent.HostServer, StartAsServer);
		gameObject.AddGlobalEventListener(MenuEvent.ClientDisconnect, Disconnect);
		gameObject.AddGlobalEventListener(MenuEvent.CloseGame, CloseApp);
		gameObject.AddGlobalEventListener(MenuEvent.InputFieldChange, HandleInputFieldChange);
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
		Debug.Log("Attempting to join: " + NetworkManager.singleton.networkAddress);
		CheckHostAddress ();
		NetworkManager.singleton.StartClient();
	}

	public void HostLan() { HostLan(null); }
	public void HostLan(EventObject evt = null) {
		NetworkManager.singleton.StopHost();
		CheckHostAddress();
//		Debug.Log("Hosting local at: " + NetworkManager.singleton.networkAddress);
		NetworkManager.singleton.StartHost();
	}

	public void StartAsServer() { StartAsServer(null); }
	public void StartAsServer(EventObject evt = null) {
		NetworkManager.singleton.StopServer();
//		Debug.Log("Hosting server at: " + NetworkManager.singleton.networkAddress);
		NetworkManager.singleton.networkAddress = "localhost";
		NetworkManager.singleton.StartServer();
	}

	public void CloseApp() { CloseApp(null); }
	public void CloseApp(EventObject evt = null) {
//		Debug.Log("Exiting Application");
		Application.Quit();
	}

	public void Disconnect(EventObject evt) {
		if(NetworkManager.singleton.IsClientConnected()) {
			NetworkManager.singleton.client.Disconnect();
		}
	}

	private void CheckHostAddress() {
		if(string.IsNullOrEmpty(NetworkManager.singleton.networkAddress)) NetworkManager.singleton.networkAddress = "localhost";
	}
}