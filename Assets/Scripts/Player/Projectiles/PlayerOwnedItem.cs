using UnityEngine;
using System.Collections;

public class PlayerOwnedItem : MonoBehaviour {

	public string playerName;
	public string playerTeam;

	public void SetOwner(string name, string team) {
		this.playerName = name;
		this.playerTeam = team;
	}

}
