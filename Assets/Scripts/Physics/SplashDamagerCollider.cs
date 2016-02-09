using UnityEngine;
using UnityEngine.Networking;

[NetworkSettings(channel=3)]
public class SplashDamagerCollider : NetworkBehaviour {

	[SyncVar] public string playerName;
	[SyncVar] public string playerTeam;

	public void SetOwner(string name, string team) {
		playerName = name;
		playerTeam = team;
	}

	void OnTriggerEnter(Collider col) {
		if(col.CompareTag(ObjectTagKeys.Player)) {
			col.gameObject.GetComponent<PlayerHealth>().TakeDamage(20, playerName, playerTeam);
		}

		if(col.CompareTag(ObjectTagKeys.DestructableObject)) {
			col.gameObject.GetComponent<DestructableModel>().Hit();
		}
	}
}
