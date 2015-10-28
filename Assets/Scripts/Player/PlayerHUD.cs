using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using smoothstudio.heroesandvillains.player.events;

public class PlayerHUD : MonoBehaviour {

	public Text healthText;
	public Text ammoText;
	public GameObject respawnText;

	void Awake() {
		gameObject.AddGlobalEventListener(PlayerEvent.PlayerFaint, PlayerFaint);
		gameObject.AddGlobalEventListener(PlayerEvent.PlayerRespawn, PlayerRespawn);
	}

	void Start() {
		respawnText.SetActive (false);
	}

	private void PlayerFaint(EventObject evt) {	
		healthText.enabled = false;
		ammoText.enabled = false;
		respawnText.SetActive(true);
	}

	private void PlayerRespawn(EventObject ev) {
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
