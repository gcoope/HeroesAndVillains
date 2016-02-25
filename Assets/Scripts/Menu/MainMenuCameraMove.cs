using UnityEngine;
using System.Collections;
using DG.Tweening;

public class MainMenuCameraMove : MonoBehaviour {

	[SerializeField] private Transform cameraPivot;

	public Vector3 startRotation;
	public Vector3 multiplayerRotation;
	public Vector3 optionsRotation;

	void Start () {
		cameraPivot.eulerAngles = startRotation;
	}	

	public void MoveToStartPosition() {
		DOTween.Kill(cameraPivot.eulerAngles);
		cameraPivot.DORotate(startRotation, 0.5f);
	}

	public void MoveToMultiplayerPosition() {
		DOTween.Kill(cameraPivot.eulerAngles);
		cameraPivot.DORotate(multiplayerRotation, 0.5f);
	}

	public void MoveToOptionsPosition() {
		DOTween.Kill(cameraPivot.eulerAngles);
		cameraPivot.DORotate(optionsRotation, 0.5f);
	}
}
