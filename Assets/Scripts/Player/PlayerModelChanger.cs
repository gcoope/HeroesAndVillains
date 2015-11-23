using System;
using UnityEngine;

public class PlayerModelChanger : MonoBehaviour {

	public Material heroMat;
	public Material villainMat;
	public Transform rootModelObject;

	public void SetModelColour(string team) {
		Material toApply = team == Settings.HeroTeam ? heroMat : villainMat;

		foreach(Transform child in rootModelObject) {
			if(child.GetComponent<Renderer>() != null) {
				child.GetComponent<Renderer>().material = toApply; 
			}
		}
	}

}

