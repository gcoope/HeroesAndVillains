using UnityEngine;
using System.Collections;
using smoothstudio.heroesandvillains.physics;
using UnityEngine.Networking;

public class DestructableModel : MonoBehaviour {

	[SerializeField] private GameObject currentModelRoot;
	[SerializeField] private GameObject destroyedModel;
	[SerializeField] private int shotsToDestroy = 3;

	private int currentHits = 0;
	protected bool hasChanged = false;

	void Awake() {
		gameObject.AddGlobalEventListener (GameplayEvent.ResetGame, Reset);
		currentModelRoot.SetActive(true);
		destroyedModel.SetActive(false);
	}

	public virtual void Hit() {
		currentHits++;
		if(currentHits == shotsToDestroy) {
			Change();
		}
	}

	public virtual void Change() {
		hasChanged = true;
		currentModelRoot.SetActive(false);
		destroyedModel.SetActive(true);
		//TODO trigger any script on newly shown destroyed model
	}

	public virtual void Reset(EventObject evt = null) {
		currentHits = 0;
		hasChanged = false;
		currentModelRoot.SetActive(true);
		destroyedModel.SetActive(false);
	}
}
