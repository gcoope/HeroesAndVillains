using UnityEngine;
using System.Collections;
using smoothstudio.heroesandvillains.player.events;
using UnityEngine.UI;
using UnityEngine.Networking;

public class BasePlayerInfo : NetworkBehaviour {

	private string rName = "abcdefghijklmnopqrstuvwxyz";

	[SyncVar(hook = "OnFaintChange")] public bool isFainted = false;

	private PlayerHUD playerHUD;

	public enum Team {
		ATTACKER,
		DEFENDER
	}

	[Header("Player")]
	[SyncVar] public string playerName = "Name";

	[Header("Stats")]
	[SyncVar(hook = "OnHealthChange")] private int health = Settings.BaseHealth; // Variable per class? Maybe have a stat setup at the start
	public int armor = Settings.BaseArmour;  // as above
	public int damage = Settings.BaseDamage;
	public int meleeDamage = Settings.BaseMeleeDamage;
	public float speed = Settings.BaseMoveSpeed;
	public float jumpHeight = Settings.BaseJumpHeight;

	public PersonalObjectPooler personalObjectPooler; // Object pool specifically for this player

	void Awake () {
		personalObjectPooler = gameObject.GetComponent<PersonalObjectPooler>();
		personalObjectPooler.Init();

		playerName = "Player-" + rName [Random.Range (0, 25)] + rName [Random.Range (0, 25)] + rName [Random.Range (0, 25)] + rName [Random.Range (0, 25)]; // TODO This needs to be a proper unique ID, maybe a server side ID manager
		gameObject.name = playerName;

		playerHUD = gameObject.GetComponent<PlayerHUD>();
		UpdateHealthText ();
	}

	void Update() {
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

	public void TakeDamage(int amount) {
		if (isFainted)
			return;
		health -= amount;
		Debug.Log(playerName + " took " + amount + " damage -  " + health + " hp left");
		UpdateHealthText ();
		if(health <= 0) Faint();
	}

	private void UpdateHealthText() {
		playerHUD.UpdateHealthText(health);
	}

	private void OnHealthChange(int hp) {
		health = hp;
		CheckFainted();
		UpdateHealthText();
	}

	private void CheckFainted() {
		if (isFainted)
			return;
		if(health <= 0) Faint();
	}

	public void Faint() {
		Debug.Log(playerName + " has fainted!");
		gameObject.DispatchGlobalEvent(PlayerEvent.PlayerFaint, new object[] {this.playerName});
	}

	private void PlayerRespawn() {
		gameObject.DispatchGlobalEvent(PlayerEvent.PlayerRespawn, new object[] {this.playerName});
		health = Settings.BaseHealth;
	}

	private void OnFaintChange(bool fainted) {
		isFainted = fainted;
		if(!isFainted) {
			gameObject.DispatchGlobalEvent(PlayerEvent.PlayerRespawn);			
		}
	}
}
