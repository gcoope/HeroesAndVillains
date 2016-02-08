using System;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerModelChanger : NetworkBehaviour {

	public Material heroMat;
	public Material villainMat;
	public Transform rootModelObject;
	public Material currentMaterial;
	[SyncVar(hook = "OnChangeTeam")] private string playerTeam;

	private void OnChangeTeam(string team) {
		CmdTellOthersMaterial(team);
	}

	public void SetModelColour(string team) {
		playerTeam = team;
		UpdateMaterial(playerTeam);
	}

	[ClientRpc]
	public void RpcSetModelColour(string team) {
		playerTeam = team;
		UpdateMaterial(playerTeam);
	}

	private void UpdateMaterial(string team) {
		currentMaterial = team == Settings.HeroTeam ? heroMat : villainMat;
		foreach(Transform child in rootModelObject) {
			if(child.GetComponent<Renderer>() != null) {
				child.GetComponent<Renderer>().material = currentMaterial; 
			}
		}
	}

	[Command]
	public void CmdTellOthersMaterial(string playerTeam) {
		RpcSetModelColour(playerTeam);
		gameObject.GetComponent<PlayerModelChanger>().UpdateMaterial(playerTeam);
	}

	public void EnableModel(bool enable) {
		rootModelObject.gameObject.SetActive(enable);
	}
}

