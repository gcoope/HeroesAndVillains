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
		private BasePlayerInfo playerInfo;
		private PlayerHealth playerHealth;
		private const float attackCooldown = 0.4f;
		private bool useFireCooldown = true;
		private bool canNormalFire = true;

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

		void Awake() {
			gameObject.AddGlobalEventListener(ProjectileEvent.MeleeHitPlayer, MeleeHitSomePlayer);
			gameObject.AddGlobalEventListener(GameplayEvent.GameOver, HandleGameOverEvent);
		}

		void Start() {
			playerInfo = gameObject.GetComponent<BasePlayerInfo>();
			playerHealth = gameObject.GetComponent<PlayerHealth>();

			splashCollider = Resources.Load<GameObject>("Prefabs/Physics/SplashDamageCollider");

			playerCameraTransform = gameObject.GetComponentInChildren<Camera>().transform;
			if(isLocalPlayer) {
				playerGravityBody = GetComponent<PlayerGravityBody>();
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

		public void SetAllowAnyInput(bool allow) {
			allowAnyInput = allow;
		}

		private void RecieveInput() {
			if (Input.GetMouseButtonDown(0)) {
				if(!canNormalFire && useFireCooldown) return;
				StartCoroutine("NormalFireCooldown");
				RaycastFire();
				playerCameraTransform.DOShakePosition(0.2f, new Vector3(0.2f, 0.2f, 0), 1); // TODO rethink camera shake implementation, cause bugs
			}       

			// Controller needs a bit more help
			if(Input.GetAxis("ControllerFire") < 0 && !controllerHasFired && controllerCanFire) {
				controllerHasFired = true;
				StartCoroutine("ControllerFireCooldown");
				RaycastFire();
				playerCameraTransform.DOShakePosition(0.2f, 0.2f, 1);	
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

		private void RaycastFire() { // Primary attack (for now)
			RaycastHit hit;
			if(Physics.Raycast(projectileLauncher.position, projectileLauncher.forward, out hit)) {
				CmdSpawnLine(transform.position, hit.point, playerInfo.playerTeam);
				CmdSpawnExplosion(hit.point, playerInfo.playerTeam);
//				if(hit.collider.CompareTag(ObjectTagKeys.Player)) {
//					CmdRaycastHit(hit.collider.gameObject);
//				} else {
					CmdSpawnExplosioncollider(hit.point, localPlayerInfoPacket);
//				}
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

		[Command] // TODO Make this server only?
		private void CmdSpawnExplosioncollider(Vector3 pos, PlayerInfoPacket playerInfoPacket) {
			GameObject col = Instantiate(splashCollider);
			col.transform.position = pos;
			col.GetComponent<SplashDamagerCollider>().SetOwner(playerInfoPacket);
			NetworkServer.Spawn(col); // TODO We might be able to do all collisions on the server someday..		
		}

		[Command]
		private void CmdRaycastHit(GameObject player) {
			player.GetComponent<PlayerHealth>().ServerTakeDamage(20, localPlayerInfoPacket);
		}

		// TODO Falcon kick
		private void MeleeHitSomePlayer(EventObject evt) {
			if (evt.Params != null) {
				if((BasePlayerInfo)evt.Params[0] == playerInfo) { // If the person hit was us
					playerHealth.TakeDamage((int)evt.Params[1], localPlayerInfoPacket);
				}
			}
		}


	}
}