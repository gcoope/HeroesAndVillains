using System;

public class PlayerInfoPacket
{
	public string playerName;
	public string playerTeam;

	public PlayerInfoPacket(string name, string team) {
		this.playerName = name;
		this.playerTeam = team;
	}
}

