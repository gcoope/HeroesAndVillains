using UnityEngine;
using System.Collections;

public class ModelFaceHandler : MonoBehaviour {

	public Color[] skinTones;

	public Texture[] heroFaceTextures;
	public Texture[] villainFaceTextures;
	private int currentIndex = 0;

	public Renderer faceRenderer;
	private Material headSkinMat;
	private Material faceMat;

	public Renderer bodyRenderer;
	private Material bodySkinMat;

	void Awake() {
		if(faceRenderer != null) {
			headSkinMat = faceRenderer.materials[0];
			faceMat = faceRenderer.materials[1];
		}

		if(bodyRenderer != null) {
			bodySkinMat = bodyRenderer.materials[2];
		}
	}

	public void SetFaceIndex(int index, bool isHero) {
		currentIndex = index;
		faceMat.mainTexture = isHero ? heroFaceTextures[currentIndex] : villainFaceTextures[currentIndex];
		switch(index) { // We have to apply a slight colour change to the diffuse as the skin colour and face textures aren't the same 
		case 0:
		case 3:
		case 7:
			headSkinMat.color = skinTones[0];
			bodySkinMat.color = skinTones[0];
			break;
		case 1:
		case 4:
		case 6:
			headSkinMat.color = skinTones[1];
			bodySkinMat.color = skinTones[1];
			break;
		case 2:
		case 5:
		case 8:
			headSkinMat.color = skinTones[2];
			bodySkinMat.color = skinTones[2];
			break;
		}
	}
}