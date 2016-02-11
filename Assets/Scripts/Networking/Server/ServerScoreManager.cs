using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using smoothstudio.heroesandvillains.player.events;

public class ServerScoreManager : NetworkBehaviour {

	public static ServerPlayerManager instance {
		get {
			if(_instance == null) {
				_instance = GameObject.FindObjectOfType<ServerPlayerManager>();
			}
			return _instance;
		}
	}
	private static ServerPlayerManager _instance;

	private Dictionary<NetworkInstanceId, PlayerInfoPacket> playersDict = new Dictionary<NetworkInstanceId, PlayerInfoPacket>();

	public void RegisterPlayer(NetworkInstanceId netID, PlayerInfoPacket player) {
		playersDict.Add(netID, player);
		ServerOnlyPlayerDisplay.instance.Log(player.playerName + " connected");
	}

	public void UnregisterPlayer(NetworkInstanceId playerID) {
		playersDict.Remove(playerID);
		ServerOnlyPlayerDisplay.instance.Log(playersDict[playerID].playerName + " disconected");
	}

	//	public PlayerInfoPacket GetPlayerInfo(string netID) {
	//		if(playersDict.ContainsKey(netID)) {
	//			return playersDict[netID];
	//		} else {
	//			Debug.Log(netId + " is not present in playersDict");
	//			return new PlayerInfoPacket();
	//		}
	//	}

	public Dictionary<NetworkInstanceId, PlayerInfoPacket> GetAllConntectedPlayersInfo() {
		return playersDict;
	}
}