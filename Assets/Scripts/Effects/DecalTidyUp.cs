using UnityEngine;
using System.Collections;
using DG.Tweening;

public class DecalTidyUp : MonoBehaviour {

	public float stayDuration = 2f;

	void OnEnable () {
		StartCoroutine("TidyUp");
	}

	IEnumerator TidyUp() {
		yield return new WaitForSeconds(stayDuration);
		ObjectPooler.instance.RecycleToPool(gameObject);			
	}

}
