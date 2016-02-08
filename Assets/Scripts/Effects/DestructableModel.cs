using UnityEngine;
using System.Collections;
using smoothstudio.heroesandvillains.physics;
using UnityEngine.Networking;

public class DestructableModel : MonoBehaviour {

	[SerializeField] private GameObject currentModelRoot;
	[SerializeField] private GameObject destroyedModel;
	[SerializeField] private int shotsToDestroy = 3;

	private Collider collider;

	private int currentHits = 0;
	private bool hasChanged = false;
//	[SyncVar(hook="CheckChange")] private bool hasChanged = false;

	void Start () {
		collider = gameObject.GetComponent<Collider>();
	}

	void OnCollisionEnter(Collision col) {
		if(hasChanged) {
			if(col.gameObject.CompareTag(ObjectTagKeys.Player)) {
				col.gameObject.GetComponent<PlayerGravityBody>().Jump(35f);
			}
		}
	}

	public void Hit() {
		currentHits++;
		if(currentHits == shotsToDestroy) {
			Change();
		}
	}

	private void Change() {
		hasChanged = true;
		currentModelRoot.SetActive(false);
		destroyedModel.SetActive(true);
		//TODO trigger any script on newly shown destroyed model
	}

	private void CheckChange(bool trigger) {
		if(trigger) Change();
	}
}
