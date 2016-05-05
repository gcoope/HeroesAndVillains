using System;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerModelChanger : NetworkBehaviour {

	public Material heroMat;
	public Material villainMat;
	public Color heroCol;
	public Color villainCol;

	private Material currentMaterial;
	[SyncVar(hook = "OnChangeTeam")]
	private string playerTeam;
	[SerializeField] private ModelMaterialHandler materialHandler;

	private void OnChangeTeam(string team) {
		if(team != playerTeam) CmdTellOthersMaterial(team);
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
//		currentMaterial = team == Settings.HeroTeam ? heroMat : villainMat;
//		materialHandler.PassMaterial(currentMaterial);
		Color teamColour = team == Settings.HeroTeam ? heroCol : villainCol;
		materialHandler.SetTeamColours (teamColour);
		GetComponent<PlayerRagdoll>().SetupRigidbody(teamColour); // TODO Maybe somewhere better - sets suit color for rigidbody. We need to also do hair style and clothes settings

	}

	[Command]
	public void CmdTellOthersMaterial(string playerTeam) {
		RpcSetModelColour(playerTeam);
//		gameObject.GetComponent<PlayerModelChanger>().UpdateMaterial(playerTeam);
	}

	public void EnableModel(bool enable) {
		materialHandler.SetModelShowing(enable);
		if(enable) materialHandler.ShowHeadModel();
		else materialHandler.HideHeadModel();
	}
}

