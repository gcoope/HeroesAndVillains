using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreboardItem : MonoBehaviour {

	[SerializeField] private Text nameText;
	[SerializeField] private Text scoreText;
	public bool isEmpty = true;

	public void Populate(string name, string score) {
		SetName(name);
		SetScore(score);
		isEmpty = false;
	}

	public void SetName(string name) {
		nameText.text = name;
	}

	public void SetScore(string score) {
		scoreText.text = score;
	}

	public void Empty() {
		nameText.text = "";
		scoreText.text = "";
		isEmpty = true;
	}
}
