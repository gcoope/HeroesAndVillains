using UnityEngine;
using System.Collections;
using smoothstudio.heroesandvillains.physics;
using UnityEngine.Networking;
using smoothstudio.heroesandvillains.player.events;

namespace smoothstudio.heroesandvillains.player
{
	public class PlanetPlayerMove : NetworkBehaviour
    {
		private bool FPSMode;
		private bool cameraIsFPS;

		private BasePlayerInfo playerInfo;
		private PlayerGravityBody playerGravityBody;
    	private Rigidbody playerRigidbody;	
		private Transform playerCameraTransform;

		private Vector3 cameraFpsPosition;
		private Vector3 cameraThirdPersonPosition;

        private float moveSpeed;
        private float rotateSpeed = 25f;
        private float jumpPower;
        private Vector3 moveDir;

		private bool isGrounded;
		private bool hasDoubleJumped;
		private bool doubleJumpEnabled;
		private float rayLength = 1.8f;

		// Sent over network
		[SyncVar]
		private Vector3 syncPos;
		[SyncVar]
		private Quaternion syncRot;

		private Quaternion lastRotation;
		private Vector3 lastPosition;
		private float sendThreshold = 0.5f;

		// FPS
		private Vector2 _mouseAbsolute;
		private Vector2 _smoothMouse;
		private Vector2 clampInDegrees = new Vector2(360, 180);
		private bool lockCursor = true;
		private Vector2 sensitivity = new Vector2(2, 2);
		private Vector2 smoothing = new Vector2(3, 3);
		private Vector2 targetDirection;
		private Vector2 targetCharacterDirection;

		void Awake() {
			playerRigidbody = GetComponent<Rigidbody>();
			if (playerRigidbody == null) playerRigidbody = gameObject.AddComponent<Rigidbody>();
			playerGravityBody = GetComponent<PlayerGravityBody>();
			playerInfo = gameObject.GetComponent<BasePlayerInfo>();
			playerCameraTransform = GetComponentInChildren<Camera>().transform;
		}

		void OnDisable() {
			Screen.lockCursor = lockCursor = false;
		}

		void Start() {
			FPSMode = Settings.FirstPersonMode;
			cameraIsFPS = FPSMode;
			moveSpeed = playerInfo.speed;
			jumpPower = playerInfo.jumpHeight;

			doubleJumpEnabled = playerInfo.doubleJumpEnabled;

			cameraFpsPosition = new Vector3(0, 0.3f, 0.85f);
			cameraThirdPersonPosition = new Vector3(0, 2f, -4.5f);

			// FPS setup
			if(FPSMode) {
				targetDirection = playerCameraTransform.localRotation.eulerAngles;	
				targetCharacterDirection = transform.localRotation.eulerAngles;
			}
		}

        void Update() {
			if(isLocalPlayer) {
				if(FPSMode)	RecieveInputFirstPerson();
				else RecieveInputThirdPerson();

				HandleJumping();

				if(Physics.Raycast(transform.position, -transform.up, rayLength)) isGrounded = true; // Grounding check
				else isGrounded = false;

				if(Input.GetKeyDown(KeyCode.C)) ToggleCameraPosition();
			} else {
				UpdateOfflineTransform();
			}
        }
		
		void FixedUpdate() {
			if(isLocalPlayer) {
				TransmitTransform(); // Kepp other clients updated
				playerRigidbody.MovePosition(playerRigidbody.position + transform.TransformDirection(moveDir) * moveSpeed * Time.deltaTime);
			}
		}

		private void HandleJumping() {
			if (Input.GetButtonDown("Jump")) {
				if(isGrounded) {
					hasDoubleJumped = false;
					playerGravityBody.Jump(jumpPower);
				} else if(!isGrounded && !hasDoubleJumped && doubleJumpEnabled) {
					hasDoubleJumped = true;
					playerGravityBody.Jump(jumpPower * 0.5f);
				}
			}      
		}

        private void RecieveInputThirdPerson() {
			moveDir = new Vector3(0, 0, Input.GetAxisRaw("Vertical")).normalized; // Forward/Back

            if (Input.GetAxisRaw("Horizontal") < 0) transform.Rotate(0, -5, 0 * Time.deltaTime * rotateSpeed); // Left
            if (Input.GetAxisRaw("Horizontal") > 0) transform.Rotate(0, 5, 0 * Time.deltaTime * rotateSpeed); // Right
        }

		private void RecieveInputFirstPerson() {

			if(Input.GetKeyDown(KeyCode.L)) lockCursor = !lockCursor;

			moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized; // Forward/Back, strafe Left/Right

			// [NOT MINE] Smooth mouse look - http://forum.unity3d.com/threads/a-free-simple-smooth-mouselook.73117/
			// TODO Reference this

			Screen.lockCursor = lockCursor;

			// Allow the script to clamp based on a desired target value.
			Quaternion targetOrientation = Quaternion.Euler(targetDirection);
			Quaternion targetCharacterOrientation = Quaternion.Euler(targetCharacterDirection);

			// Get raw mouse input for a cleaner reading on more sensitive mice.
			Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
				
			// Scale input against the sensitivity setting and multiply that against the smoothing value.
			mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity.x * smoothing.x, sensitivity.y * smoothing.y));
			
			// Interpolate mouse movement over time to apply smoothing delta.
			_smoothMouse.x = Mathf.Lerp(_smoothMouse.x, mouseDelta.x, 1f / smoothing.x);
			_smoothMouse.y = Mathf.Lerp(_smoothMouse.y, mouseDelta.y, 1f / smoothing.y);
			
			// Find the absolute mouse movement value from point zero.
			_mouseAbsolute += _smoothMouse;
			
			// Clamp and apply the local x value first, so as not to be affected by world transforms.
			if (clampInDegrees.x < 360)
				_mouseAbsolute.x = Mathf.Clamp(_mouseAbsolute.x, -clampInDegrees.x * 0.5f, clampInDegrees.x * 0.5f);
			
			Quaternion xRotation = Quaternion.AngleAxis(-_mouseAbsolute.y, targetOrientation * Vector3.right);
			playerCameraTransform.localRotation = xRotation;
			
			// Then clamp and apply the global y value.
			if (clampInDegrees.y < 360)
				_mouseAbsolute.y = Mathf.Clamp(_mouseAbsolute.y, -clampInDegrees.y * 0.5f, clampInDegrees.y * 0.5f);
			
			playerCameraTransform.localRotation *= targetOrientation;

			transform.Rotate(new Vector3(0,_smoothMouse.x,0));

			// -----------------------------------------
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
			if(isLocalPlayer) { // TODO Add limiters here if lag/bandwidth issue
					Cmd_PassPosition(transform.position);
//				if(transform.rotation != lastRotation) {
					Cmd_PassRotation(transform.rotation);
//					lastRotation = transform.rotation;
//				}
			}
		}

		private void ToggleCameraPosition() {
			cameraIsFPS = !cameraIsFPS;
			if(cameraIsFPS) {
				playerCameraTransform.localPosition = cameraFpsPosition;
			} else {
				playerCameraTransform.localPosition = cameraThirdPersonPosition;
			}
		}
    }
}