using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using smoothstudio.heroesandvillains.player.events;

public class ServerPlayerManager : NetworkBehaviour {

	public static ServerPlayerManager instance {
		get {
			if(_instance == null) {
				_instance = GameObject.FindObjectOfType<ServerPlayerManager>();
			}
			return _instance;
		}
	}
	private static ServerPlayerManager _instance;

	private Dictionary<string, PlayerInfoPacket> playersDict = new Dictionary<string, PlayerInfoPacket>();

	public void RegisterPlayer(string netID, PlayerInfoPacket player) {
		playersDict.Add(netID, player);
		ServerOnlyPlayerDisplay.instance.Log(player.playerName + " connected");
	}

	public void UnregisterPlayer(string playerID) {
		if(ServerOnlyPlayerDisplay.instance != null) ServerOnlyPlayerDisplay.instance.Log(playersDict[playerID].playerName + " disconected");
		playersDict.Remove(playerID);
	}
		
	public PlayerInfoPacket GetPlayerInfo(string netID) {
		if(playersDict.ContainsKey(netID)) {
			return playersDict[netID];
		} else {
			Debug.Log(netId + " is not present in playersDict");
			return new PlayerInfoPacket();
		}
	}

	public Dictionary<string, PlayerInfoPacket> GetAllConntectedPlayersInfo() {
		return playersDict;
	}
}