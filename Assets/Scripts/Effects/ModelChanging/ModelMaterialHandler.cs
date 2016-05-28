using UnityEngine;
using System.Collections;
using DG.Tweening;

public class ModelMaterialHandler : MonoBehaviour {

	public Color[] heroColours;
	public Color[] villainColours;

	public Renderer[] headRenderers;
	private Renderer[] playerRenderers;
	private const float fadeDuration = 1f;
	private bool modelShouldShow = false;

	public Renderer suitRend;
	private Material suitMat;

	void Awake() {
		playerRenderers = GetComponentsInChildren<Renderer>();
	}

	void Start() {
		suitMat = suitRend.materials [0]; // TODO So hard coded, fix me
	}


	public void SetModelShowing(bool enable) {
		modelShouldShow = enable;
	}

	public void FadeIn(bool delay = false) {
		if(delay) {
			StartCoroutine("WaitToVisuallyActivate");
		} else {
			FadeInRenderers();
		}
	}

	IEnumerator WaitToVisuallyActivate() {
		yield return new WaitForSeconds(0.5f);
		FadeInRenderers();
	}

	private void FadeInRenderers() {
//		if(modelShouldShow) {
			for(int i = 0; i < playerRenderers.Length; i++) {
				playerRenderers[i].enabled = true;
			}
//		}
	}

	public void FadeOut() {
		for(int i = 0; i < playerRenderers.Length; i++) {
			playerRenderers[i].enabled = false;
		}
	}
		
	public void ShowHeadModel() {
		for(int i = 0; i < headRenderers.Length; i++) {
			headRenderers[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
		}
	}

	public void HideHeadModel() {
		for(int i = 0; i < headRenderers.Length; i++) {
			headRenderers[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
		}
	}

	// Colour changing based on team
	public void SetTeamColours(Color col) {
		if (suitMat == null) {
			suitMat = suitRend.materials [0]; // TODO So hard coded, fix me

		}
		suitMat.color = col;
	}

	// This takes the outfit index and team
	public void SetTeamColour(int outfitIndex, bool isHero) {
		Debug.Log("[ModelMaterialHandler] " + isHero);
		if (suitMat == null) {
			suitMat = suitRend.materials [0]; // TODO So hard coded, fix me
		}

		switch(outfitIndex){
		case 0:
		case 1:
			suitMat.color = isHero ? heroColours[0] : villainColours[0];
			break;
		case 2:
		case 3:
			suitMat.color = isHero ? heroColours[1] : villainColours[1];
			break;
		case 4:
		case 5:
			suitMat.color = isHero ? heroColours[2] : villainColours[2];
			break;
		case 6:
		case 7:
			suitMat.color = isHero ? heroColours[3] : villainColours[3];
			break;
		case 8:
			suitMat.color = isHero ? heroColours[4] : villainColours[4];
			break;
		}
	}

}
