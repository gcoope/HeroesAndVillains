using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using smoothstudio.heroesandvillains.player.events;

public class OutfitOption : MonoBehaviour {

	private Outline outline;

	[HideInInspector]
	private Button button;

	public int index;

	void Awake() {
		outline = GetComponent<Outline>();
		Deselected();
		button = GetComponent<Button>();

		button.onClick.AddListener(() => gameObject.DispatchGlobalEvent (MenuEvent.LobbyOutfitSelected, new object[] { index }));
	}

	public void Selected() {
		outline.enabled = true;
	}

	public void Deselected() {
		outline.enabled = false;
	}
}
