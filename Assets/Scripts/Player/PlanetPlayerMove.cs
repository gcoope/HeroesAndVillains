using UnityEngine;
using System.Collections;
using smoothstudio.heroesandvillains.physics;
using UnityEngine.Networking;
using smoothstudio.heroesandvillains.player.events;

namespace smoothstudio.heroesandvillains.player
{
	[NetworkSettings(channel=2, sendInterval=0)]
	public class PlanetPlayerMove : NetworkBehaviour
    {
		[SerializeField] private PlayerAnimator playerAnimator;
		private bool cameraIsFPS = true;
		private BasePlayerInfo playerInfo;
		private PlayerGravityBody playerGravityBody;
    	private Rigidbody playerRigidbody;	
		private Transform playerCameraTransform;

		private Vector3 cameraFpsPosition;
		private Vector3 cameraThirdPersonPosition;

        private float moveSpeed;
        private const float rotateSpeed = 25f;
        private float jumpPower;
        private Vector3 moveDir;

		private bool isGrounded;
		private bool hasDoubleJumped;
		private bool doubleJumpEnabled;
		private const float rayLength = 1f;
//		private const float rayLength = 1.8f;

		private PlayerModelChanger playerModel; // So we can turn it on in 3rd person

		// Sent over network
		[SyncVar]
		private Vector3 syncPos;
		[SyncVar]
		private Quaternion syncRot;

		private Quaternion lastRotation;
		private Vector3 lastPosition;

		// FPS
		private Vector2 _mouseAbsolute;
		private Vector2 _smoothMouse;
		private Vector2 clampInDegrees = new Vector2(360, 180);
		private Vector2 sensitivity = new Vector2(0.75f, 0.75f);
		private Vector2 smoothing = new Vector2(3, 3);
		private Vector2 targetDirection;
		private Vector2 targetCharacterDirection;

		private bool useMouseSmoothing = true;

		// Pause menu control
		private bool allowAllControl = true;

		// Sticking to building fix
		private PhysicMaterial colliderMaterial;

		// Game flow
		private bool isGameOver = false;

		void OnDisable() {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			Screen.lockCursor = false;
		}

		void OnEnable() {
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			Screen.lockCursor = true;
		}

		void Awake() {
			gameObject.AddGlobalEventListener(GameplayEvent.GameOver, HandleGameOverEvent);
			gameObject.AddGlobalEventListener(GameplayEvent.ResetGame, HandleGameResetEvent);

			// Mouse smoothing option
			gameObject.AddGlobalEventListener(MenuEvent.EnableMouseSmoothing, (EventObject evt)=>{
				useMouseSmoothing = true;
			});
			gameObject.AddGlobalEventListener(MenuEvent.DisableMouseSmoothing, (EventObject evt)=>{
				useMouseSmoothing = false;
			});
	}

		void Start() {
			playerModel = GetComponent<PlayerModelChanger>();
			playerInfo = gameObject.GetComponent<BasePlayerInfo>();
			if(isLocalPlayer) {
				colliderMaterial = GetComponent<Collider>().material;
				playerRigidbody = GetComponent<Rigidbody>();
				playerGravityBody = GetComponent<PlayerGravityBody>();			
				playerCameraTransform = GetComponentInChildren<Camera>().transform;

				moveSpeed = playerInfo.speed;
				jumpPower = playerInfo.jumpHeight;

				doubleJumpEnabled = playerInfo.doubleJumpEnabled;

				cameraFpsPosition = new Vector3(0, 0.5f, 0f);
				cameraThirdPersonPosition = new Vector3(0, 2f, -4.5f);


				targetDirection = playerCameraTransform.localRotation.eulerAngles;	
				targetCharacterDirection = transform.localRotation.eulerAngles;
			} else {
				playerModel.EnableModel(true);
			}
		}

