using UnityEngine;
using System.Collections;
using smoothstudio.heroesandvillains.player.events;
using UnityEngine.UI;

public class DropdownFieldView : MonoBehaviour {

	public string inputKey;
	private Dropdown dropdown;

	void Awake() {
		dropdown = gameObject.GetComponent<Dropdown>();
		dropdown.onValueChanged.AddListener(OnInputChange);
	}

	public void OnInputChange(int selected) {
		gameObject.DispatchGlobalEvent(MenuEvent.InputFieldChange, new object[]{ new InputFieldVO(inputKey, dropdown.options[selected].text) });
	}

}
