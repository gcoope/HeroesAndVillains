using UnityEngine;
using System.Collections;

public class PrimaryAttackExplosion : MonoBehaviour {

	public string audioPrefabName = "";

	private bool playedOnce = false;

	void OnEnable() {
		if (!playedOnce) {
			playedOnce = true;
			return;
		}
		audioPrefabName.PlaySound (transform.position);
	}
}
