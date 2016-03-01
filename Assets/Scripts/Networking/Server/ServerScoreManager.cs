using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

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
	private bool restartingGame;

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
		if(Settings.TDMGameMode) CheckWinCondition();
		if (restartingGame)	restartingGame = false; // No harm in doing this here - means we know it's a new round/game
	}

	private void CheckWinCondition() {
		if(heroScore >= Settings.TDMWinScore) {
			gameOver = true;
			gameObject.DispatchGlobalEvent(GameplayEvent.GameOver, new object[]{true});
			ServerOnlyPlayerDisplay.instance.Log("<color=cyan>Heroes win!</color>");
		}

		if(villainScore >= Settings.TDMWinScore) {
			gameOver = true;
			gameObject.DispatchGlobalEvent(GameplayEvent.GameOver, new object[]{false});
			ServerOnlyPlayerDisplay.instance.Log("<color=red>Villains win!</color>");
		}
	}

	public void RestartNextGame() {
		if (!restartingGame) {
			restartingGame = true;
			ResetGameScores ();
			gameOver.DispatchGlobalEvent (GameplayEvent.ResetGame);
			ServerPlayerManager.instance.ResetAllScores ();

		}
	}
}