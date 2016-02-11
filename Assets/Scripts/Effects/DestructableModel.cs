﻿using UnityEngine;
using System.Collections;
using smoothstudio.heroesandvillains.physics;
using UnityEngine.Networking;

public class DestructableModel : MonoBehaviour {

	[SerializeField] private GameObject currentModelRoot;
	[SerializeField] private GameObject destroyedModel;
	[SerializeField] private int shotsToDestroy = 3;

	private int currentHits = 0;
	protected bool hasChanged = false;

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
}
