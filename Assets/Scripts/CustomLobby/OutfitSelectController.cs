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
		gameObject.DispatchGlobalEvent (MenuEvent.LobbyOutfitSelected, new object[]{Random.Range(0,9)}); // Do this once at the start otherwise something isn't getting set
		outfitButtons[chosenOutfitIndex].Selected();
	}

	public void SelectOutfit(EventObject evt) {
		if (outfitButtons [chosenOutfitIndex] != null) {
			outfitButtons [chosenOutfitIndex].Deselected ();
			chosenOutfitIndex = (int)evt.GetParam (0); // presume only 1 param
			outfitButtons [chosenOutfitIndex].Selected ();
		}
	}

}
