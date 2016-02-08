using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using smoothstudio.heroesandvillains.player.events;
using DG.Tweening;
using smoothstudio.heroesandvillains.player;

public class PlayerHUD : MonoBehaviour {

	public Text healthText;
	public Image healthAmountBar;
	public Text sensitivityValueLabel;

	public CanvasGroup ingameScreen;
	public CanvasGroup pauseScreen;
	public CanvasGroup respawnScreen;
	public CanvasGroup scoreboardScreen;

	private bool showingScoreboard = false;
	private bool showingPauseMenu = false;

	private PlanetPlayerMove planetPlayerMove;
	private PlayerAttack playerAttack;
	[SerializeField] private Slider mouseSensSlider;

	void Awake() {
		gameObject.AddGlobalEventListener(PlayerEvent.PlayerFaint, PlayerHasFainted);
		gameObject.AddGlobalEventListener(PlayerEvent.PlayerRespawn, PlayerHasRespawned);
		planetPlayerMove = gameObject.GetComponent<PlanetPlayerMove>();
		playerAttack = gameObject.GetComponent<PlayerAttack>();
		mouseSensSlider.onValueChanged.AddListener(OnSliderChange);
	}

	void Start() {
		HideScreen(respawnScreen);
		HideScreen(scoreboardScreen);
		HideScreen(pauseScreen);
		ShowScreen(ingameScreen);
	}

	void Update() {
		if(Input.GetKeyDown(KeyCode.Tab)) {
			if(!showingPauseMenu) ToggleScoreboard();
		}

		if(Input.GetKeyDown(KeyCode.Escape)) {
			TogglePauseMenu();
		}
	}

	public void PlayerHasFainted(EventObject evt = null) {
		HideScreen(ingameScreen);
		ShowScreen(respawnScreen);
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

}
