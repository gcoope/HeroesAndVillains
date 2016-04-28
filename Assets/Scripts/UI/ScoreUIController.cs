using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using smoothstudio.heroesandvillains.player.events;
using UnityEngine.Networking;
using System.Linq;
using UnityEngine.UI;

public class ScoreUIController : NetworkBehaviour {

	[SerializeField] ScoreboardItem[] heroPlayersItems;
	[SerializeField] ScoreboardItem[] villainPlayersItems;

	private Dictionary<NetworkInstanceId, PlayerInfoPacket> localPlayerDetails;

	[SerializeField] private Text heroTeamScoreText;
	[SerializeField] private Text villainTeamScoreText;
	private int heroTeamTotalScore = 0;
	private int villainTeamTotalScore = 0;

	public Text heroCornerScore;
	public Text villainCornerScore;

	void Awake() {
		ClearBoard();
		localPlayerDetails = new Dictionary<NetworkInstanceId, PlayerInfoPacket>();
		gameObject.AddGlobalEventListener(NetworkEvent.NewPlayerConnected, AddNewPlayer);
		gameObject.AddGlobalEventListener(NetworkEvent.PlayerDisconnected, RemovePlayer);
		gameObject.AddGlobalEventListener(NetworkEvent.UpdatePlayerInfo, UpdatePlayerDetails);
	}


	private void ClearBoard() {
		for(int i = 0; i < heroPlayersItems.Length; i++) {
			heroPlayersItems[i].Empty();
		}
		for(int i = 0; i < villainPlayersItems.Length; i++) {
			villainPlayersItems[i].Empty();
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

		List<ScoreboardPacket> heroScorePackets = new List<ScoreboardPacket>(5);
		List<ScoreboardPacket> villainScorePackets = new List<ScoreboardPacket>(5);

		heroTeamTotalScore = 0;
		villainTeamTotalScore = 0;

		foreach(NetworkInstanceId playerID in localPlayerDetails.Keys) {
			if(localPlayerDetails[playerID].playerTeam.Equals(Settings.HeroTeam)) {	
				heroScorePackets.Add(new ScoreboardPacket(localPlayerDetails[playerID].playerName, localPlayerDetails[playerID].score));
				heroTeamTotalScore += localPlayerDetails[playerID].score;
			} else if(localPlayerDetails[playerID].playerTeam.Equals(Settings.VillainTeam)) {
				villainScorePackets.Add(new ScoreboardPacket(localPlayerDetails[playerID].playerName, localPlayerDetails[playerID].score));
				villainTeamTotalScore += localPlayerDetails[playerID].score;
			}
		}

		heroTeamScoreText.text = heroTeamTotalScore.ToString();
		villainTeamScoreText.text = villainTeamTotalScore.ToString();

		heroCornerScore.text = heroTeamTotalScore.ToString ();
		villainCornerScore.text = villainTeamTotalScore.ToString ();

		heroScorePackets = heroScorePackets.OrderByDescending(o=>o.score).ToList();
		for(int i = 0; i < heroScorePackets.Count; i++) {
			if(heroPlayersItems[i].isEmpty) {
				heroPlayersItems[i].Populate(heroScorePackets[i].name, heroScorePackets[i].score);
			}
		}

		villainScorePackets = villainScorePackets.OrderByDescending(o=>o.score).ToList();
		for(int i = 0; i < villainScorePackets.Count; i++) {
			if(villainPlayersItems[i].isEmpty) {
				villainPlayersItems[i].Populate(villainScorePackets[i].name, villainScorePackets[i].score);
			}
		}
	}
}

public class ScoreboardPacket {
	public string name;
	public int score;
	public ScoreboardPacket(string name, int score) {
		this.name = name;
		this.score = score;
	}
}
