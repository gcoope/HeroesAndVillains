using UnityEngine;
using System.Collections;
using DG.Tweening;

public class ModelMaterialFader : MonoBehaviour {

	public Renderer[] headRenderers;
	private Renderer[] playerRenderers;
	private const float fadeDuration = 1f;
	private bool modelShouldShow = false;

	void Awake() {
		playerRenderers = GetComponentsInChildren<Renderer>();
	}

	public void PassMaterial(Material mat) {
		for(int i = 0; i < playerRenderers.Length; i++) {
			playerRenderers[i].material = mat;
		}
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
		if(modelShouldShow) {
			for(int i = 0; i < playerRenderers.Length; i++) {
				playerRenderers[i].enabled = true;
			}
		}
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

}
