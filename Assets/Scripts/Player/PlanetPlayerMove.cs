using UnityEngine;
using System.Collections;
using smoothstudio.heroesandvillains.physics;
using UnityEngine.Networking;
using smoothstudio.heroesandvillains.player.events;

namespace smoothstudio.heroesandvillains.player
{
	public class PlanetPlayerMove : NetworkBehaviour
    {
        [HideInInspector] public Camera playerCamera;
        private float moveSpeed = Settings.BaseMoveSpeed;
        private float rotateSpeed = 15f;
        private float jumpPower = Settings.BaseJumpHeight;
        private Vector3 moveDir;
        private Rigidbody playerRigidbody;

		private bool isGrounded = false;
		private float rayLength = 2.5f;

		// Sent over network
		[SyncVar]
		private Vector3 syncPos;
		[SyncVar]
		private Quaternion syncRot;

		private Quaternion lastRotation;
		private Vector3 lastPosition;
		private float sendThreshold = 0.5f;

        void Awake() { 
            playerRigidbody = GetComponent<Rigidbody>();
            if (playerRigidbody == null) playerRigidbody = gameObject.AddComponent<Rigidbody>();
			playerCamera = gameObject.GetComponentInChildren<Camera>();
        }

		public override void OnStartLocalPlayer() {
			playerCamera.enabled = true;
			playerRigidbody.isKinematic = false;
			gameObject.GetComponent<PlayerGravityBody>().localPlayer = true;
		}

        void Update() {
			if(isLocalPlayer) {
				RecieveInput();
				if(Physics.Raycast(transform.position, -transform.up, rayLength)) {
					isGrounded = true;
				} else {
					isGrounded = false;
				}
			} else {
				UpdateOfflineTransform();
			}
        }
		
		void FixedUpdate() {
			if(isLocalPlayer) {
				TransmitTransform();
				this.playerRigidbody.MovePosition(playerRigidbody.position + transform.TransformDirection(moveDir) * moveSpeed * Time.deltaTime);
			}
		}

        private void RecieveInput() {
            moveDir = new Vector3(0, 0, Input.GetAxisRaw("Vertical")).normalized;
            if (Input.GetAxisRaw("Horizontal") < 0) transform.Rotate(0, -5, 0 * Time.deltaTime * rotateSpeed); // L
            if (Input.GetAxisRaw("Horizontal") > 0) transform.Rotate(0, 5, 0 * Time.deltaTime * rotateSpeed); // R
			if (Input.GetButtonDown("Jump") && isGrounded) {
				gameObject.DispatchGlobalEvent(PlayerEvent.PlayerJump, new object[] {jumpPower});
            }            
        }

		private void UpdateOfflineTransform() {
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
					Cmd_PassRotation(transform.rotation);
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