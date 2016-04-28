using UnityEngine;
using System.Collections;
using smoothstudio.heroesandvillains.player.events;
using UnityEngine.UI;
using UnityEngine.Networking;
using smoothstudio.heroesandvillains.player;
using DG.Tweening;

public class PlayerHealth : NetworkBehaviour {
	

	public BasePlayerInfo thisPlayerInfo;
	public PlayerHUD playerHUD;
	private PlayerFaint playerFaint;
//	[SerializeField] private Transform playerCameraTransform;
	public PlayerInfoPacket lastPlayerToDamage;

//	[SyncVar(hook = "OnHealthChange")] 
//	[SyncVar] 
	private int currentHealth = Settings.BaseHealth;

	void Awake () {
		playerHUD = gameObject.GetComponent<PlayerHUD>();
		thisPlayerInfo = gameObject.GetComponent<BasePlayerInfo>();
		playerFaint = gameObject.GetComponent<PlayerFaint>();
		UpdateHealthText ();
	}

	void Update() {
		if(!isLocalPlayer) return;
		if(Input.GetKeyDown(KeyCode.F9) && Input.GetKey(KeyCode.LeftControl)) {
			CmdTakeDamageOnServer(25, new PlayerInfoPacket(thisPlayerInfo.playerName, thisPlayerInfo.playerTeam, netId), true);
		}	
	}

	[Command]
	public void CmdTakeDamageOnServer(int amount, PlayerInfoPacket fromPlayerInfo, bool forceDmg) {
		TakeDamageOnServer(amount, fromPlayerInfo, forceDmg);
	}

	[Server]
	public void TakeDamageOnServer(int amount, PlayerInfoPacket fromPlayerInfo, bool forceDamage = false) {
		lastPlayerToDamage = fromPlayerInfo;
		if(!forceDamage && !string.IsNullOrEmpty(fromPlayerInfo.playerTeam)) { // No friendly damage
			if(fromPlayerInfo.playerTeam.Equals(thisPlayerInfo.playerTeam)) {
				return;
			}
		}
		if(currentHealth > 0) currentHealth -= amount; // So that it doesn't show negative health for players
		if(currentHealth <= 0 && !playerFaint.isFainted) {
			playerFaint.FaintOnServer();
		}
		RpcUpdatePlayerHeath(fromPlayerInfo, currentHealth);
	}

	[ClientRpc]
	private void RpcUpdatePlayerHeath(PlayerInfoPacket fromPlayerInfo, int newHealthValue) {
		lastPlayerToDamage = fromPlayerInfo;
		currentHealth = newHealthValue;
		UpdateHealthText ();
	}

	public void ResetHealthAfterRespawn() {
		playerHUD.PlayerHasRespawned();
		currentHealth = Settings.BaseHealth;
		UpdateHealthText();
	}

	private void UpdateHealthText() {
		playerHUD.UpdateHealthText(currentHealth);
	}

	[Command]
	private void CmdLogSomething(string msg) {
		ServerOnlyPlayerDisplay.instance.Log(msg);
	}
}
