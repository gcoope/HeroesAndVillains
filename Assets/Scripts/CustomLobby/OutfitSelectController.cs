using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using smoothstudio.heroesandvillains.player.events;

public class OutfitSelectController : MonoBehaviour {

	public OutfitOption[] outfitButtons;
	public int chosenOutfitIndex = 0;


	void Awake() {
		gameObject.AddGlobalEventListener(MenuEvent.LobbyOutfitSelected, SelectOutfit); // TODO listen for this on setup controller
	}

	void Start() {
		chosenOutfitIndex = Random.Range(0,10);
		outfitButtons[chosenOutfitIndex].Selected();
	}

	public void SelectOutfit(EventObject evt) {
		outfitButtons[chosenOutfitIndex].Deselected();
		chosenOutfitIndex = (int)evt.GetParam(0); // presume only 1 item
		outfitButtons[chosenOutfitIndex].Selected();
	}

}
