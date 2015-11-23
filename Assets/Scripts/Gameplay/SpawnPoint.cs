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

	public void SpawnedOn() {
		_isFree = false;
	}

	IEnumerator WaitASec() {
		yield return new WaitForSeconds(3f);
		_isFree = true;
	}

}
