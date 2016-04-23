using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ParticleTidyUp : MonoBehaviour {

	public float stayDuration = 0.25f;

	void OnEnable () {
		StartCoroutine("TidyUp");
	}

	IEnumerator TidyUp() {
		yield return new WaitForSeconds(stayDuration);
		ObjectPooler.instance.RecycleToPool(gameObject);
	}
}
