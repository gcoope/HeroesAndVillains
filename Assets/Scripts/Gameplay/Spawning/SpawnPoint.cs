using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour {		
	public bool CheckIsFree() {
		Collider[] overlaps = Physics.OverlapSphere(transform.position, 1.5f);
		for(int i = 0; i < overlaps.Length; i++) {
			if(overlaps[i].CompareTag(ObjectTagKeys.Player)) return false;
		}
		return true;
	}
}
