using System;
using UnityEngine.Networking;

[Serializable]
public struct PlayerInfoPacket
{
	public string playerName;
	public string playerTeam;
	public NetworkInstanceId networkID;
	public int score;

	public PlayerInfoPacket(string name, string team, NetworkInstanceId networkID, int score = 0) {
		this.playerName = name;
		this.playerTeam = team;
		this.networkID = networkID;
		this.score = score;
	}

	public void AddScore(int newScore) {
		this.score += newScore;
	}
}

[Serializable]
public class PlayerInfoPacketJSON {
	public string playerName;
	public string playerTeam;
	public int playerScore;
}

[Serializable]
public class OnlineJSONPacket {
	public PlayerInfoPacketJSON[] players;

	public OnlineJSONPacket(int playerCount) {
		players = new PlayerInfoPacketJSON[playerCount];
	}
}