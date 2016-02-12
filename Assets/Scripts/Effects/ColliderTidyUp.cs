using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ColliderTidyUp : MonoBehaviour {

	public float stayDuration = 0.25f;

	void Start () {
		StartCoroutine("TidyUp");
	}

	IEnumerator TidyUp() {
		yield return new WaitForSeconds(stayDuration);
		Destroy(gameObject);
	}
}
