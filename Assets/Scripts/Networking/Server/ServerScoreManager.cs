using UnityEngine;
using UnityEngine.Networking;

public class ServerScoreManager : NetworkBehaviour {

	public static ServerScoreManager instance {
		get {
			if(_instance == null) {
				_instance = GameObject.FindObjectOfType<ServerScoreManager>();
			}
			return _instance;
		}
	}
	private static ServerScoreManager _instance;

	private int heroScore;
	private int villainScore;
	private bool gameOver;

	void Start() {
		ResetGameScores();
	}

	private void ResetGameScores() {
		gameOver = false;
		heroScore = 0;
		villainScore = 0;
	}

	public void AddScore(bool isHeroTeam, int amount) {
		if(gameOver) return;
		if(isHeroTeam) heroScore += amount;
		else villainScore += amount;
		CheckWinCondition();
	}

	private void CheckWinCondition() {
		if(heroScore >= Settings.TDMWinScore) {
			Debug.Log("Hero's win");
			gameOver = true;
			gameObject.DispatchGlobalEvent(GameplayEvent.GameOver, new object[]{true});
			ServerOnlyPlayerDisplay.instance.Log("<color=cyan>Heroes win!</color>");
		}

		if(villainScore >= Settings.TDMWinScore) {
			Debug.Log("Villains's win");
			gameOver = true;
			gameObject.DispatchGlobalEvent(GameplayEvent.GameOver, new object[]{false});
			ServerOnlyPlayerDisplay.instance.Log("<color=red>Villains win!</color>");
		}
	}
}