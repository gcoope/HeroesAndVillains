﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using smoothstudio.heroesandvillains.player.events;
using DG.Tweening;
using smoothstudio.heroesandvillains.player;
using UnityStandardAssets.ImageEffects;
using UnityEngine.Networking;

public class PlayerHUD : NetworkBehaviour {

	[SerializeField] private Antialiasing antiAliasEffect;
	[SerializeField] private ColorCorrectionCurves colorCorrectionEffect;
	[SerializeField] private Camera playerCamera;

	public Text healthText;
	public Image healthAmountBar;
	public Text sensitivityValueLabel;
	public Text respawnText;

	public CanvasGroup ingameScreen;
	public CanvasGroup pauseScreen;
	public CanvasGroup respawnScreen;
	public CanvasGroup scoreboardScreen;

	private bool showingScoreboard = false;
	private bool showingPauseMenu = false;

	private BasePlayerInfo playerInfo;
	private PlanetPlayerMove planetPlayerMove;
	private PlayerAttack playerAttack;
	[SerializeField] private Slider mouseSensSlider;

	private float targetFOV = 80;
	private bool isGameOver = false;

	void Awake() {
		gameObject.AddGlobalEventListener(PlayerEvent.PlayerRespawn, PlayerHasRespawned);
		planetPlayerMove = gameObject.GetComponent<PlanetPlayerMove>();
		playerAttack = gameObject.GetComponent<PlayerAttack>();
		playerInfo = gameObject.GetComponent<BasePlayerInfo>();
		mouseSensSlider.onValueChanged.AddListener(OnSliderChange);

		gameObject.AddGlobalEventListener(GameplayEvent.GameOver, HandleGameoverEvent);
	}

	void Start() {
		HideScreen(respawnScreen);
		HideScreen(scoreboardScreen);
		HideScreen(pauseScreen);
		ShowScreen(ingameScreen);
	}

	void Update() {
		// TODO Move this to own video settings class
		if(Input.GetKeyDown(KeyCode.F5)) {
			if(antiAliasEffect != null) {
				antiAliasEffect.enabled = !antiAliasEffect.enabled;
			}
		}
		if(Input.GetKeyDown(KeyCode.F6)) {
			if(colorCorrectionEffect != null) {
				colorCorrectionEffect.enabled = !colorCorrectionEffect.enabled;
			}
		}
		
		if(isGameOver) return;
		if(Input.GetKeyDown(KeyCode.Tab)) {
			if(!showingPauseMenu) ToggleScoreboard();
		}

		if(Input.GetKeyDown(KeyCode.Escape)) {
			TogglePauseMenu();
		}

		HandleZooming();
	}

	// Gameover handling
	private void HandleGameoverEvent(EventObject evt) {
		bool isHero = (bool)evt.Params[0];
		RpcShowGameOver(isHero);
	}

	[ClientRpc]
	private void RpcShowGameOver(bool isHero) {
		isGameOver = true;

		string gameOverText = "GAME OVER\n";
		if(isHero) {
			gameOverText += "<color=cyan>Heroes win!</color>";
		} else {
			gameOverText += "<color=red>Villains win!</color>";
		}
		respawnText.text = gameOverText;
		FadeOutScreen(ingameScreen);
		FadeInScreen(respawnScreen);
		StartCoroutine("WaitTheShowScores");
	}

	private IEnumerator WaitTheShowScores() {
		yield return new WaitForSeconds(3.5f);
		showingScoreboard = false;
		ToggleScoreboard();
	}

	// Faint handling
	public void PlayerHasFainted(PlayerInfoPacket lastPlayerTohurt) {

		string newRespawnText = "";

		// Set respawn text
		if(lastPlayerTohurt.playerName == playerInfo.playerName) {
			if(playerInfo.playerTeam == Settings.HeroTeam)	newRespawnText = "<color=cyan>You</color> destroyed yourself!";
			else newRespawnText = "<color=red>You</color> destroyed yourself!";
		} else { // Wasn't self-kill
			if(playerInfo.playerTeam == Settings.HeroTeam) newRespawnText = "<color=red>" + lastPlayerTohurt.playerName + "</color> destroyed <color=cyan>you!</color>";
			else newRespawnText = "<color=cyan>" + lastPlayerTohurt.playerName + "</color> destroyed <color=red>you!</color>";
		}

		newRespawnText += "\nPress R to respawn";
		respawnText.text = newRespawnText;

		FadeOutScreen(ingameScreen);
		FadeInScreen(respawnScreen);
	}

	public void PlayerHasRespawned(EventObject evt = null) {
		ShowScreen(ingameScreen);
		HideScreen(respawnScreen);
		HideScreen(scoreboardScreen);
		HideScreen(pauseScreen);
	}

	public void HideAllHUD() {
		HideScreen(ingameScreen);
		HideScreen(respawnScreen);
		HideScreen(scoreboardScreen);
	}

	public void UpdateHealthText(int health) {
		healthText.text = health.ToString();
		healthAmountBar.fillAmount = health * 0.01f;
	}

	// Scoreboard showing
	private void ToggleScoreboard() {
		if(showingScoreboard) {
			HideScreen(scoreboardScreen);
			ShowScreen(ingameScreen);
		}
		else { 
			ShowScreen(scoreboardScreen);
			HideScreen(ingameScreen);
		}
		showingScoreboard = !showingScoreboard;
	}

	// Pause menu showing
	public void TogglePauseMenu() {
		if(showingPauseMenu) {
			HideScreen(pauseScreen);
			HideScreen(scoreboardScreen);
			showingScoreboard = false;
			ShowScreen(ingameScreen);
			planetPlayerMove.SetAllowControl(true);
			playerAttack.SetAllowAnyInput(true);
		}
		else { 
			ShowScreen(pauseScreen);
			HideScreen(scoreboardScreen);
			HideScreen(ingameScreen);
			showingScoreboard = false;
			planetPlayerMove.SetAllowControl(false);
			playerAttack.SetAllowAnyInput(false);
		}
		showingPauseMenu = !showingPauseMenu;
	}

	// Mouse sensitivity
	private void OnSliderChange(float sens) {
		planetPlayerMove.SetSensitivity(sens);
		sensitivityValueLabel.text = sens.ToString("F2");
	}

	// Exiting
	public void Exit() {
		gameObject.DispatchGlobalEvent(MenuEvent.ClientDisconnect); // Not working
	}

	// util
	private void ShowScreen(CanvasGroup screen) {
		screen.alpha = 1;
		screen.interactable = true;
	}

	private void HideScreen(CanvasGroup screen) {
		screen.alpha = 0;
		screen.interactable = false;
	}

	private void FadeInScreen(CanvasGroup screen) {
		screen.alpha = 0;
		screen.DOFade(1, 0.5f);
	}

	private void FadeOutScreen(CanvasGroup screen) {
		screen.alpha = 1;
		screen.DOFade(0, 0.5f);
	}

	// Camera zoom
	private void HandleZooming() {
		if(playerCamera != null) {
			// TODO Console controller zooming
			if(Input.GetMouseButton(1)) {
				targetFOV = 35;
			} else {
				targetFOV = 80;
			}

			if(playerCamera.fieldOfView != targetFOV) {
				playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * 10f);
			}
		}
	}

}
