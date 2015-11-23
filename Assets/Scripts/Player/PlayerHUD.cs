using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using smoothstudio.heroesandvillains.player.events;

public class PlayerHUD : MonoBehaviour {

	public Text healthText;
	public Text ammoText;
	public GameObject respawnText;

	void Awake() {
		gameObject.AddGlobalEventListener(PlayerEvent.PlayerFaint, PlayerHasFainted);
		gameObject.AddGlobalEventListener(PlayerEvent.PlayerRespawn, PlayerHasRespawned);
	}

	void Start() {
		respawnText.SetActive (false);
	}

	public void PlayerHasFainted() { PlayerHasFainted(null); }
	private void PlayerHasFainted(EventObject evt) {	
		healthText.enabled = false;
		ammoText.enabled = false;
		respawnText.SetActive(true);
	}

	public void PlayerHasRespawned() { PlayerHasRespawned(null); }
	private void PlayerHasRespawned(EventObject ev) {
		healthText.enabled = true;
		ammoText.enabled = true;
		respawnText.SetActive(false);
	}

	public void HideAllHUD() {
		healthText.enabled = false;
		ammoText.enabled = false;
		respawnText.SetActive(false);
	}

	public void UpdateHealthText(int health) {
		healthText.text = "Health: " + health.ToString();
	}

	public void UpdateAmmoText(int ammo) {

	}
}
