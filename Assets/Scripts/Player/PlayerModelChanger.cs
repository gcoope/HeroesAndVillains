using System;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerModelChanger : NetworkBehaviour {
	
	public Color heroCol;
	public Color villainCol;

	public ModelHairHandler hairHandler;
	public ModelFaceHandler faceHandler;
	public ModelMaterialHandler materialHandler;

	public void SetupModel(string team) {
		Color teamColour = team == Settings.HeroTeam ? heroCol : villainCol;
		materialHandler.SetTeamColours (teamColour);
	}

	public void SetupOutfit(int outfitIndex) {
		hairHandler.SetHairIndex(outfitIndex);
		faceHandler.SetFaceIndex(outfitIndex);
	}

	public void EnableModel(bool enable) {
		materialHandler.SetModelShowing(enable);
		if(enable) materialHandler.ShowHeadModel();
		else materialHandler.HideHeadModel();
	}
}

