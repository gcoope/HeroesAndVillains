using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MapOption : MonoBehaviour {

	private Outline outline;
	[HideInInspector]
	public Button button;

	public Text voteCount;

	public string mapName;

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
		if(voteCount != null && voteCount.text != null)	voteCount.text = count.ToString();
	}
}
