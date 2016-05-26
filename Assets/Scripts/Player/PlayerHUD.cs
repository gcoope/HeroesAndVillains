using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using smoothstudio.heroesandvillains.player.events;
using DG.Tweening;
using smoothstudio.heroesandvillains.player;
using UnityStandardAssets.ImageEffects;
using UnityEngine.Networking;
using Prototype.NetworkLobby;

public class PlayerHUD : NetworkBehaviour {

	[SerializeField] private Antialiasing antiAliasEffect;
	[SerializeField] private ColorCorrectionCurves colorCorrectionEffect;
	[SerializeField] private Camera playerCamera;

	// Hitmarker stuff
	[SerializeField] private Image hitmarkerImage;
	private float showHitmarkerDuration = 0.25f;
	private bool isShowingHitmarker = false;

	public Text healthText;
	public Image healthAmountBar;
	public Text sensitivityValueLabel;
	public Text respawnText;

	public CanvasGroup ingameScreen;
	public CanvasGroup pauseScreen;
	public CanvasGroup respawnScreen;
	public CanvasGroup scoreboardScreen;
	public CanvasGroup damageIndicator;

	public Text countdownText;
	public CanvasGroup countdownScreen;
	private int currentCountdownNumber = 20;

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
		gameObject.AddGlobalEventListener(GameplayEvent.ResetGame, ResetGame);
		hitmarkerImage.enabled = false;
		damageIndicator.alpha = 0;
	}

	void Start() {
		ResetHUD ();
	}

	private void ResetHUD() {
		HideScreen(respawnScreen);
		HideScreen(scoreboardScreen);
		HideScreen(pauseScreen);
		ShowScreen(ingameScreen);
		HideScreen (countdownScreen);
	}

	void Update() {
		// TODO Move this to own video settings class?
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

	#region Gameover handling
	private void HandleGameoverEvent(EventObject evt) {
		RpcShowGameOver((bool)evt.Params[0]);
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
		if (showingScoreboard) ToggleScoreboard ();
		FadeInScreen(respawnScreen);
		HideScreen (pauseScreen);
		StartCoroutine("WaitThenShowScores");
	}

	private IEnumerator WaitThenShowScores() {
		yield return new WaitForSeconds(3.5f);
		HideScreen(respawnScreen);
		showingScoreboard = false;
		FadeInScreen (scoreboardScreen);
		showingScoreboard = true;
		FadeInScreen (countdownScreen);
		currentCountdownNumber = Settings.timeBeforeNextRound;
		countdownText.text = currentCountdownNumber.ToString ();
		StartCoroutine ("CountdownToNextGame");
	}

	private IEnumerator CountdownToNextGame() {
		while (currentCountdownNumber > 0) {
			yield return new WaitForSeconds (1f);
			currentCountdownNumber--;
			countdownText.text = currentCountdownNumber.ToString ();
		}
		FadeOutScreen (countdownScreen);
		CmdReadyForNextGame ();
	}

	[Command]
	private void CmdReadyForNextGame() {
		ServerScoreManager.instance.RestartNextGame ();
	}

	private void ResetGame(EventObject evt) {
		ResetHUD ();
		isGameOver = false;
	}
	#endregion

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
		gameObject.DispatchGlobalEvent(MenuEvent.ClientDisconnect);
//		LobbyManager.s_Singleton.KickPlayer(connectionToServer); 
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
			if(Input.GetMouseButton(1) && !showingPauseMenu && !isGameOver) {
				targetFOV = 35;
			} else {
				targetFOV = 80;
			}

			if(playerCamera.fieldOfView != targetFOV) {
				playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * 10f);
			}
		}
	}

	// Hitmarker handling
	[ClientRpc]
	public void RpcShowHitmarker() {
		if (!isShowingHitmarker) {
			if(isLocalPlayer) AudioKeys.Hitmarker.PlaySound();
			isShowingHitmarker = true;
			hitmarkerImage.enabled = true;
			StartCoroutine ("ShowHitmarkerCooldown");
		}
	}

	IEnumerator ShowHitmarkerCooldown() {
		yield return new WaitForSeconds (showHitmarkerDuration);
		isShowingHitmarker = false;
		hitmarkerImage.enabled = false;
	}

	bool damageIndicatorShowing = false;

	// Damage indicator showing
	public void ShowDamageIndicator() {
		if(!damageIndicatorShowing) {
			damageIndicatorShowing = true;
			damageIndicator.DOFade(1, 0.1f).OnComplete(()=>{
				damageIndicator.DOFade(0, 0.1f).OnComplete(()=>{
					damageIndicatorShowing = false;
				});
			});
		}
	}
}
