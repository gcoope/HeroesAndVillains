using UnityEngine;
using System.Collections;
using smoothstudio.heroesandvillains.player.events;
using UnityEngine.UI;
using UnityEngine.Networking;
using smoothstudio.heroesandvillains.player;

public class LocalPlayerSetupInfo : MonoBehaviour {

	public Text nameInputText;
	public Text selectedDropdownText;

	private string _localPlayerName;
	public string LocalPlayerName {
		get {
			return _localPlayerName;
		}
		set {
			_localPlayerName = value;
		}
	}

	private string _localPlayerTeam;
	public string LocalPlayerTeam {
		get {
			return _localPlayerTeam;
		}
		set {
			_localPlayerTeam = value;
		}
	}

	void Awake() {
		gameObject.AddGlobalEventListener(MenuEvent.HostLocal, UpdateItems); 
		gameObject.AddGlobalEventListener(MenuEvent.JoinLocal, UpdateItems); 
	}

	void Start () {
		_localPlayerName = "";
		_localPlayerTeam = Settings.HeroTeam; // default
	}

	public void OnDropdownChange() {
		if(selectedDropdownText == null) {
			selectedDropdownText = GameObject.Find("DropdownLabel").GetComponent<Text>();
		}

		_localPlayerTeam = selectedDropdownText.text == "Heroes" ? Settings.HeroTeam : Settings.VillainTeam;
	}

	public void UpdateNameInput() {
		if(nameInputText == null) {
			nameInputText = GameObject.Find("NameInputText").GetComponent<Text>();
		}

		_localPlayerName = nameInputText.text != "" ? nameInputText.text : NameGenerator.GetRandomName();
	}

	private void UpdateItems(EventObject evt) {
		OnDropdownChange();
		UpdateNameInput();
	}
}
