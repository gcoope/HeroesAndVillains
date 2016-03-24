using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerAnimator : NetworkBehaviour {

	private NetworkAnimator anim;
	private Vector3 playerVel;

	float xSpeed;
	float ySpeed;
	float zSpeed;
	bool localIsGrounded;

	void Awake() {
		anim = gameObject.GetComponent<NetworkAnimator>();
	}

	[Command]
	public void CmdJump() {
		RpcJump();
	}

	[ClientRpc]
	private void RpcJump() {
		anim.animator.SetTrigger("Jump");
	}

	public void UpdateIsGrounded(bool isGrounded) {
		localIsGrounded = isGrounded;
		anim.animator.SetBool("OnGround", isGrounded);
	}

	[Command]
	public void CmdFire() {
		RpcFire();
	}

	[ClientRpc]
	private void RpcFire() {
		anim.animator.SetTrigger("Shoot");
	}

	[Command]
	public void CmdUpdateVelocity(Vector3 playerVelocity) {
		Vector3 localVelo = transform.InverseTransformDirection(playerVelocity);
		playerVel = localVelo;
		RpcUpdateVelocity(playerVelocity);
	}

	[ClientRpc]
	public void RpcUpdateVelocity(Vector3 playerVelocity) {
		Vector3 localVelo = transform.InverseTransformDirection(playerVelocity);
		playerVel = localVelo;
	}

	void Update() {
		// forwards/backwards
		float clampedZ = Mathf.Clamp(playerVel.z, -1, 1);
		zSpeed = Mathf.Lerp(zSpeed, clampedZ, Time.deltaTime * 10);
		anim.animator.SetFloat("ForwardBackward", zSpeed);

		// left right
		float clampedX =  Mathf.Clamp(playerVel.x, -1, 1);
		xSpeed = Mathf.Lerp(xSpeed, clampedX, Time.deltaTime * 10);
		anim.animator.SetFloat("LeftRight", xSpeed);

		// Air speed
//		float clampedY = Mathf.Clamp(playerVel.y, -1, 1);
//		ySpeed = clampedY;

//		anim.animator.SetFloat("AirSpeed", ySpeed);
	
	}
}
