using UnityEngine;
using System.Collections;
using smoothstudio.heroesandvillains.player.events;
using UnityEngine.UI;
using UnityEngine.Networking;
using smoothstudio.heroesandvillains.player;

public class PlayerHealth : NetworkBehaviour {
	
	[SyncVar] public bool isFainted = false;

	private BasePlayerInfo playerInfo;
	private PlayerHUD playerHUD;
	private PlayerFaint playerFaint;

	[SyncVar(hook = "OnHealthChange")] public int Health = Settings.BaseHealth; // Variable per class? Maybe have a stat setup at the start	

	void Awake () {
		playerHUD = gameObject.GetComponent<PlayerHUD>();
		playerInfo = gameObject.GetComponent<BasePlayerInfo>();
		playerFaint = gameObject.GetComponent<PlayerFaint>();
		UpdateHealthText ();
	}

	
	void Update() {

		if(!isFainted) CheckFainted();

		if(!isLocalPlayer) return;
		if(Input.GetKeyDown(KeyCode.F9)) {
			TakeDamage(10);
		}	
		if(isFainted) {
			if(Input.GetKeyDown(KeyCode.R)) {
				PlayerRespawn();
			}
		}
	}

	public void TakeDamage(int amount, string fromPlayerName = "", string fromPlayerTeam = "") {
		if (isFainted) return;
		
		if(fromPlayerTeam != "") {
			if(fromPlayerTeam == playerInfo.playerTeam) {
				return;
			}
		}
		
		Health -= amount;
		UpdateHealthText ();
	}
	
	private void UpdateHealthText() {
		playerHUD.UpdateHealthText(Health);
	}
	
	private void OnHealthChange(int hp) {
		Health = hp;
		UpdateHealthText();
	}
	
	private void CheckFainted() {
		if(Health <= 0) {
			Faint();
		}
	}
	
	public void Faint(BasePlayerInfo fromPlayer = null) {
		isFainted = true;

		playerFaint.LocalFaint();
		playerHUD.PlayerHasFainted();

		if(fromPlayer != null) {
			Debug.Log(playerInfo.playerName + " killed by " + fromPlayer.playerName);
		}
	}
	
	private void PlayerRespawn() {
		isFainted = false;

		playerFaint.LocalRespawn();
		playerHUD.PlayerHasRespawned();
		Health = Settings.BaseHealth;

		Debug.Log(gameObject.name + " has respawned!");
	}
}
