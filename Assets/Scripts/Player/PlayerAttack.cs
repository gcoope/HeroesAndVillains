using UnityEngine;
using System.Collections;
using smoothstudio.heroesandvillains.physics;
using UnityEngine.Networking;
using smoothstudio.heroesandvillains.player.events;
using System.Collections.Generic;
using DG.Tweening;

namespace smoothstudio.heroesandvillains.player
{
	[RequireComponent(typeof(BasePlayerInfo))]
	public class PlayerAttack : NetworkBehaviour
	{	
		[SerializeField] private PlayerAnimator playerAnimator;
		private BasePlayerInfo playerInfo;
		private PlayerHealth playerHealth;
		private float attackCooldown = 0.4f;
		private float rapidFireCooldown = 0.2f;
		private bool useFireCooldown = true;
		private bool canNormalFire = true;
		private bool canRapidFire = true;
		private bool cameraShaking = false;

		private Transform playerCameraTransform;
		PlayerGravityBody playerGravityBody;

		// Projectile firing
		public Transform projectileLauncher;
		private GameObject splashCollider;

		private bool controllerHasFired = false;
		private bool controllerCanFire = true;

		private Color fireLineColor;

		private bool allowAnyInput = true;
		private bool isGameOver = false;

		private PlayerInfoPacket localPlayerInfoPacket;

		// Rapid fire
		private bool rapidFireEnabled = false;

		// Hitmarker handling
		private PlayerHUD playerHUD;

		void Awake() {
			gameObject.AddGlobalEventListener(ProjectileEvent.MeleeHitPlayer, MeleeHitSomePlayer);
			gameObject.AddGlobalEventListener(GameplayEvent.GameOver, HandleGameOverEvent);
			gameObject.AddGlobalEventListener(GameplayEvent.ResetGame, HandleResetGameEvent);
		}

		void Start() {
			playerInfo = gameObject.GetComponent<BasePlayerInfo>();
			playerHealth = gameObject.GetComponent<PlayerHealth>();

			splashCollider = Resources.Load<GameObject>("Prefabs/Physics/SplashDamageCollider");

			rapidFireCooldown = Settings.RapidFireCooldownSpeed;

			playerCameraTransform = gameObject.GetComponentInChildren<Camera>().transform;
			if(isLocalPlayer) {
				playerGravityBody = gameObject.GetComponent<PlayerGravityBody>();
				playerHUD = gameObject.GetComponent<PlayerHUD> ();
			}

			localPlayerInfoPacket = new PlayerInfoPacket(playerInfo.playerName, playerInfo.playerTeam, netId);
		}

		void Update() {
			if(isLocalPlayer && allowAnyInput && !isGameOver) {
				RecieveInput();
			}
		}

		private void HandleGameOverEvent(EventObject evt) {
			RpcSetGameOver(true);
		}
		[ClientRpc]
		private void RpcSetGameOver(bool gameOver) {
			isGameOver = gameOver;
		}

		private void HandleResetGameEvent(EventObject evt) {
			ResetGame();
		}

		private void ResetGame() {
			isGameOver = false;
		}

		public void SetAllowAnyInput(bool allow) {
			allowAnyInput = allow;
		}

		private void RecieveInput() {

			if (rapidFireEnabled) {
				if (Input.GetMouseButton (0)) {
					if (!canRapidFire)
						return;
					StartCoroutine ("RapidFireCooldown");
					RaycastFire ();
					ShakeCamera();
				}
			} else {
				if (Input.GetMouseButtonDown (0)) {
					if (!canNormalFire && useFireCooldown)
						return;
					StartCoroutine ("NormalFireCooldown");
					RaycastFire ();
					ShakeCamera();
				} 

			}

			// Controller needs a bit more help
			if(Input.GetAxis("ControllerFire") < 0 && !controllerHasFired && controllerCanFire) {
				controllerHasFired = true;
				StartCoroutine("ControllerFireCooldown");
				RaycastFire();
				ShakeCamera ();
			}
			if(Input.GetAxis("ControllerFire") > 0 && controllerHasFired) {
				controllerHasFired = false;
			}
		}

