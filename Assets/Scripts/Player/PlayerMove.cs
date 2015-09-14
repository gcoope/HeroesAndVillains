using UnityEngine;
using System.Collections;

public class PlayerMove : Photon.MonoBehaviour {

	public Camera playerCamera;
	private float moveSpeed = 15f;
	private float rotateSpeed = 20f;
	private Vector3 moveDir;
	private Rigidbody playerRigidbody;
	private Transform playerTransform;

	// Multiplayer syncing
	private PhotonView pv;
	private Vector3 trueLocation;

	void Awake () {
		playerRigidbody = GetComponent<Rigidbody>();
		playerTransform = GetComponent<Transform>();
		pv = GetComponent<PhotonView>();
	}
	
	void Update () {
		if(pv.isMine) RecieveInput();
//		else if(!pv.isMine) SyncWithNetwork();
	}

	private void SyncWithNetwork() {
		playerTransform.position = Vector3.Lerp(playerTransform.position, trueLocation, Time.deltaTime * 5f);
	}

	private void RecieveInput() {
		moveDir = new Vector3(0, 0, Input.GetAxisRaw("Vertical")).normalized;
		if(Input.GetAxisRaw("Horizontal") < 0) transform.Rotate(0,-5,0 * Time.deltaTime * rotateSpeed); // L
		if(Input.GetAxisRaw("Horizontal") > 0) transform.Rotate(0,5,0 * Time.deltaTime * rotateSpeed); // R
	}
	
	void FixedUpdate() {
		this.playerRigidbody.MovePosition(playerRigidbody.position + transform.TransformDirection(moveDir) * moveSpeed * Time.deltaTime);
	}
}
