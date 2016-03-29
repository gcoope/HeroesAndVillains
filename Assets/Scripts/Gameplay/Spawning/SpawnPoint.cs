using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour {	


	private bool _isFree = true;
	public bool IsFree {
		get {
			return _isFree;
		}
		set {
			_isFree = value;
		}
	}

//	public void SpawnedOn() {
//		_isFree = false;
//		StartCoroutine("WaitASec");
////	}
//
//	IEnumerator WaitASec() {
//		yield return new WaitForSeconds(3f);
//		_isFree = true;
//	}

	void OnTriggerEnter(Collider col) {
		if(col.CompareTag(ObjectTagKeys.Player)) {
			_isFree = false;
		}
	}

	void OnTriggerStay(Collider col) {
		if(col.CompareTag(ObjectTagKeys.Player)) {
			_isFree = false;
		}
	}

	void OnTriggerExit(Collider col) {
		if(col.CompareTag(ObjectTagKeys.Player)) {
			_isFree = true;
		}
	}
}
