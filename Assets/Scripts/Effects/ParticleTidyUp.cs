using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ParticleTidyUp : NetworkBehaviour {

	public float stayDuration = 0.25f;

	void Start () {
		StartCoroutine("TidyUp");
	}

	IEnumerator TidyUp() {
		yield return new WaitForSeconds(stayDuration);
		Destroy(gameObject);
	}
}
