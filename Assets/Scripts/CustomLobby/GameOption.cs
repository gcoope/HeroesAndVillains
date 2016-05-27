using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOption : MonoBehaviour {

	private Outline outline;
	[HideInInspector]
	public Button button;

	public Text voteCount;

	public string gameName;

	void Awake() {
		outline = GetComponent<Outline>();
		Deselected();
		button = GetComponent<Button>();
	}

	public void Selected() {
		outline.enabled = true;
	}

	public void Deselected() {
		outline.enabled = false;
	}

	public void SetVoteCount(int count) {
		if (voteCount != null) {
			if (voteCount.isActiveAndEnabled) voteCount.text = count.ToString ();
		}
	}
}
