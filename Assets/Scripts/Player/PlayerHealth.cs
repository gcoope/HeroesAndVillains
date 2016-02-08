using UnityEngine;
using System.Collections;
using smoothstudio.heroesandvillains.player.events;
using UnityEngine.UI;
using UnityEngine.Networking;
using smoothstudio.heroesandvillains.player;
using DG.Tweening;

public class PlayerHealth : NetworkBehaviour {
	
	[SyncVar] public bool isFainted = false;

	private BasePlayerInfo playerInfo;
	private PlayerHUD playerHUD;
	private PlayerFaint playerFaint;
	[SerializeField] private Transform playerCameraTransform;

	[SyncVar(hook = "OnHealthChange")] 
	public int Health = Settings.BaseHealth; // Variable per class? Maybe have a stat setup at the start	

	void Awake () {
		playerHUD = gameObject.GetComponent<PlayerHUD>();
		playerInfo = gameObject.GetComponent<BasePlayerInfo>();
		playerFaint = gameObject.GetComponent<PlayerFaint>();
		UpdateHealthText ();
	}

	
	void Update() {

		if(Health <= 0) {
			Faint();
		}

		if(!isLocalPlayer) return;
		if(Input.GetKeyDown(KeyCode.F9)) {
			TakeDamage(25, playerInfo.playerName, playerInfo.playerTeam, true);
		}	
		if(isFainted) {
			if(Input.GetKeyDown(KeyCode.R)) {
				PlayerRespawn();
			}
		}
	}

	public void TakeDamage(int amount, string fromPlayerName = "", string fromPlayerTeam = "", bool forceDmg = false) {
		if (isFainted) return;

		if(!forceDmg && !string.IsNullOrEmpty(fromPlayerTeam)) {
			if(fromPlayerTeam == playerInfo.playerTeam) {
				return;
			}
		}

		playerCameraTransform.DOShakePosition(0.2f, new Vector3(0.2f, 0.2f, 0), 1);

		Health -= amount;
		UpdateHealthText ();
		if(Health <= 0) {
			Faint(fromPlayerName);
		}
	}
	
	private void UpdateHealthText() {
		playerHUD.UpdateHealthText(Health);
	}
	
	private void OnHealthChange(int hp) {
		Health = hp;
		if(isLocalPlayer) UpdateHealthText();
	}
	
	public void Faint(string fromPlayer = "") {
		isFainted = true;

		if(isLocalPlayer) {
			playerFaint.CmdFaint(gameObject);
			playerHUD.PlayerHasFainted();
		}

		if(!string.IsNullOrEmpty(fromPlayer) && isLocalPlayer) {
			Debug.Log(playerInfo.playerName + " killed by " + fromPlayer);
		}
	}
	
	private void PlayerRespawn() {
		isFainted = false;

		playerFaint.CmdRespawn(gameObject);
		playerHUD.PlayerHasRespawned();
		Health = Settings.BaseHealth;
		UpdateHealthText();
		Debug.Log(gameObject.name + " has respawned!");
	}

	[Command]
	private void CmdLogSomething(string msg) {
		ServerOnlyPlayerDisplay.instance.Log(msg);
	}
}
