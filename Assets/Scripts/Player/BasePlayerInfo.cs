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
	private PlayerName nameText;
	private PlayerModelChanger modelChanger;

	[Header("Player")]

	[SyncVar(hook = "OnNameChange")] 
	public string playerName = "Player Name";

	[SyncVar(hook = "OnTeamChange")] 
	public string playerTeam = Settings.HeroTeam;

	[Header("Stats")]
	public int armor = Settings.BaseArmour;  // as above
	public int damage = Settings.BaseDamage;
	public int meleeDamage = Settings.BaseMeleeDamage;
	public float speed = Settings.BaseMoveSpeed;
	public float jumpHeight = Settings.BaseJumpHeight;
	public bool doubleJumpEnabled = Settings.DoubleJumpEnabled;
	public float rocketJumpPower = Settings.RocketJumpPower;

	void Awake () {
		playerHUD = gameObject.GetComponent<PlayerHUD>();
		playerFaint = gameObject.GetComponent<PlayerFaint>();
		playerHealth = gameObject.GetComponent<PlayerHealth>();
		nameText = gameObject.GetComponent<PlayerName>();
		modelChanger = gameObject.GetComponent<PlayerModelChanger>();
//		gameObject.AddGlobalEventListener(NetworkEvent.GotPlayerInfo, GotPlayerInfo);
	}

	public override void OnStartLocalPlayer() {
		playerName = PlayerPrefs.GetString(PlayerPrefKeys.LocalPlayerName);
		if(string.IsNullOrEmpty(playerName)) playerName = NameGenerator.GetRandomName();
		playerTeam = PlayerPrefs.GetString(PlayerPrefKeys.LocalPlayerTeam);
		CmdTellServerPlayerInfo(playerName, playerTeam);

		gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");	 // Only on local client - So we can't click ourselves, bit hacky...

		// Positioning
		Transform spawnPos = SpawnManager.instance.GetFreeSpawn(playerTeam == Settings.HeroTeam);
		transform.position = spawnPos.position;
		transform.rotation = spawnPos.rotation;
	}

	public override void OnStartClient () {		
		base.OnStartClient ();
		nameText = gameObject.GetComponent<PlayerName>();
		OnNameChange(playerName);
		OnTeamChange(playerTeam);
	}
		
	private void RequestInfo(string id) {
		PlayerInfoPacket info = ServerPlayerManager.instance.GetPlayerInfo(id);
		if(!string.IsNullOrEmpty(info.playerName)) {
			playerName = info.playerName;
			playerTeam = info.playerTeam;
			gameObject.name = playerName;

			OnNameChange(playerName);
			OnTeamChange(playerTeam);
		} else {
			Debug.Log("Info came back null - not updating player");
		}
	}

	private void GotPlayerInfo(EventObject evt) {
		PlayerInfoPacket info = (PlayerInfoPacket)evt.Params[0];
		playerName = info.playerName;
		playerTeam = info.playerTeam;
		gameObject.name = playerName;

		OnNameChange(playerName);
		OnTeamChange(playerTeam);
	}

	public void OnTeamChange(string team) {
		if(isLocalPlayer) return;
		modelChanger.SetModelColour(playerTeam);
	}

	public void OnNameChange(string name) {
		if(isLocalPlayer) return;
		nameText.SetName(name);
		gameObject.name = name;
	}

	[Command]
	private void CmdTellServerPlayerInfo(string name, string team) {
		playerName = name;
		playerTeam = team;
		gameObject.name = playerName;
		modelChanger.SetModelColour(playerTeam);
		nameText.SetName(playerName);
		RpcSetClientPlayerInfo(playerName, playerTeam);

		string _netID = gameObject.GetComponent<NetworkIdentity>().netId.ToString();
		PlayerInfoPacket packet = new PlayerInfoPacket(playerName, playerTeam, _netID);
		ServerPlayerManager.instance.RegisterPlayer(_netID, packet);
	}

	[ClientRpc]
	private void RpcSetClientPlayerInfo(string name, string team) {
		playerName = name;
		playerTeam = team;
		gameObject.name = playerName;
		modelChanger.SetModelColour(playerTeam);
		nameText.SetName(playerName);
	}
}
