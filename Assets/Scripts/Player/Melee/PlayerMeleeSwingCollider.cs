using UnityEngine;
using System.Collections;

public class PlayerMeleeSwingCollider : MonoBehaviour {
	
	public PlayerMeleeSwing parentMeleeSwing;
	
	void Start () {
		//parentMeleeSwing = transform.parent.GetComponent<PlayerMeleeSwing> ();
	}	

	public void OnTriggerEnter(Collider col) {
		parentMeleeSwing.MeleeSwingHit (col);
	}
	
}
