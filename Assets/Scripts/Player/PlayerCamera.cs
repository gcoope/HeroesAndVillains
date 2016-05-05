using UnityEngine;
using DG.Tweening;

public class PlayerCamera : MonoBehaviour {

	public Transform cameraTransform;
	bool cameraShaking;

	public void ShakeCamera() {
		if(!cameraShaking) {
			cameraShaking = true;
			float x = Random.Range(-0.5f, 0.5f);
			float y= Random.Range(-0.5f, 0.5f);
			cameraTransform.DOShakePosition (0.2f, new Vector3 (x, y, 0), 4).OnComplete (()=>{
				cameraShaking = false;
			});
		}
	}
}
