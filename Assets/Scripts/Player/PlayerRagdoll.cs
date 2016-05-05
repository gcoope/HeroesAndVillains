using UnityEngine;
using System.Collections;

public class PlayerRagdoll : MonoBehaviour {

	public GameObject ragdoll;
	public Rigidbody ragdollRbody;
	public Transform ragdollTransform;
	public Renderer model;
	private Material suitMat;
	private Vector3 startPos;
	private Vector3 startRot;

	void Awake() {
		startPos = ragdollTransform.localPosition;
		startRot = ragdollTransform.localEulerAngles;
		ragdoll.SetActive(false);
		suitMat = model.materials[0]; // TODO un-hardcode me
	}

	public void SetupRigidbody(Color suitCol) {
		suitMat.color = suitCol;
	}

	public void EnableRagdoll(Vector3 velo) {
		ragdoll.GetComponent<Animator>().enabled = false;
		ragdoll.SetActive(true);
		ragdollRbody.velocity = velo;
	}

	public void HideRagdoll() {
		ragdoll.SetActive(false);
		ragdoll.GetComponent<Animator>().enabled = true;
		ragdollRbody.velocity = Vector3.zero;
		ragdollTransform.localPosition = startPos;
		ragdollTransform.localEulerAngles = startRot;
	}

}
