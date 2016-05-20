﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Prototype.NetworkLobby;
using smoothstudio.heroesandvillains.player.events;

public class MapSelectController : MonoBehaviour {

	public MapOption metroOption;
	public MapOption borgOption;
	public MapOption candyOption;
	private MapOption currentOption;


	void Start() {
		currentOption = metroOption;
		currentOption.Selected();

		metroOption.button.onClick.AddListener(()=>{SetCurrentOption(metroOption);});
		borgOption.button.onClick.AddListener(()=>{SetCurrentOption(borgOption);});
		candyOption.button.onClick.AddListener(()=>{SetCurrentOption(candyOption);});
	}

	private void SetCurrentOption(MapOption option) {
		if(option == currentOption) return;
		currentOption.Deselected();
		currentOption = option;
		currentOption.Selected();
		TellServerPreferred();
	}

	// Map names are: metro, borg and candyland (all lowercase)
	public int GetPreferredMap() {
		switch(currentOption.mapName) {
		case "metro":
			return 0;
			break;
		case "borg":
			return 1;
			break;
		case "candyland":
			return 2;
			break;
		default:
			return 0;
		}
	}

	private void TellServerPreferred() {
		gameObject.DispatchGlobalEvent(MenuEvent.LobbySetLocalPreferredMap, new object[]{GetPreferredMap()});
	}

	public void SetVoteCountValues(int metroCount, int borgCount, int candyCount) {
		metroOption.SetVoteCount(metroCount);
		borgOption.SetVoteCount(borgCount);
		candyOption.SetVoteCount(candyCount);
	}
}
