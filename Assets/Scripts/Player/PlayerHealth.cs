using UnityEngine;
using System.Collections;
using smoothstudio.heroesandvillains.player.events;
using UnityEngine.UI;
using UnityEngine.Networking;
using smoothstudio.heroesandvillains.player;
using DG.Tweening;

public class PlayerHealth : NetworkBehaviour {
	
	[SyncVar] public bool isFainted = false;

	private BasePlayerInfo thisPlayerInfo;
	private PlayerHUD playerHUD;
	private PlayerFaint playerFaint;
	[SerializeField] private Transform playerCameraTransform;
	private PlayerInfoPacket lastPlayerToDamage;

	[SyncVar(hook = "OnHealthChange")] 
	public int Health = Settings.BaseHealth; // Variable per class? Maybe have a stat setup at the start	

	void Awake () {
		playerHUD = gameObject.GetComponent<PlayerHUD>();
		thisPlayerInfo = gameObject.GetComponent<BasePlayerInfo>();
		playerFaint = gameObject.GetComponent<PlayerFaint>();
		UpdateHealthText ();
	}

	
	void Update() {
		if(!isLocalPlayer) return;

		if(Health <= 0) {
			if(!isFainted) Faint();
		}

		if(Input.GetKeyDown(KeyCode.F9)) {
			TakeDamage(25, new PlayerInfoPacket(thisPlayerInfo.playerName, thisPlayerInfo.playerTeam, netId), true);
		}	
		if(isFainted) {
			if(Input.GetKeyDown(KeyCode.R)) {
				PlayerRespawn();
			}
		}
			
		if(Input.GetKeyDown(KeyCode.Alpha5)) {
			CmdAddScore(netId, Settings.ScorePerKill, thisPlayerInfo.playerTeam == Settings.HeroTeam); // findme Points added here
		}
	}

	public void TakeDamage(int amount, PlayerInfoPacket fromPlayerInfo, bool forceDmg = false) {
		if (isFainted) return;

		lastPlayerToDamage = fromPlayerInfo;

		if(!forceDmg && !string.IsNullOrEmpty(fromPlayerInfo.playerTeam)) { // No friendly damage
			if(fromPlayerInfo.playerTeam.Equals(thisPlayerInfo.playerTeam)) {
				return;
			}
		}

		Health -= amount;
		UpdateHealthText ();
		if(Health <= 0) {
			Faint();
		}
	}

	[Server]
	public void ServerTakeDamage(int amount, PlayerInfoPacket fromPlayerInfo) {
		lastPlayerToDamage = fromPlayerInfo;
		if(!string.IsNullOrEmpty(fromPlayerInfo.playerTeam)) { // No friendly damage
			if(fromPlayerInfo.playerTeam.Equals(thisPlayerInfo.playerTeam)) {
				return;
			}
		}
		Health -= amount;
		RpcTakeDamage(fromPlayerInfo);
	}

	[ClientRpc]
	private void RpcTakeDamage(PlayerInfoPacket fromPlayerInfo) {
		lastPlayerToDamage = fromPlayerInfo;
		UpdateHealthText ();
		if(Health <= 0 && !isFainted) {
			Faint();
		}
	}

	[Command]
	private void CmdAddScore(NetworkInstanceId id, int amount, bool isHeroTeam) {
		ServerPlayerManager.instance.AddScore(id, amount);
		ServerScoreManager.instance.AddScore(isHeroTeam, amount);
	}

	private void OnHealthChange(int hp) {
		Health = hp;
		if(isLocalPlayer) { 
			UpdateHealthText();
			if(Health <= 0 && !isFainted) {
				Faint();
			}
		}
	}

	private void UpdateHealthText() {
		playerHUD.UpdateHealthText(Health);
	}
	
	[Client]
	public void Faint() {
		isFainted = true;

		if(isLocalPlayer) {
			playerFaint.CmdFaint(gameObject);
			playerHUD.PlayerHasFainted(lastPlayerToDamage);
		}

		if(isLocalPlayer) {
			if(lastPlayerToDamage.playerName == thisPlayerInfo.playerName) {
				if(thisPlayerInfo.playerTeam == Settings.HeroTeam) {
					CmdLogSomething("<color=cyan>" + lastPlayerToDamage.playerName + "</color> destroyed themselves!");
				} else {
					CmdLogSomething("<color=red>" + lastPlayerToDamage.playerName + "</color> destroyed themselves!");
				}
			} else { // Wasn't suicide
				if(thisPlayerInfo.playerTeam == Settings.HeroTeam) {
					CmdLogSomething("<color=red>" + lastPlayerToDamage.playerName + "</color> destroyed <color=cyan>" +  thisPlayerInfo.playerName + "</color>");
				} else {
					CmdLogSomething("<color=cyan>" + lastPlayerToDamage.playerName + "</color> destroyed <color=red>" +  thisPlayerInfo.playerName + "</color>");
				}
				CmdAddScore(lastPlayerToDamage.networkID, Settings.ScorePerKill, thisPlayerInfo.playerTeam == Settings.HeroTeam); // findme Points added here
			}
		}

	}
	
	private void PlayerRespawn() {
		isFainted = false;
		playerFaint.CmdRespawn(gameObject);
		playerHUD.PlayerHasRespawned();
		Health = Settings.BaseHealth;
		UpdateHealthText();
	}

	[Command]
	private void CmdLogSomething(string msg) {
		ServerOnlyPlayerDisplay.instance.Log(msg);
	}
}
