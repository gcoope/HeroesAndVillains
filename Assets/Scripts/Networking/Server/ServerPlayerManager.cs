using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using smoothstudio.heroesandvillains.player.events;

public class ServerPlayerManager : NetworkBehaviour {

	public static ServerPlayerManager instance {
		get {
			if(_instance == null) {_instance = GameObject.FindObjectOfType<ServerPlayerManager>();}
			return _instance;
		}
	}
	private static ServerPlayerManager _instance;

	private Dictionary<NetworkInstanceId, PlayerInfoPacket> playerDetails = new Dictionary<NetworkInstanceId, PlayerInfoPacket>();

	[Server]
	public void RegisterPlayer(PlayerInfoPacket playerInfo) {
		if(!playerDetails.ContainsKey(playerInfo.networkID)) {
			ServerOnlyPlayerDisplay.instance.Log(playerInfo.playerName + " connected");
			playerDetails.Add(playerInfo.networkID, playerInfo);

			PlayerInfoPacket[] packets = new PlayerInfoPacket[playerDetails.Count];
			playerDetails.Values.CopyTo(packets, 0);
			RpcUpdatePlayers(packets);
		}
	}

	[ClientRpc]
	private void RpcUpdatePlayers(PlayerInfoPacket[] playerInfoArray) {
		gameObject.DispatchGlobalEvent(NetworkEvent.NewPlayerConnected, new object[]{playerInfoArray});
	}

	[Server]
	public void UnregisterPlayer(NetworkInstanceId playerID) {
		if(playerDetails.ContainsKey(playerID)) {
			RpcRemovePlayer(playerDetails[playerID]);
			ServerOnlyPlayerDisplay.instance.Log(playerDetails[playerID].playerName + " disconnected");
			playerDetails.Remove(playerID);
		}
	}

	[ClientRpc]
	private void RpcRemovePlayer(PlayerInfoPacket playerToRemove) {
		gameObject.DispatchGlobalEvent(NetworkEvent.PlayerDisconnected, new object[]{playerToRemove});
	}

	public Dictionary<NetworkInstanceId, PlayerInfoPacket> GetAllConntectedPlayersInfo() {
		return playerDetails;
	}

	[Server]
	public void AddScore(NetworkInstanceId id, int newScore) {
		if(playerDetails.ContainsKey(id)) {
			playerDetails[id] = new PlayerInfoPacket( // This doesn't feel right..
				playerDetails[id].playerName,
				playerDetails[id].playerTeam,
				playerDetails[id].networkID,
				playerDetails[id].score + newScore
			);
			RpcUdateScore(playerDetails[id]);
		}
	}

	[Server]
	public void ResetAllScores() {
		List<NetworkInstanceId> keyList = new List<NetworkInstanceId> (playerDetails.Keys);
		foreach (NetworkInstanceId key in keyList) {
			playerDetails[key] = new PlayerInfoPacket( // This doesn't feel right either..
				playerDetails[key].playerName,
				playerDetails[key].playerTeam,
				playerDetails[key].networkID,
				0
			);
			RpcUdateScore(playerDetails[key]);
		}
	}

	[ClientRpc]
	private void RpcUdateScore(PlayerInfoPacket updatedPlayerDetails) {
		gameObject.DispatchGlobalEvent(NetworkEvent.UpdatePlayerInfo, new object[]{updatedPlayerDetails});
	}

}