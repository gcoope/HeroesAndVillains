using System;

[Serializable]
public struct PlayerInfoPacket
{
	public string playerName;
	public string playerTeam;
	public string networkID;

	public PlayerInfoPacket(string name, string team, string networkID) {
		this.playerName = name;
		this.playerTeam = team;
		this.networkID = networkID;
	}
}

