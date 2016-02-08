using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using smoothstudio.heroesandvillains.player.events;
using UnityEngine.Networking;

public class ScoreUIController : NetworkBehaviour {

	[SerializeField] ScoreboardItem[] heroPlayers;
	[SerializeField] ScoreboardItem[] villainPlayers;

	void Awake() {
		ClearBoard();

		gameObject.AddGlobalEventListener(PlayerEvent.UpdateScoreboard, (EventObject evt)=>{
			if(!isServer) CmdUpdateScoreBoard();
		});
	}

	IEnumerator Start() {
		yield return new WaitForSeconds(0.5f);
		if(!isServer && isLocalPlayer) CmdUpdateScoreBoard();
	}


	private void ClearBoard() {
		for(int i = 0; i < heroPlayers.Length; i++) {
			heroPlayers[i].Empty();
		}
		for(int i = 0; i < villainPlayers.Length; i++) {
			villainPlayers[i].Empty();
		}
	}

	[Command]
	public void CmdUpdateScoreBoard() {
		ClearBoard();

		List<PlayerInfoPacket> playersInfo = new List<PlayerInfoPacket>();

		Dictionary<string, PlayerInfoPacket> playersDict = ServerPlayerManager.instance.GetAllConntectedPlayersInfo();
		foreach(KeyValuePair<string, PlayerInfoPacket> entry in playersDict) {
			AddElement(entry.Value);
			playersInfo.Add(entry.Value);
		}

		RpcUpdateScoreboard(playersInfo.ToArray());
	}

	[ClientRpc]
	private void RpcUpdateScoreboard(PlayerInfoPacket[] playersDict) {
		ClearBoard();
		foreach(PlayerInfoPacket entry in playersDict) {
			AddElement(entry);
		}
	}

	private void AddElement(PlayerInfoPacket infoPacket) {
		if(infoPacket.playerTeam == Settings.HeroTeam) {
			foreach(ScoreboardItem item in heroPlayers) {
				if(item.isEmpty) {
					item.Populate(infoPacket.playerName, "0");
					break;
				}
			}
		} else if(infoPacket.playerTeam == Settings.VillainTeam) {
			foreach(ScoreboardItem item in villainPlayers) {
				if(item.isEmpty) {
					item.Populate(infoPacket.playerName, "0");
					break;
				}
			}
		}
	}
}
