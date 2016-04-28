using UnityEngine;
using System.Collections;

public class RagdollTester : MonoBehaviour {

	void Update () {
	
		if(Input.GetKeyDown(KeyCode.Space)) {
			GetComponent<Rigidbody> ().AddForce (Vector3.up * 50, ForceMode.Impulse);
			Debug.Log ("hit");
		}

	}
}
