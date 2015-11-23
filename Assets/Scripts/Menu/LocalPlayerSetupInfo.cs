using UnityEngine;
using System.Collections;
using smoothstudio.heroesandvillains.player.events;
using UnityEngine.UI;
using UnityEngine.Networking;
using smoothstudio.heroesandvillains.player;

public class LocalPlayerSetupInfo : MonoBehaviour {

	public Text nameInputText;
	public Text selectedDropdownText;

	public string LocalPlayerName;
	public string LocalPlayerTeam;

	void Start () {
		LocalPlayerName = "";
		LocalPlayerTeam = Settings.HeroTeam; // default
	}

	public void OnDropdownChange() {
		LocalPlayerTeam = selectedDropdownText.text == "Heroes" ? Settings.HeroTeam : Settings.VillainTeam;
	}

	public void UpdateNameInput() {
		LocalPlayerName = nameInputText.text;
	}
}
