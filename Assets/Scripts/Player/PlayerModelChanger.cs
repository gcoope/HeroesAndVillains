using System;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerModelChanger : NetworkBehaviour {

	public Material heroMat;
	public Material villainMat;
	public Transform rootModelObject;
	public Material currentMaterial;
	[SyncVar(hook = "OnChangeTeam")] private string playerTeam;

	public void SetModelColour(string team) {
		playerTeam = team;
		UpdateMaterial();
	}

	private void OnChangeTeam(string team) {
		SetModelColour(team);
	}

	private void UpdateMaterial() {
		currentMaterial = playerTeam == Settings.HeroTeam ? heroMat : villainMat;
		foreach(Transform child in rootModelObject) {
			if(child.GetComponent<Renderer>() != null) {
				child.GetComponent<Renderer>().material = currentMaterial; 
			}
		}
	}

	[Command]
	private void CmdTellOthersMaterial(string playerTeam) {
		playerTeam = playerTeam;
	}

}

