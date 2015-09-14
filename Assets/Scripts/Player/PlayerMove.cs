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
		private float rotateSpeed = 20f;
		private float jumpPower = Settings.BaseJumpHeight;
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
			UpdateTransform();
		}
		
		private void RecieveInput() {
			moveDir = new Vector3(0, 0, Input.GetAxisRaw("Vertical")).normalized;
			if (Input.GetAxisRaw("Horizontal") < 0) transform.Rotate(0, -5, 0 * Time.deltaTime * rotateSpeed); // L
			if (Input.GetAxisRaw("Horizontal") > 0) transform.Rotate(0, 5, 0 * Time.deltaTime * rotateSpeed); // R
			
			if (Input.GetKeyDown(KeyCode.Space)) {

				if(Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f)) {
					playerRigidbody.AddForce(transform.up * jumpPower, ForceMode.Impulse);
				}

			}
			
		}
		
		void FixedUpdate() {
			TransmitTransform();

			// Artificial gravity
			playerRigidbody.AddForce(transform.up * gravity);

			if(isLocalPlayer) {
				this.playerRigidbody.MovePosition(playerRigidbody.position + transform.TransformDirection(moveDir) * moveSpeed * Time.deltaTime);
			}
		}
		
		void UpdateTransform() {
			if(!isLocalPlayer) {
				transform.position = Vector3.Lerp(transform.position, syncPos, Time.deltaTime * 15f);
				transform.rotation = Quaternion.Slerp(transform.rotation, syncRot, Time.deltaTime * 15f);
			}
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

				if(transform.rotation != lastPosition) {
					Cmd_PassRotation(transform.rotation);
					lastRotation = transform.rotation;
				}
			}
		}
	}
}