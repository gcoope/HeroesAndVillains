using UnityEngine;
using System.Collections;
using smoothstudio.heroesandvillains.player.events;
using smoothstudio.heroesandvillains.player;
using UnityEngine.Networking;
using smoothstudio.heroesandvillains.physics;

public class PlayerFaint : NetworkBehaviour {

	private bool isHero;
	public bool isFainted = false;
	[SerializeField] private ModelMaterialHandler materialFader;
	private PlayerHealth playerHealth;

	void Awake() {
		playerHealth = gameObject.GetComponent<PlayerHealth>();
	}

	void Start() {
		isHero = gameObject.GetComponent<BasePlayerInfo>().playerTeam == Settings.HeroTeam;
	}

	void Update() {
		if(isLocalPlayer) {
			if(isFainted && Input.GetKeyDown(KeyCode.R)) {
				CmdRequestRespawn();
			}
			
			if(Input.GetKeyDown(KeyCode.Alpha5)) {
				CmdAddScore(netId, Settings.ScorePerKill, playerHealth.thisPlayerInfo.playerTeam == Settings.HeroTeam); // findme Points added here
			}
		}
	}

	[Server]
	public void FaintOnServer() {
		isFainted = true;
		materialFader.FadeOut();
		RpcFaintOnClient();

		// Score and message handling
		if(playerHealth.lastPlayerToDamage.playerName == playerHealth.thisPlayerInfo.playerName) {
			if(playerHealth.thisPlayerInfo.playerTeam == Settings.HeroTeam) {
				CmdLogSomething("<color=cyan>" + playerHealth.lastPlayerToDamage.playerName + "</color> destroyed themselves!");
			} else {
				CmdLogSomething("<color=red>" + playerHealth.lastPlayerToDamage.playerName + "</color> destroyed themselves!");
			}
		} else {
			if(playerHealth.thisPlayerInfo.playerTeam == Settings.HeroTeam) {
				CmdLogSomething("<color=red>" + playerHealth.lastPlayerToDamage.playerName + "</color> destroyed <color=cyan>" +  playerHealth.thisPlayerInfo.playerName + "</color>");
			} else {
				CmdLogSomething("<color=cyan>" + playerHealth.lastPlayerToDamage.playerName + "</color> destroyed <color=red>" +  playerHealth.thisPlayerInfo.playerName + "</color>");
			}
			CmdAddScore(playerHealth.lastPlayerToDamage.networkID, Settings.ScorePerKill, playerHealth.lastPlayerToDamage.playerTeam == Settings.HeroTeam); // findme Points added here
		}

	}

	[ClientRpc]
	public void RpcFaintOnClient() {
		isFainted = true;
		GetComponent<PlanetPlayerMove>().enabled = false;
		GetComponent<PlayerAttack>().enabled = false;
		GetComponent<PlayerMeleeSwing>().enabled = false;
		GetComponent<Collider>().enabled = false;

		materialFader.FadeOut();
//		GetComponent<PlayerRagdoll>().EnableRagdoll(GetComponent<Rigidbody>().velocity);

		if(isLocalPlayer) {
			GetComponent<PlayerGravityBody>().enabled = false;
			if(GetComponent<Rigidbody>() != null) GetComponent<Rigidbody>().velocity = Vector3.zero;
		}

		playerHealth.playerHUD.PlayerHasFainted(playerHealth.lastPlayerToDamage); // Displays death screen for client
	}


	[Command]
	private void CmdRequestRespawn() {
		if(isFainted) {			
			ServerRespawn();
			RpcLocalRespawn();
		}
	}

	[Server]
	private void ServerRespawn() {
		isFainted = false;
		materialFader.FadeIn();
		playerHealth.ResetHealthAfterRespawn();
	}

	[ClientRpc]
	private void RpcLocalRespawn() {
		isFainted = false;

		Transform spawnPos = SpawnManager.instance.GetFreeSpawn(GetComponent<BasePlayerInfo>().playerTeam == Settings.HeroTeam); // TODO Better spawning system
		transform.position = spawnPos.position;
		transform.rotation = spawnPos.rotation;

		GetComponent<PlanetPlayerMove>().enabled = true;
		GetComponent<PlayerAttack>().enabled = true;
		GetComponent<PlayerMeleeSwing>().enabled = true;
		GetComponent<Collider>().enabled = true;

		materialFader.FadeIn(true);
//		GetComponent<PlayerRagdoll>().HideRagdoll();

		if(isLocalPlayer) {
			GetComponent<PlayerGravityBody>().enabled = true;
			if(GetComponent<Rigidbody>() != null) GetComponent<Rigidbody>().velocity = Vector3.zero;
		}

		playerHealth.ResetHealthAfterRespawn();
	}

	[Command]
	private void CmdAddScore(NetworkInstanceId id, int amount, bool isHeroTeam) {
		ServerPlayerManager.instance.AddScore(id, amount);
		ServerScoreManager.instance.AddScore(isHeroTeam, amount);
	}

	[Command]
	private void CmdLogSomething(string msg) {
		ServerOnlyPlayerDisplay.instance.Log(msg);
	}

}
