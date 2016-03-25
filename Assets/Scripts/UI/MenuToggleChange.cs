using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using smoothstudio.heroesandvillains.player.events;

public class MenuToggleChange : MonoBehaviour {

	public enum Type {
		MOUSE_SMOOTH
	}

	public Type optionType;
	private Toggle tog;

	void Start () {
		tog = gameObject.GetComponent<Toggle>();
		tog.onValueChanged.AddListener(OnChange);
	}

	void OnChange(bool val) {
		if(optionType == Type.MOUSE_SMOOTH) {
			if(val) {
				gameObject.DispatchGlobalEvent(MenuEvent.EnableMouseSmoothing);
			} else {
				gameObject.DispatchGlobalEvent(MenuEvent.DisableMouseSmoothing);
			}
		}
	}
}
