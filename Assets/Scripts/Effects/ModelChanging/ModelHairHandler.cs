using UnityEngine;
using System.Collections;

public class ModelHairHandler : MonoBehaviour {

	public GameObject[] hairModels;
	private GameObject currentHairModel;
	private int currentIndex = 0; // for checks

	void Awake() {
		foreach (GameObject g in hairModels) {
			g.SetActive (false);
		}
	}

	public void SetHairIndex(int index) {
		if(currentHairModel != null && currentHairModel.activeSelf && index == currentIndex) return;
		currentIndex = index;
		if(currentHairModel != null) currentHairModel.SetActive (false);		
		currentHairModel = hairModels [index];
		currentHairModel.SetActive (true);
	}
}
