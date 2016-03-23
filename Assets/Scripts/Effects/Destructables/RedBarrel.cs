using UnityEngine;
using System.Collections;

public class RedBarrel : DestructableModel {

	public override void Change () {
		gameObject.GetComponent<Collider>().enabled = false;
		base.Change();
	}

	public override void Reset (EventObject evt = null) {
		gameObject.GetComponent<Collider>().enabled = true;
		base.Reset();
	}

}
