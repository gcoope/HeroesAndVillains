using UnityEngine;
using UnityEngine.Networking;
using smoothstudio.heroesandvillains.physics;

[NetworkSettings(channel=1)]
public class SplashDamagerCollider : NetworkBehaviour {

	[SyncVar]
	public PlayerInfoPacket playerPacket;

	public void SetOwner(PlayerInfoPacket infoPacket) {
		playerPacket = infoPacket;
	}

	void OnTriggerEnter(Collider col) {
		if(col.CompareTag(ObjectTagKeys.Player)) {
			col.gameObject.GetComponent<PlayerHealth>().TakeDamage(20, playerPacket);
		}

		if(col.CompareTag(ObjectTagKeys.DestructableObject)) {
			col.gameObject.GetComponent<DestructableModel>().Hit();
		}

		if(col.CompareTag(ObjectTagKeys.MoveableObject)) {
			col.gameObject.GetComponent<FauxGravityBody>().AddExplosionForce(transform.position, 15f);
		}
	}
}
