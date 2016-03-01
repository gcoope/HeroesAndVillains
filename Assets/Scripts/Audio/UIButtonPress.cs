using System;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonPress : MonoBehaviour {

	void Awake() {
		Button b = gameObject.GetComponent<Button> ();
		if (b != null) {
			b.onClick.AddListener (OnClickHandler);
		}
	}

	void OnClickHandler() {
		AudioKeys.UIClick.PlaySound ();
	}

}
