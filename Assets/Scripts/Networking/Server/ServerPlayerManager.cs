using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class ServerPlayerManager : MonoBehaviour {
	
	private static Dictionary<string, BasePlayerInfo> playersDict = new Dictionary<string, BasePlayerInfo>();

	public static void RegisterPlayer(string netID, BasePlayerInfo player) {
		playersDict.Add(netID, player);
		player.transform.name = netID;
		Debug.Log("Registered: "+ netID);
	}

	public static void UnregisterPlayer(string playerID) {
		playersDict.Remove(playerID);
		Debug.Log("Unregistered: "+ playerID);
	}

	public BasePlayerInfo GetPlayerInfo(string netID) {
		return playersDict[netID];
	}
}
