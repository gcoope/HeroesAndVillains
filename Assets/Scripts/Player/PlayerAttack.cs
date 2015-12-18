using UnityEngine;
using System.Collections;
using smoothstudio.heroesandvillains.physics;
using UnityEngine.Networking;
using smoothstudio.heroesandvillains.player.events;
using System.Collections.Generic;
using smoothstudio.heroesandvillains.player.projectiles;
using DG.Tweening;

namespace smoothstudio.heroesandvillains.player
{
	[RequireComponent(typeof(BasePlayerInfo))]
	public class PlayerAttack : NetworkBehaviour
	{	
		private BasePlayerInfo playerInfo;
		private PlayerHealth playerHealth;
		private float attackCooldown = 0.5f;
		private bool canNormalFire = true;
		private bool useFireCooldown = true;

		private PlayerInfoPacket playerPacket;

		private Transform playerCameraTransform;
		PlayerGravityBody playerGravityBody;

		// Projectile firing
		public Transform projectileLauncher;
		private GameObject fireballPrefab;
		private int fireballPoolCount = 100;
		private List<GameObject> fireballPool;

		private GameObject lineDrawPrefab;
		private GameObject explosionParticlePrefab;
		private GameObject splashCollider;

		private bool controllerHasFired = false;

		private Color fireLineColor;

		void Awake() {
//			gameObject.AddGlobalEventListener(ProjectileEvent.DestroyProjectile, TidySpawnedItems);
//			gameObject.AddGlobalEventListener(ProjectileEvent.ProjectileHitPlayer, ProjectileHitSomePlayer);
			gameObject.AddGlobalEventListener(ProjectileEvent.MeleeHitPlayer, MeleeHitSomePlayer);
		}

		void Start() {
			playerInfo = gameObject.GetComponent<BasePlayerInfo>();
			playerHealth = gameObject.GetComponent<PlayerHealth>();
//			fireballPrefab = Resources.Load<GameObject>("Prefabs/Player/Fireball");
//			playerInfo.personalObjectPooler.CreatePool(fireballPrefab, fireballPoolCount);

			lineDrawPrefab = Resources.Load<GameObject>("Prefabs/Effects/LineDrawer");
			splashCollider = Resources.Load<GameObject>("Prefabs/Physics/SplashDamageCollider");

			if(playerInfo.playerTeam == Settings.HeroTeam) {
				fireLineColor = Color.cyan;
				explosionParticlePrefab = Resources.Load<GameObject>("Prefabs/Effects/Elementals/Thunder/Lightning Spark");
			} else if(playerInfo.playerTeam == Settings.VillainTeam) {
				fireLineColor = Color.red;
				explosionParticlePrefab = Resources.Load<GameObject>("Prefabs/Effects/Elementals/Fire/Explosion");				
			}

			playerCameraTransform = gameObject.GetComponentInChildren<Camera>().transform;
			if(isLocalPlayer) {
				playerPacket = new PlayerInfoPacket(playerInfo.playerName, playerInfo.playerTeam);
				playerGravityBody = GetComponent<PlayerGravityBody>();
			}
		}


		void Update() {
			if(isLocalPlayer) {
				RecieveInput();
			}
		}

		private void RecieveInput() {
			if (Input.GetMouseButtonDown(0)) {
				if(!canNormalFire && useFireCooldown) return;
				//CmdSpawn(); // Old shooting method (bad)
				StartCoroutine("NormalFireCooldown");
				RaycastFire();
				playerCameraTransform.DOShakePosition(0.2f, new Vector3(0.2f, 0.2f, 0), 1);
			}       

			// Controller needs a bit more help
			if(Input.GetAxis("ControllerFire") > 0 && !controllerHasFired) {
				controllerHasFired = true;
				RaycastFire();
				playerCameraTransform.DOShakePosition(0.2f, 0.2f, 1);	
			}
			if(Input.GetAxis("ControllerFire") <= 0 && controllerHasFired) {
				controllerHasFired = false;
			}
		}

		IEnumerator NormalFireCooldown() { // Adds cooldown so you can't spam fire
			canNormalFire = false;
			yield return new WaitForSeconds(attackCooldown);
			canNormalFire = true;
		}

