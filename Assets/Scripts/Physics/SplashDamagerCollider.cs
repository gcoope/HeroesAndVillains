using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using smoothstudio.heroesandvillains.player;
using System.Collections.Generic;

public class SplashDamagerCollider : NetworkBehaviour {

	[SyncVar] private string playerName;
	[SyncVar] private string playerTeam;

	void Start() {
		playerName = GetComponent<PlayerOwnedItem>().playerName;
		playerTeam = GetComponent<PlayerOwnedItem>().playerTeam;
	}

	void OnTriggerEnter(Collider col) {
		if(col.CompareTag("Player")) {
			col.gameObject.GetComponent<PlayerHealth>().TakeDamage(10, playerName, playerTeam);
		}
	}
}
