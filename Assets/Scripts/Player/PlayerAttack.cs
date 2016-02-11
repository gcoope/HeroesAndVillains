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
		private float attackCooldown = 0.5f;
		private bool canNormalFire = true;
		private bool useFireCooldown = true;

		private Transform playerCameraTransform;
		PlayerGravityBody playerGravityBody;

		// Projectile firing
		public Transform projectileLauncher;
		private GameObject fireballPrefab;
		private int fireballPoolCount = 100;
		private List<GameObject> fireballPool;

		private GameObject lineDrawPrefab;
		private GameObject splashCollider;

		GameObject lightningSparkPrefab;
		GameObject fireExplosionPrefab;
		bool isHero;

		private bool controllerHasFired = false;
		private bool controllerCanFire = true;

		private Color fireLineColor;

		private bool allowAnyInput = true;

		private PlayerInfoPacket localPlayerInfoPacket;

		void Awake() {
			gameObject.AddGlobalEventListener(ProjectileEvent.MeleeHitPlayer, MeleeHitSomePlayer);
		}

		void Start() {
			playerInfo = gameObject.GetComponent<BasePlayerInfo>();
			playerHealth = gameObject.GetComponent<PlayerHealth>();

			lineDrawPrefab = Resources.Load<GameObject>("Prefabs/Effects/LineDrawer");
			splashCollider = Resources.Load<GameObject>("Prefabs/Physics/SplashDamageCollider");

			lightningSparkPrefab = Resources.Load<GameObject>("Prefabs/Effects/Elementals/Thunder/Lightning Spark");
			fireExplosionPrefab = Resources.Load<GameObject>("Prefabs/Effects/Elementals/Fire/Explosion");

			if(playerInfo.playerTeam == Settings.HeroTeam) {
				fireLineColor = Color.cyan;
			} else {
				fireLineColor = Color.red;			
			}

			isHero = playerInfo.playerTeam == Settings.HeroTeam;

			playerCameraTransform = gameObject.GetComponentInChildren<Camera>().transform;
			if(isLocalPlayer) {
				playerGravityBody = GetComponent<PlayerGravityBody>();
			}

			localPlayerInfoPacket = new PlayerInfoPacket(playerInfo.playerName, playerInfo.playerTeam, netId);
		}

		void Update() {
			if(isLocalPlayer && allowAnyInput) {
				RecieveInput();
			}
		}

		public void SetAllowAnyInput(bool allow) {
			allowAnyInput = allow;
		}

		private void RecieveInput() {
			if (Input.GetMouseButtonDown(0)) {
				if(!canNormalFire && useFireCooldown) return;
				StartCoroutine("NormalFireCooldown");
				RaycastFire();
				playerCameraTransform.DOShakePosition(0.2f, new Vector3(0.2f, 0.2f, 0), 1);
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

		IEnumerator ControllerFireCooldown() { // Adds cooldown so you can't spam fire
			controllerCanFire = false;
			yield return new WaitForSeconds(attackCooldown);
			controllerCanFire = true;
		}

		private void RaycastFire() { // New main shooting function
			RaycastHit hit;
//			Vector3 fwd = projectileLauncher.TransformDirection(projectileLauncher.forward);
			if(Physics.Raycast(projectileLauncher.position, projectileLauncher.forward, out hit)) {
				CmdSpawnLine(transform.position, hit.point, fireLineColor);
				CmdSpawnExplosion(hit.point, isHero);
				CmdSpawnExplosioncollider(hit.point, localPlayerInfoPacket);

			} else { // Missed everything so we draw a line 100 units and spawn an explosion (with no collider)
				CmdSpawnLine(transform.position, projectileLauncher.position + (projectileLauncher.forward * 100f), fireLineColor);
				CmdSpawnExplosion(projectileLauncher.position + (projectileLauncher.forward * 100f), isHero);
			}

			// Rocket jumping 
			float distanceToHit = Vector3.Distance(transform.position, hit.point);
			if(distanceToHit < 3f) {
				playerGravityBody.AddExplosionForce(hit.point, playerInfo.rocketJumpPower);
			}
		}

		[Command]
		private void CmdSpawnLine(Vector3 start, Vector3 end, Color col)	{
			GameObject line = Instantiate<GameObject>(lineDrawPrefab);
			line.GetComponent<LineDrawer>().Setup(start, end, col);
			NetworkServer.Spawn(line);		
		}

		[Command]
		private void CmdSpawnExplosion(Vector3 pos, bool isHero)	{
			GameObject explosion = Instantiate(isHero ? lightningSparkPrefab : fireExplosionPrefab);	
			explosion.transform.position = pos;
			NetworkServer.Spawn(explosion);
		}

		[Command]
		private void CmdSpawnExplosioncollider(Vector3 pos, PlayerInfoPacket playerInfoPacket) {
			GameObject col = Instantiate(splashCollider);
			col.transform.position = pos;
			col.GetComponent<SplashDamagerCollider>().SetOwner(playerInfoPacket);
			NetworkServer.Spawn(col); // TODO We might be able to do all collisions on the server someday..		
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