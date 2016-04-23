using System;
using UnityEngine;
using System.Collections;
using smoothstudio.heroesandvillains.player;
using UnityEngine.Networking;

public class SpeedBoostPowerup : NetworkBehaviour, IPowerup {

	private float respawnTime = 15f;
	[SyncVar(hook="OnEnabledChanged")] public bool isEnabled = true;
	public GameObject powerupModel;

	void Awake() {
		respawnTime = Settings.PowerupRespawnTime;
	}

	void OnTriggerEnter(Collider col) {
		if(col.CompareTag(ObjectTagKeys.Player)) {
			if (Activate ()) {
				col.GetComponent<PlanetPlayerMove> ().SetMoveSpeedPowerup ();
			}
		}
	}

	public bool Activate() {
		if (isEnabled) {
			isEnabled = false;
			powerupModel.SetActive (false);
			StartCoroutine ("Respawn");
			return true;
		}
		return false;
	}

	[Command]
	private void CmdActivate() {
		isEnabled = false;
	}

	[Command]
	private void CmdRespawn() {
		isEnabled = true;
	}

	private void OnEnabledChanged(bool enable) {
		isEnabled = enable;
		if (!isEnabled) {
			isEnabled = false;
			powerupModel.SetActive (false);
			StartCoroutine ("Respawn");
		}
	}

	IEnumerator Respawn() {
		yield return new WaitForSeconds (respawnTime);
		isEnabled = true;
		powerupModel.SetActive (true);
	}

}