		IEnumerator NormalFireCooldown() { // Adds cooldown so you can't spam fire
			canNormalFire = false;
			yield return new WaitForSeconds(attackCooldown);
			canNormalFire = true;
		}
		IEnumerator ControllerFireCooldown() { // Adds cooldown for controller attacks so you can't spam fire
			controllerCanFire = false;
			yield return new WaitForSeconds(attackCooldown);
			controllerCanFire = true;
		}
		IEnumerator RapidFireCooldown() { // Adds cooldown so you can't spam fire
			canRapidFire = false;
			yield return new WaitForSeconds(rapidFireCooldown);
			canRapidFire = true;
		}

		private void ShakeCamera() {
			if(!cameraShaking) {
				cameraShaking = true;
				playerCameraTransform.DOShakePosition (0.2f, new Vector3 (0.4f, 0.4f, 0), 4).OnComplete (()=>{
					cameraShaking = false;
				});
			}

		}

		private void RaycastFire() { // Primary attack (for now)
			playerAnimator.CmdFire();
			RaycastHit hit;
			if(Physics.Raycast(projectileLauncher.position, projectileLauncher.forward, out hit)) {
				CmdSpawnLine(transform.position, hit.point, playerInfo.playerTeam);
				if(hit.collider.CompareTag(ObjectTagKeys.Player)) {
					CmdSpawnExplosion(hit.point, playerInfo.playerTeam);
					CmdRaycastHit(hit.collider.gameObject, localPlayerInfoPacket);
					if(hit.collider.GetComponent<BasePlayerInfo>().playerTeam != playerInfo.playerTeam) playerHUD.ShowHitmarker();
				} else {
					CmdSpawnExplosionWithNormal(hit.point, hit.normal, playerInfo.playerTeam);
					CmdSpawnExplosioncollider(hit.point, localPlayerInfoPacket);
				}
			} else { // Missed everything so we draw a line 100 units and spawn an explosion (with no collider)
				CmdSpawnLine(transform.position, projectileLauncher.position + (projectileLauncher.forward * 100f), playerInfo.playerTeam);
				CmdSpawnExplosion(projectileLauncher.position + (projectileLauncher.forward * 100f), playerInfo.playerTeam);
			}

			// Rocket jumping 
			if(Vector3.Distance(transform.position, hit.point) < 3f) {
				playerGravityBody.AddExplosionForce(hit.point, playerInfo.rocketJumpPower);
			}
		}

		[Command]
		private void CmdSpawnLine(Vector3 start, Vector3 end, string team)	{
			LocalPrefabSpawner.instance.ServerSpawnLine(start, end, team);	
		}

		[Command]
		private void CmdSpawnExplosion(Vector3 pos, string team)	{
			LocalPrefabSpawner.instance.ServerSpawnExplosion(pos, team);
		}

		[Command]
		private void CmdSpawnExplosionWithNormal(Vector3 pos, Vector3 hitNormal, string team)	{
			LocalPrefabSpawner.instance.ServerSpawnExplosionWithNormal(pos, hitNormal, team);
		}

		[Command] // TODO Make this server only?
		private void CmdSpawnExplosioncollider(Vector3 pos, PlayerInfoPacket playerInfoPacket) {
			GameObject col = Instantiate(splashCollider);
			col.transform.position = pos;
			col.GetComponent<SplashDamagerCollider>().SetOwner(playerInfoPacket);
			NetworkServer.Spawn(col); // TODO We might be able to do all collisions on the server someday..		
		}

		[Command]
		private void CmdRaycastHit(GameObject player, PlayerInfoPacket playerInfoPacket) {
			player.GetComponent<PlayerHealth>().ServerTakeDamage(20, playerInfoPacket);
		}

		// TODO Falcon kick
		private void MeleeHitSomePlayer(EventObject evt) {
			if (evt.Params != null) {
				if((BasePlayerInfo)evt.Params[0] == playerInfo) { // If the person hit was us
					playerHealth.TakeDamage((int)evt.Params[1], localPlayerInfoPacket);
				}
			}
		}


		// Powerups
		public void EnableRapidFire() {
			if (!rapidFireEnabled) {
				StartCoroutine ("RapidFireCooldownTimer");
			}
		}

		IEnumerator RapidFireCooldownTimer() {
			rapidFireEnabled = true;
			yield return new WaitForSeconds (Settings.RapidFirePowerupDuration);
			rapidFireEnabled = false;
		}


	}
}