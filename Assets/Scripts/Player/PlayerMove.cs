using UnityEngine;
using System.Collections;
using smoothstudio.heroesandvillains.physics;
using UnityEngine.Networking;

namespace smoothstudio.heroesandvillains.player
{
	public class PlayerMove : NetworkBehaviour
	{
		[HideInInspector] public Camera playerCamera;
		private float moveSpeed = Settings.BaseMoveSpeed;
		private float jumpPower = Settings.BaseJumpHeight;
		private float rotateSpeed = 25f;
		private Vector3 moveDir;
		private Rigidbody playerRigidbody;
		private float gravity = Settings.Gravity;
		private float distToGround;
		
		// Sent over network
		[SyncVar]
		private Vector3 syncPos;
		
		[SyncVar]
		private Quaternion syncRot;

		private Quaternion lastRotation;
		private Vector3 lastPosition;
		private float sendThreshold = 0.1f;
		
		void Awake() { 
			playerRigidbody = GetComponent<Rigidbody>();
			if (playerRigidbody == null) playerRigidbody = gameObject.AddComponent<Rigidbody>();
			playerCamera = gameObject.GetComponentInChildren<Camera>();
		}

		void Start() {
			distToGround = GetComponent<Collider>().bounds.extents.y;
		}
		
		public override void OnStartLocalPlayer() {
			playerCamera.enabled = true;
		}
		
		void Update() {
			if(isLocalPlayer) RecieveInput();
			if(!isLocalPlayer) UpdateTransform();
		}
		
		private void RecieveInput() {
			moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized; // F/B

			if (Input.GetAxisRaw("Horizontal") < 0) { // L
				playerRigidbody.MoveRotation(transform.rotation * Quaternion.Euler(new Vector3(0, -5f * Time.deltaTime * rotateSpeed, 0)));
			} 
			if (Input.GetAxisRaw("Horizontal") > 0) { // R
				playerRigidbody.MoveRotation(transform.rotation * Quaternion.Euler(new Vector3(0, 5f * Time.deltaTime * rotateSpeed, 0)));
			}
			
			if (Input.GetButtonDown("Jump")) {
				if(Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f)) {
					playerRigidbody.AddForce(transform.up * jumpPower, ForceMode.Impulse);
				}
			}
			
		}
		
		void FixedUpdate() {
			TransmitTransform();

			// Artificial gravity
//			playerRigidbody.AddForce(transform.up * gravity);

			if(isLocalPlayer) {
//				this.playerRigidbody.MovePosition(playerRigidbody.position + transform.TransformDirection(moveDir)	 * moveSpeed * Time.deltaTime);
//				this.playerRigidbody.AddForce(transform.TransformDirection(moveDir) * moveSpeed * Time.deltaTime);
//				this.playerRigidbody.velocity += (transform.TransformDirection(moveDir) * moveSpeed);

				if(playerRigidbody.velocity.magnitude > 2f) {
					playerRigidbody.velocity = playerRigidbody.velocity.normalized * 2f;
				} else {
					playerRigidbody.velocity = moveDir;
				}
			}
		}
		
		void UpdateTransform() {
			transform.position = Vector3.Lerp(transform.position, syncPos, Time.deltaTime * 15f);
			transform.rotation = Quaternion.Slerp(transform.rotation, syncRot, Time.deltaTime * 15f);
		}

		[Command]
		private void Cmd_PassPosition(Vector3 pos) {
			this.syncPos = pos;
		}
		
		[Command]
		private void Cmd_PassRotation(Quaternion rotation) {
			this.syncRot = rotation;
		}
		
		[ClientCallback]
		private void TransmitTransform() {
			if(isLocalPlayer) {
				if(Vector3.Distance(transform.position, lastPosition) > sendThreshold) {
					Cmd_PassPosition(transform.position);
					lastPosition = transform.position;
				}

				if(transform.rotation != lastRotation) {
					Cmd_PassRotation(transform.rotation);
					lastRotation = transform.rotation;
				}
			}
		}
	}
}