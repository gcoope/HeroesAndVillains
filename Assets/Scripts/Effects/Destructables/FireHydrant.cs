using UnityEngine;
using System.Collections;
using smoothstudio.heroesandvillains.physics;

public class FireHydrant : DestructableModel {

	void OnCollisionEnter(Collision col) {
		if(hasChanged) {
			if(col.gameObject.CompareTag(ObjectTagKeys.Player)) {
				PlayerGravityBody playerGravBod = col.gameObject.GetComponent<PlayerGravityBody>();
				if(playerGravBod != null) {
					playerGravBod.Jump(22f);
				}
				col.gameObject.GetComponent<PlayerAnimator>().CmdJump();
			}
		}
	}

	public override void Change () {
		base.Change();
	}

}
