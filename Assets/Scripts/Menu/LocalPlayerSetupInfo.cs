using UnityEngine;
using System.Collections;
using smoothstudio.heroesandvillains.player.events;
using UnityEngine.UI;
using UnityEngine.Networking;
using smoothstudio.heroesandvillains.player;

public class LocalPlayerSetupInfo : MonoBehaviour {

	private Text nameInputText;

	public string LocalPlayerName;
	public string LocalPlayerTeam;

	void Awake() {
		gameObject.AddGlobalEventListener(MenuEvent.HostLocal, UpdateItems); 
		gameObject.AddGlobalEventListener(MenuEvent.JoinLocal, UpdateItems); 
		gameObject.AddGlobalEventListener(MenuEvent.InputFieldChange, HandleInputFieldChange);
	}

	void Start () {
		LocalPlayerName = "";
		LocalPlayerTeam = Settings.HeroTeam; // default
	}

	private void HandleInputFieldChange(EventObject evt) {
		if(evt.Params[0] != null) {
			InputFieldVO data = (InputFieldVO)evt.Params[0];

			if(data.inputKey == InputFieldKeys.MenuNameInput) {
				LocalPlayerName = data.inputValue;
			}
			else if(data.inputKey == InputFieldKeys.MenuTeamSelectInput) {
				LocalPlayerTeam = data.inputValue;
			}
		}
	}

	public void UpdateNameInput() {
		if(nameInputText == null) {
			nameInputText = GameObject.Find("NameInputText").GetComponent<Text>();
		}

		if(LocalPlayerName == "") {
			LocalPlayerName = NameGenerator.GetRandomName();
		}
	}

	private void UpdateItems(EventObject evt) {
		UpdateNameInput();
		Debug.Log("Player name: " + LocalPlayerName + ", Player team: " + LocalPlayerTeam);
	}
}