        void Update() {
			if(isLocalPlayer && !isGameOver) {

//				Cursor.lockState = !allowAllControl ? CursorLockMode.Locked : CursorLockMode.None; // findme Cursor control
				Cursor.visible = !allowAllControl;
				Screen.lockCursor = allowAllControl;

				RecieveInputFirstPerson();
				HandleJumping();
				playerAnimator.UpdateIsGrounded(isGrounded);

				RaycastHit sphereHit;
//				if(Physics.Raycast(transform.position, -transform.up, rayLength)) isGrounded = true; // Grounding check
				if(Physics.SphereCast(transform.position, 0.4f, -transform.up, out sphereHit, rayLength)) isGrounded = true;
				else isGrounded = false;

				if(Input.GetKeyDown(KeyCode.C)) ToggleCameraPosition();

				// Move to top of world for testing
				if(Input.GetKeyDown(KeyCode.F8)) {
					ResetPositionToSpawn ();
				}

//				 Sticking fix
				if (isGrounded) {
					colliderMaterial.dynamicFriction = 1;
					colliderMaterial.staticFriction = 1;
				} else {
					colliderMaterial.dynamicFriction = 0.2f;
					colliderMaterial.staticFriction = 0.2f;
				}
			} 
			else {
				UpdateOfflineTransform(); // For non-player-player movement
			}
        }
		
		void FixedUpdate() {
			if(isLocalPlayer) {
				if(playerRigidbody == null) playerRigidbody = GetComponent<Rigidbody>();


				if(moveDir == Vector3.zero && isGrounded) {
					playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x * 0.9f, playerRigidbody.velocity.y, playerRigidbody.velocity.z * 0.9f);
				}

				float xSpeed = Mathf.Abs(transform.InverseTransformDirection(playerRigidbody.velocity).x);
				float zSpeed = Mathf.Abs(transform.InverseTransformDirection(playerRigidbody.velocity).z);
				if(xSpeed < moveSpeed && zSpeed < moveSpeed) {
					playerRigidbody.velocity += transform.TransformDirection(moveDir);
				}

				// Sending velocity to animator
				playerAnimator.CmdUpdateVelocity(playerRigidbody.velocity);

				TransmitTransform(); // Keep other clients updated
			}			
		}

		private void ResetPositionToSpawn() {
			Transform topPos = SpawnManager.instance.GetFreeSpawn(playerInfo.playerTeam == Settings.HeroTeam);
			transform.position = topPos.position;
			transform.rotation = topPos.rotation;
		}

		private void HandleGameOverEvent(EventObject evt) {
			RpcSetGameOver(true);
		}

		[ClientRpc]
		private void RpcSetGameOver(bool gameOver) {
			moveDir = new Vector3(0,0,0);
			if(playerRigidbody != null) playerRigidbody.velocity = new Vector3(0,0,0);
			isGameOver = gameOver;
		}

		private void HandleGameResetEvent(EventObject evt) {
			ResetGame();
		}

		private void ResetGame() {
			moveDir = new Vector3(0,0,0);
			if(playerRigidbody != null) playerRigidbody.velocity = new Vector3(0,0,0);
			isGameOver = false;
			SetAllowControl (true);
			ResetPositionToSpawn ();
		}

		public void SetAllowControl(bool allow) {
			allowAllControl = allow;
		}

		public void SetSensitivity(float sens) {
			sensitivity = new Vector2(sens, sens);
		}

		float helpDownTimer = 0;
		float maxHoldtime = 1f;
		bool reduceGravity = false;

		private void HandleJumping() {
			if(!allowAllControl) return;
			if (Input.GetButtonDown("Jump")) {

				helpDownTimer = 0;

				if(isGrounded) {
					playerGravityBody.SetGravity(8f);
					playerGravityBody.Jump(7f);
					playerAnimator.CmdJump();
					reduceGravity = true;
				}

//				if(isGrounded) {
//					playerGravityBody.Jump(jumpPower);
//					hasDoubleJumped = false;
//				} else if(!isGrounded && !hasDoubleJumped && doubleJumpEnabled) {
//					hasDoubleJumped = true;
//					playerGravityBody.Jump(jumpPower * 0.5f);
//				}			
			}  

			if(Input.GetButton("Jump")) {
				if(helpDownTimer < maxHoldtime) {
					if(reduceGravity) {
						helpDownTimer += Time.deltaTime;
					}
				} else {
					reduceGravity = false;
					playerGravityBody.ResetGravity();
				}
			}

			if(Input.GetButtonUp("Jump")) {
				playerGravityBody.ResetGravity();
			}
		}

