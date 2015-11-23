using UnityEngine;
using System.Collections;

public class ParticleTidyUp : MonoBehaviour {

	public float stayDuration = 5f;

	void Start () {
		StartCoroutine("TidyUp");
	}

	IEnumerator TidyUp() {
		yield return new WaitForSeconds(stayDuration);
		Destroy(gameObject);
	}
}
