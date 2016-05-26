using UnityEngine;
using System.Collections;
using smoothstudio.heroesandvillains.player.events;

public class GameModeSelectController : MonoBehaviour {
	
	public GameOption arenaOption;
	public GameOption ctfOption;
	public GameOption zoneOption;
	public GameOption superiorOption;
	private GameOption currentOption;


	void Start() {
		currentOption = arenaOption;
		currentOption.Selected();

		arenaOption.button.onClick.AddListener(()=>{SetCurrentOption(arenaOption);});
		ctfOption.button.onClick.AddListener(()=>{SetCurrentOption(ctfOption);});
		zoneOption.button.onClick.AddListener(()=>{SetCurrentOption(zoneOption);});
		superiorOption.button.onClick.AddListener(()=>{SetCurrentOption(superiorOption);});
	}

	private void SetCurrentOption(GameOption option) {
		if(option == currentOption) return;
		currentOption.Deselected();
		currentOption = option;
		currentOption.Selected();
		TellServerPreferred();
	}

	// Map names are: metro, borg and candyland (all lowercase)
	public int GetPreferredMode() {
		switch(currentOption.gameName) {
		case "arena":
			return 0;
			break;
		case "ctf":
			return 1;
			break;
		case "zone":
			return 2;
			break;
		case "superiority":
			return 3;
			break;
		default:
			return 0;
		}
	}

	private void TellServerPreferred() {
		gameObject.DispatchGlobalEvent(MenuEvent.LobbySetLocalPreferredGameMode, new object[]{GetPreferredMode()});
	}

	public void SetGameVoteCountValues(int arena, int ctf, int zone, int superior) {
		arenaOption.SetVoteCount(arena);
		ctfOption.SetVoteCount(ctf);
		zoneOption.SetVoteCount(zone);
		superiorOption.SetVoteCount(superior);
	}
}