		private void RecieveInputFirstPerson() {

			if(!allowAllControl) { // When game paused
				moveDir = Vector3.zero;
				return;
			}

			moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized; // Forward/Back, strafe Left/Right

			// [NOT MINE] Smooth mouse look - http://forum.unity3d.com/threads/a-free-simple-smooth-mouselook.73117/ // TODO reference me

			// Get raw mouse input for a cleaner reading on more sensitive mice.
			Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

			// Allow the script to clamp based on a desired target value.
			Quaternion targetOrientation = Quaternion.Euler(targetDirection);
			Quaternion targetCharacterOrientation = Quaternion.Euler(targetCharacterDirection);

			// Scale input against the sensitivity setting and multiply that against the smoothing value.
			mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity.x * smoothing.x, sensitivity.y * smoothing.y));

			if(useMouseSmoothing) { // Quite intensive so have the ability to turn it off			
				_smoothMouse.x = Mathf.Lerp(_smoothMouse.x, mouseDelta.x, 1f / smoothing.x);
				_smoothMouse.y = Mathf.Lerp(_smoothMouse.y, mouseDelta.y, 1f / smoothing.y);
			} else {
				_smoothMouse.x = mouseDelta.x;
				_smoothMouse.y = mouseDelta.y;
			}
			
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
			if(syncRot == null || syncPos == null) return;
			transform.position = Vector3.Lerp(transform.position, syncPos, Time.deltaTime * 15f);
			transform.rotation = Quaternion.Lerp(transform.rotation, syncRot, Time.deltaTime * 15f);
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
			if(isLocalPlayer && NetworkManager.singleton.IsClientConnected()) { // TODO Add limiters here if lag/bandwidth issue
				Cmd_PassPosition(transform.position);
				if(transform.rotation != lastRotation) {
					Cmd_PassRotation(transform.rotation);
					lastRotation = transform.rotation;
				}
			}
		}

		private void ToggleCameraPosition() {
			cameraIsFPS = !cameraIsFPS;
			if(cameraIsFPS) {
				playerCameraTransform.localPosition = cameraFpsPosition;
			} else {
				playerCameraTransform.localPosition = cameraThirdPersonPosition;
			}

			playerModel.EnableModel(!cameraIsFPS);
		}

		public bool isCameraFPS() {
			return cameraIsFPS;
		}

		// Controller check
		private bool isControlerInput() {
			if(Input.GetKey(KeyCode.Joystick1Button0)  ||
				Input.GetKey(KeyCode.Joystick1Button1)  ||
				Input.GetKey(KeyCode.Joystick1Button2)  ||
				Input.GetKey(KeyCode.Joystick1Button3)  ||
				Input.GetKey(KeyCode.Joystick1Button4)  ||
				Input.GetKey(KeyCode.Joystick1Button5)  ||
				Input.GetKey(KeyCode.Joystick1Button6)  ||
				Input.GetKey(KeyCode.Joystick1Button7)  ||
				Input.GetKey(KeyCode.Joystick1Button8)  ||
				Input.GetKey(KeyCode.Joystick1Button9)  ||
				Input.GetKey(KeyCode.Joystick1Button10) ||
				Input.GetKey(KeyCode.Joystick1Button11) ||
				Input.GetKey(KeyCode.Joystick1Button12) ||
				Input.GetKey(KeyCode.Joystick1Button13) ||
				Input.GetKey(KeyCode.Joystick1Button14) ||
				Input.GetKey(KeyCode.Joystick1Button15) ||
				Input.GetKey(KeyCode.Joystick1Button16) ||
				Input.GetKey(KeyCode.Joystick1Button17) ||
				Input.GetKey(KeyCode.Joystick1Button18) ||
				Input.GetKey(KeyCode.Joystick1Button19) ) {
				return true;
			}
			return false;
		}
    }
}