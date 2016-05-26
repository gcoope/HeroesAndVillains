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
		if(isLocalPlayer || gameOver) return;

		if(isHeroTeam) heroScore += amount;
		else villainScore += amount;

//		ServerOnlyPlayerDisplay.instance.Log (isHeroTeam ? "<color=cyan>Heroes scored a point</color>" : "<color=red>Villains scored a point</color>");

		if(Settings.currentGameMode == SettingsGameMode.ARENA) CheckArenaWinCondition();

		if (restartingGame)	restartingGame = false; // No harm in doing this here(?) - means we know it's a new round/game
	}

	private void CheckArenaWinCondition() {
		if (isServer) {
			if (heroScore >= Settings.TDMWinScore) {
				EndGame (true);
			}
			if (villainScore >= Settings.TDMWinScore) {
				EndGame (false);
			}
		}
	}

	private void EndGame(bool isHeroes) {
		gameOver = true;
		gameObject.DispatchGlobalEvent (GameplayEvent.GameOver, new object[]{ isHeroes });
		ServerOnlyPlayerDisplay.instance.Log (isHeroes ? "<color=cyan>Heroes win!</color>" : "<color=red>Villains win!</color>");
	}

	public void RestartNextGame() {
		if (!restartingGame) {
			restartingGame = true;
			ResetGameScores ();
			ServerPlayerManager.instance.ResetGame ();
		}
	}
}