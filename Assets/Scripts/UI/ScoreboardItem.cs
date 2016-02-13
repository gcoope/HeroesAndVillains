using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreboardItem : MonoBehaviour {

	[SerializeField] private Text nameText;
	[SerializeField] private Text scoreText;
	public bool isEmpty = true;

	public string currentName;
	public int currentScore;

	public void Populate(string name, int score) {
		currentName = name;
		currentScore = score;
		SetName(currentName);
		SetScore(currentScore);
		isEmpty = false;
	}

	public void SetName(string name) {
		currentName = name;
		nameText.text = name;
	}

	public void SetScore(int score) {
		currentScore = score;
		scoreText.text = score.ToString();
	}

	public void Empty() {
		currentName = "";
		currentScore = 0;
		nameText.text = "";
		scoreText.text = "";
		isEmpty = true;
	}
}
