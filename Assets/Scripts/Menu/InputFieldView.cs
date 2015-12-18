using UnityEngine;
using System.Collections;
using smoothstudio.heroesandvillains.player.events;
using UnityEngine.UI;

public class InputFieldView : MonoBehaviour {

	public string inputKey;
	private InputField field;

	void Awake() {
		field = gameObject.GetComponent<InputField>();
		field.onValueChanged.AddListener(OnInputChange);
	}

	public void OnInputChange(string input) {
		gameObject.DispatchGlobalEvent(MenuEvent.InputFieldChange, new object[]{ new InputFieldVO(inputKey, input) });
	}

}
