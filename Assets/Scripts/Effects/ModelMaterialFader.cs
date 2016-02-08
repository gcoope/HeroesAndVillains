using UnityEngine;
using System.Collections;
using DG.Tweening;

public class ModelMaterialFader : MonoBehaviour {

	private Renderer[] playerRenderers;
	private float fadeDuration = 1f;

	void Awake() {
		playerRenderers = GetComponentsInChildren<Renderer>();
	}

	public void PassMaterial(Material mat) {
		for(int i = 0; i < playerRenderers.Length; i++) {
			playerRenderers[i].material = mat;
		}
	}

	public void FadeIn() {
		for(int i = 0; i < playerRenderers.Length; i++) {
			playerRenderers[i].material.DOFade(1, fadeDuration);
		}
	}

	public void FadeOut() {
		for(int i = 0; i < playerRenderers.Length; i++) {
			playerRenderers[i].material.DOFade(0, fadeDuration);
		}
	}
		
	public void ShowModel() {
		for(int i = 0; i < playerRenderers.Length; i++) {
			playerRenderers[i].enabled = true;
		}
	}

	public void HideModel() {
		for(int i = 0; i < playerRenderers.Length; i++) {
			playerRenderers[i].enabled = false;
		}
	}

}
