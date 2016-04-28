using UnityEngine;
using UnityEngine.Networking;

public class BasePlayerInfo : NetworkBehaviour {

	private PlayerFaint playerFaint;
	private PlayerHealth playerHealth;
//	private PlayerName nameText;
	private PlayerModelChanger modelChanger;

	[Header("Player")]

	[SyncVar(hook = "OnNameChange")] 
	public string playerName = "Player Name";

	[SyncVar(hook = "OnTeamChange")] 
	public string playerTeam = Settings.HeroTeam; // TODO change this to int hero = 1 villain = 2 etc.

	[Header("Stats")]
	public int overShield = Settings.OverShield;  // as above
	public int damage = Settings.BaseDamage;
	public int meleeDamage = Settings.BaseMeleeDamage;
	public float speed = Settings.BaseMoveSpeed;
	public float jumpHeight = Settings.BaseJumpHeight;
	public float rocketJumpPower = Settings.RocketJumpPower;
	public bool doubleJumpEnabled = Settings.DoubleJumpEnabled;

	void Awake () {
		playerFaint = gameObject.GetComponent<PlayerFaint>();
		playerHealth = gameObject.GetComponent<PlayerHealth>();
		modelChanger = gameObject.GetComponent<PlayerModelChanger>();
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
		OnNameChange(playerName);
		OnTeamChange(playerTeam);
	}

	public void OnTeamChange(string team) {
		if(isLocalPlayer) return;
		modelChanger.SetModelColour(playerTeam);
	}

	public void OnNameChange(string name) {
		if(isLocalPlayer) return;
		gameObject.name = name;
	}

	[Command]
	private void CmdTellServerPlayerInfo(string name, string team) {
		playerName = name;
		playerTeam = team;
		gameObject.name = playerName;
		modelChanger.SetModelColour(playerTeam);
		RpcSetClientPlayerInfo(playerName, playerTeam);

		NetworkInstanceId _netID = gameObject.GetComponent<NetworkIdentity>().netId;
		PlayerInfoPacket packet = new PlayerInfoPacket(playerName, playerTeam, _netID);
		ServerPlayerManager.instance.RegisterPlayer(packet);
	}

	[ClientRpc]
	private void RpcSetClientPlayerInfo(string name, string team) {
		playerName = name;
		playerTeam = team;
		gameObject.name = playerName;
		modelChanger.SetModelColour(playerTeam);
	}
}
