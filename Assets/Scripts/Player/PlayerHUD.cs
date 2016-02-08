using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using smoothstudio.heroesandvillains.player.events;
using DG.Tweening;

public class PlayerHUD : MonoBehaviour {

	public Text healthText;
	public Image healthAmountBar;
	public Text ammoText;
	public GameObject respawnText;
	public CanvasGroup scoreboardPanel;
	private bool showingScoreboard = false;

	void Awake() {
		gameObject.AddGlobalEventListener(PlayerEvent.PlayerFaint, PlayerHasFainted);
		gameObject.AddGlobalEventListener(PlayerEvent.PlayerRespawn, PlayerHasRespawned);
		scoreboardPanel.alpha = 0;
		showingScoreboard = false;
	}

	void Start() {
		respawnText.SetActive (false);
	}

	void Update() {
		if(Input.GetKeyDown(KeyCode.Tab)) {
			ToggleScoreboard();
		}
	}

	public void PlayerHasFainted() { PlayerHasFainted(null); }
	private void PlayerHasFainted(EventObject evt) {	
		respawnText.SetActive(true);
	}

	public void PlayerHasRespawned() { PlayerHasRespawned(null); }
	private void PlayerHasRespawned(EventObject ev) {
		respawnText.SetActive(false);
	}

	public void HideAllHUD() {
		healthText.enabled = false;
		ammoText.enabled = false;
		respawnText.SetActive(false);
	}

	public void UpdateHealthText(int health) {
		healthText.text = health.ToString();
		healthAmountBar.fillAmount = health * 0.01f;
	}

	// Scoreboard showing
	private void ToggleScoreboard() {
		showingScoreboard = !showingScoreboard;
		scoreboardPanel.alpha = showingScoreboard ? 1 : 0;
	}

}
