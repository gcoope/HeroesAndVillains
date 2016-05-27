using UnityEngine;
using UnityEngine.Networking;

public class BasePlayerInfo : NetworkBehaviour {

	private PlayerFaint playerFaint;
	private PlayerHealth playerHealth;
	private PlayerModelChanger modelChanger;

	[Header("Player")]

	[SyncVar(hook = "OnNameChange")] 
	public string playerName = "Player Name";

	[SyncVar(hook = "OnTeamChange")] 
	public string playerTeam = Settings.HeroTeam; // TODO change this to int hero = 1 villain = 2 etc.

	[SyncVar(hook = "OnOutfitChange")] 
	public int playerOutfit = 0;


	[Header("Stats")]
	[HideInInspector]
	public int directHitDamage = Settings.DirectHitDamage;
	[HideInInspector]
	public int splashDamage = Settings.SplashDamage;
	[HideInInspector]
	public int meleeDamage = Settings.BaseMeleeDamage;
	[HideInInspector]
	public float speed = Settings.BaseMoveSpeed;
	[HideInInspector]
	public float jumpHeight = Settings.BaseJumpHeight;
	[HideInInspector]
	public float rocketJumpPower = Settings.RocketJumpPower;
	[HideInInspector]
	public bool doubleJumpEnabled = Settings.DoubleJumpEnabled;

	void Awake () {
		playerFaint = gameObject.GetComponent<PlayerFaint>();
		playerHealth = gameObject.GetComponent<PlayerHealth>();
		modelChanger = gameObject.GetComponent<PlayerModelChanger>();
	}

	public override void OnStartLocalPlayer() {
//		playerName = PlayerPrefs.GetString(PlayerPrefKeys.LocalPlayerName);
//		if(string.IsNullOrEmpty(playerName)) playerName = NameGenerator.GetRandomName();
//		playerTeam = PlayerPrefs.GetString(PlayerPrefKeys.LocalPlayerTeam);
//		playerOutfit = PlayerPrefs.GetInt (PlayerPrefKeys.LocalPlayerOutfit);

		gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");	 // Only on local client - So we can't click ourselves, bit hacky but works

		// Positioning
		Transform spawnPos = SpawnManager.instance.GetFreeSpawn(playerTeam == Settings.HeroTeam);
		transform.position = spawnPos.position;
		transform.rotation = spawnPos.rotation;

		Invoke("LateOnStartLocalPlayer", 1f); // Hack to tell server our info - seems to be called just before player manager is setup

	}

	void LateOnStartLocalPlayer() {
		if(ServerPlayerManager.instance) CmdTellServerPlayerInfo(playerName, playerTeam, playerOutfit);
	}

	public override void OnStartClient () {		
		OnNameChange(playerName);
		OnTeamChange(playerTeam);
		OnOutfitChange(playerOutfit);
		base.OnStartClient ();
	}

	public void OnTeamChange(string team) {
		modelChanger.SetupModel(playerTeam);
	}

	public void OnNameChange(string name) {
		gameObject.name = name;
	}

	public void OnOutfitChange(int outfit) {
		modelChanger.SetupOutfit (outfit);
	}

	[Command]
	private void CmdTellServerPlayerInfo(string pName, string pTeam, int outfitIndex) {
		playerName = pName;
		playerTeam = pTeam;
		playerOutfit = outfitIndex;
		gameObject.name = playerName;

		modelChanger.SetupModel(playerTeam);
		modelChanger.SetupOutfit(playerOutfit);
		RpcSetClientPlayerInfo(playerName, playerTeam, playerOutfit);

		NetworkInstanceId _netID = gameObject.GetComponent<NetworkIdentity>().netId;
		PlayerInfoPacket packet = new PlayerInfoPacket(playerName, playerTeam, _netID);
		if(ServerPlayerManager.instance) ServerPlayerManager.instance.RegisterPlayer(packet);
	}

	[ClientRpc]
	private void RpcSetClientPlayerInfo(string pName, string pTeam, int pOutfitIndex) {
//		if(isLocalPlayer) return;
		playerName = pName;
		playerTeam = pTeam;
		playerOutfit = pOutfitIndex;
		gameObject.name = playerName;
		modelChanger.SetupModel(playerTeam);
		modelChanger.SetupOutfit (playerOutfit);
	}
}
