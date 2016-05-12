using UnityEngine;
using smoothstudio.heroesandvillains.player.events;
using UnityEngine.UI;

public class LocalPlayerSetupInfo : MonoBehaviour {

	private string localPlayerName;
	private string localPlayerTeam;

	void Awake() {
		gameObject.AddGlobalEventListener(MenuEvent.HostLocal, UpdateItems); 
		gameObject.AddGlobalEventListener(MenuEvent.JoinLocal, UpdateItems); 
		gameObject.AddGlobalEventListener(MenuEvent.CreateOnlineRoom, UpdateItems); 
		gameObject.AddGlobalEventListener(MenuEvent.JoinDefaultRoom, UpdateItems); 
		gameObject.AddGlobalEventListener(MenuEvent.InputFieldChange, HandleInputFieldChange);
	}

	// TODO Get info from lobby

	void Start () {
		localPlayerName = "";
		localPlayerTeam = Settings.HeroTeam;
	}

	private void HandleInputFieldChange(EventObject evt) {
		if(evt.Params[0] != null) {
			InputFieldVO data = (InputFieldVO)evt.Params[0];

			if(data.inputKey == InputFieldKeys.MenuNameInput) {
				localPlayerName = data.inputValue;
			}
			else if(data.inputKey == InputFieldKeys.MenuTeamSelectInput) {
				if(data.inputValue == "Heroes") {
					localPlayerTeam = Settings.HeroTeam;
				} else {
					localPlayerTeam = Settings.VillainTeam;
				}
			}
		}
	}

	public void UpdateNameInput() {
		if(string.IsNullOrEmpty(localPlayerName)) {
			localPlayerName = NameGenerator.GetRandomName();
		}

		PlayerPrefs.SetString(PlayerPrefKeys.LocalPlayerName, localPlayerName);
	}

	private void UpdateItems(EventObject evt) {
		UpdateNameInput();
		PlayerPrefs.SetString(PlayerPrefKeys.LocalPlayerTeam, localPlayerTeam);
	}
}
