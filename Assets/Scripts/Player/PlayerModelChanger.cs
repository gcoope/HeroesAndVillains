using System;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerModelChanger : NetworkBehaviour {
	
//	public Color heroCol;
//	public Color villainCol;

	public ModelHairHandler hairHandler;
	public ModelFaceHandler faceHandler;
	public ModelMaterialHandler materialHandler;

	private string pTeam;

	public void SetupModel(string team) {
		pTeam = team;
//		Color teamColour = team == Settings.HeroTeam ? heroCol : villainCol;
//		materialHandler.SetTeamColours (teamColour);
		// TODO base colour of outfit index
	}

	public void SetupOutfit(int outfitIndex) {
		hairHandler.SetHairIndex(outfitIndex);
		faceHandler.SetFaceIndex(outfitIndex, pTeam == Settings.HeroTeam);
		materialHandler.SetTeamColour(outfitIndex, pTeam == Settings.HeroTeam);
	}

	public void EnableModel(bool enable) {
		materialHandler.SetModelShowing(enable);
		if(enable) materialHandler.ShowHeadModel();
		else materialHandler.HideHeadModel();
	}
}