		private void RaycastFire() { // New main shooting function
			RaycastHit hit;
			Vector3 fwd = projectileLauncher.TransformDirection(projectileLauncher.forward);
			if(Physics.Raycast(projectileLauncher.position, projectileLauncher.forward, out hit)) {

				CmdSpawnLine(transform.position, hit.point);
				CmdSpawnExplosion(hit.point);
				CmdSpawnExplosioncollider(hit.point, playerInfo.playerName, playerInfo.playerTeam);
				
				if(hit.collider.CompareTag("Player")) { // Actual player collision check
					//CmdApplyDamage(hit.collider.gameObject, playerInfo.damage); // TODO Currently can only have splash or raycast dmg
				} 
			} else {		
				CmdSpawnLine(transform.position, projectileLauncher.position + (projectileLauncher.forward * 100f));
				CmdSpawnExplosion(projectileLauncher.position + (projectileLauncher.forward * 100f));
				CmdSpawnExplosioncollider(projectileLauncher.position + (projectileLauncher.forward * 100f), playerInfo.playerName, playerInfo.playerTeam);
			}

			// Rocket jumping for fun
			float distanceToHit = Vector3.Distance(transform.position, hit.point);

			if(distanceToHit < 3f) {
				playerGravityBody.AddExplosionForce(hit.point, playerInfo.rocketJumpPower);
			}

		}

		[Command]
		private void CmdApplyDamage(GameObject obj, int damage) {
			obj.GetComponent<PlayerAttack>().RaycastShotHitPlayer(damage);
		}

		[Command]
		private void CmdSpawnLine(Vector3 start, Vector3 end)	{
			GameObject line = Instantiate<GameObject>(lineDrawPrefab);
			line.GetComponent<LineDrawer>().Setup(start, end, fireLineColor);
			NetworkServer.Spawn(line);		
		}

		[Command]
		private void CmdSpawnExplosion(Vector3 pos)	{
			GameObject explosion = Instantiate(explosionParticlePrefab);	
			explosion.transform.position = pos;
			NetworkServer.Spawn(explosion);
		}

		[Command]
		private void CmdSpawnExplosioncollider(Vector3 pos, string playerName, string playerTeam) {
			GameObject col = Instantiate(splashCollider);
			col.transform.position = pos;
			col.GetComponent<PlayerOwnedItem>().SetOwner(playerName, playerTeam);
			NetworkServer.Spawn(col);
			
		}

//		[Command]
//		private void CmdSpawn()	{
//			GameObject fireBall = playerInfo.personalObjectPooler.SpawnFromPool(fireballPrefab, null, projectileLauncher.position, projectileLauncher.eulerAngles);
//            fireBall.GetComponent<Rigidbody>().AddForce(projectileLauncher.forward * 30f, ForceMode.Impulse); // Seems wrong
//			NetworkServer.Spawn(fireBall);
//		}

		
		// ----------------------
		// Public callable from raycast hit
		// ----------------------

		public void RaycastShotHitPlayer(int damage) {
			playerHealth.TakeDamage(damage);
		}

		// ----------------------
		// Event handlers
		// ----------------------

//		private void TidySpawnedItems(EventObject evt) {
//			if(playerInfo.personalObjectPooler != null && evt.Params != null) {
//				GameObject toPass = (GameObject)evt.Params[0];
//				if(playerInfo.personalObjectPooler.SpawnedItemsContains(toPass)) {
//					playerInfo.personalObjectPooler.RecycleToPool(toPass);
//				}
//			}
//		}

//		private void ProjectileHitSomePlayer(EventObject evt) {
//			if(evt.Params != null) {
//				GameObject toPass = (GameObject)evt.Params[0];
//				if(!playerInfo.personalObjectPooler.SpawnedItemsContains(toPass)) { // Was fired not from this player
//					gameObject.DispatchGlobalEvent(ProjectileEvent.DestroyProjectile, new object[] {toPass});
//					playerHealth.TakeDamage((int)evt.Params[1]);
//				}
//			}
//		}

		private void MeleeHitSomePlayer(EventObject evt) {
			if (evt.Params != null) {
				if((BasePlayerInfo)evt.Params[0] == playerInfo) { // If the person hit was us
					playerHealth.TakeDamage((int)evt.Params[1]);
				}
			}
		}


	}
}