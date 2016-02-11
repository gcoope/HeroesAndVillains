using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using smoothstudio.heroesandvillains.player.events;
using UnityEngine.Networking;

public class ScoreUIController : NetworkBehaviour {

	[SerializeField] ScoreboardItem[] heroPlayers;
	[SerializeField] ScoreboardItem[] villainPlayers;

	private Dictionary<NetworkInstanceId, PlayerInfoPacket> localPlayerDetails;

	void Awake() {
		ClearBoard();
		localPlayerDetails = new Dictionary<NetworkInstanceId, PlayerInfoPacket>();
		gameObject.AddGlobalEventListener(NetworkEvent.NewPlayerConnected, AddNewPlayer);
		gameObject.AddGlobalEventListener(NetworkEvent.PlayerDisconnected, RemovePlayer);
		gameObject.AddGlobalEventListener(NetworkEvent.UpdatePlayerInfo, UpdatePlayerDetails);
	}


	private void ClearBoard() {
		for(int i = 0; i < heroPlayers.Length; i++) {
			heroPlayers[i].Empty();
		}
		for(int i = 0; i < villainPlayers.Length; i++) {
			villainPlayers[i].Empty();
		}
	}

	private void AddNewPlayer(EventObject evt) {
		PlayerInfoPacket[] playerInfoArray = (PlayerInfoPacket[])evt.Params[0];
		for(int i = 0; i < playerInfoArray.Length; i++) {
			if(!localPlayerDetails.ContainsKey(playerInfoArray[i].networkID)) {
				localPlayerDetails.Add(playerInfoArray[i].networkID, playerInfoArray[i]);
			}
		}
		PopulateScoreboard();
	}

	private void RemovePlayer(EventObject evt) {
		PlayerInfoPacket playerToRemove = (PlayerInfoPacket)evt.Params[0];
		if(localPlayerDetails.ContainsKey(playerToRemove.networkID)) {
			localPlayerDetails.Remove(playerToRemove.networkID);
		}
		PopulateScoreboard();
	}

	private void UpdatePlayerDetails(EventObject evt) {
		PlayerInfoPacket playerToUpdate = (PlayerInfoPacket)evt.Params[0];
		localPlayerDetails[playerToUpdate.networkID] = playerToUpdate;
		PopulateScoreboard();
	}

	private void PopulateScoreboard() {
		ClearBoard();
		foreach(NetworkInstanceId playerID in localPlayerDetails.Keys) {

			if(localPlayerDetails[playerID].playerTeam == Settings.HeroTeam) {				
				for(int i = 0; i < heroPlayers.Length; i++) {
					if(heroPlayers[i].isEmpty) {
						heroPlayers[i].Populate(localPlayerDetails[playerID].playerName, localPlayerDetails[playerID].score.ToString());
						break;
					}
				}

			} else if(localPlayerDetails[playerID].playerTeam == Settings.VillainTeam) {
				for(int i = 0; i < villainPlayers.Length; i++) {
					if(villainPlayers[i].isEmpty) {
						villainPlayers[i].Populate(localPlayerDetails[playerID].playerName, localPlayerDetails[playerID].score.ToString());
						break;
					}
				}
			}
		}

	}
}
