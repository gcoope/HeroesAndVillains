using UnityEngine.Networking;

public class PlayerOwnedItem : NetworkBehaviour {

	[SyncVar] public string playerName;
	[SyncVar] public string playerTeam;

	public void SetOwner(string name, string team) {
		playerName = name;
		playerTeam = team;
	}

}
