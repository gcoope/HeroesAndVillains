using System;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerModelChanger : NetworkBehaviour {

	public Material heroMat;
	public Material villainMat;
	private Material currentMaterial;
	[SyncVar(hook = "OnChangeTeam")] private string playerTeam;
	[SerializeField] private ModelMaterialFader materialFader;

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
		materialFader.PassMaterial(currentMaterial);
	}

	[Command]
	public void CmdTellOthersMaterial(string playerTeam) {
		RpcSetModelColour(playerTeam);
		gameObject.GetComponent<PlayerModelChanger>().UpdateMaterial(playerTeam);
	}

	public void EnableModel(bool enable) {
		if(enable) materialFader.ShowModel();
		else materialFader.HideModel();
	}
}

