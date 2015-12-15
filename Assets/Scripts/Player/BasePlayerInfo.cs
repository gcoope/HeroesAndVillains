using UnityEngine;
using System.Collections;
using smoothstudio.heroesandvillains.player.events;
using UnityEngine.UI;
using UnityEngine.Networking;
using smoothstudio.heroesandvillains.player;

public class BasePlayerInfo : NetworkBehaviour {

	private PlayerHUD playerHUD;
	private PlayerFaint playerFaint;
	private PlayerHealth playerHealth;

	[Header("Player")]
	[SyncVar(hook = "OnNameChange")] public string playerName = "Name";
	[SyncVar(hook = "OnTeamChange")] public string playerTeam = Settings.HeroTeam;
	private NetworkInstanceId playerNetID;

	[Header("Stats")]
	public int armor = Settings.BaseArmour;  // as above
	public int damage = Settings.BaseDamage;
	public int meleeDamage = Settings.BaseMeleeDamage;
	public float speed = Settings.BaseMoveSpeed;
	public float jumpHeight = Settings.BaseJumpHeight;
	public bool doubleJumpEnabled = Settings.DoubleJumpEnabled;

	void Awake () {
		playerHUD = gameObject.GetComponent<PlayerHUD>();
		playerFaint = gameObject.GetComponent<PlayerFaint>();
		playerHealth = gameObject.GetComponent<PlayerHealth>();
	}

	public override void OnStartLocalPlayer() {
//		GetNetID();
//		SetClientID();

		// Bit messy - needs a model
		GameObject manager = GameObject.Find ("NetworkManager");
		LocalPlayerSetupInfo localPlayerInfo = manager.GetComponent<LocalPlayerSetupInfo>();
		this.playerName = localPlayerInfo.LocalPlayerName == "" ? NameGenerator.GetRandomName() : localPlayerInfo.LocalPlayerName;
		this.playerTeam = localPlayerInfo.LocalPlayerTeam;
		gameObject.name = playerName;

		CmdTellServerName(playerName);
		CmdTellServerTeam(playerTeam);

		Debug.Log("Player: " + playerName + " is on team: " + playerTeam);

		gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");	 // So we can't click ourselves #hacky

		// Positioning
		Transform spawnPos = SpawnManager.instance.GetFreeSpawn(playerTeam == Settings.HeroTeam);
		transform.position = spawnPos.position;
		transform.rotation = spawnPos.rotation;
		GetComponent<PlayerModelChanger>().SetModelColour(playerTeam);

		GetComponent<PlayerName>().SetName(playerName);
	}

	public void OnTeamChange(string team) {
		playerTeam = team;
		GetComponent<PlayerModelChanger>().SetModelColour(playerTeam);
	}

	public void OnNameChange(string name) {
		GetComponent<PlayerName>().SetName(name);
	}

	[Client]
	private void GetNetID() {
		playerNetID = GetComponent<NetworkIdentity>().netId;
		CmdTellServerName(MakeUniqueName());
	}

	private void SetClientID() {
		if(isLocalPlayer) {
			gameObject.name = playerName;
		} else {
			gameObject.name = MakeUniqueName();
		}
	}

	private string MakeUniqueName() {
		GameObject manager = GameObject.Find ("NetworkManager");
		LocalPlayerSetupInfo localPlayerInfo = manager.GetComponent<LocalPlayerSetupInfo>();
		return this.playerName = localPlayerInfo.LocalPlayerName == "" ? NameGenerator.GetRandomName() : localPlayerInfo.LocalPlayerName;
	}

	[Command]
	private void CmdTellServerName(string name) {
		gameObject.name = name;
		playerName = name;
	}

	[Command]
	private void CmdTellServerTeam(string team) {
		playerTeam = team;
	}

//	void FixedUpdate() {
//		if(gameObject.name == "" || gameObject.name == "PlanetPlayer(Clone"){
//			SetClientID();
//		}
//	}
}
