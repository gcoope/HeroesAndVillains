using UnityEngine;
using System.Collections;
using smoothstudio.heroesandvillains.physics;
using UnityEngine.Networking;
using smoothstudio.heroesandvillains.player.events;
using System.Collections.Generic;
using smoothstudio.heroesandvillains.player.projectiles;

namespace smoothstudio.heroesandvillains.player
{
	[RequireComponent(typeof(BasePlayerInfo))]
	public class PlayerAttack : NetworkBehaviour
	{	
		private BasePlayerInfo playerInfo;
//		private float attackCooldown = 0.5f;

		// Projectile firing
		private Transform projectileLauncher;
		private GameObject fireballPrefab;
		private int fireballPoolCount = 100;
		private List<GameObject> fireballPool;

		void Awake() {
			gameObject.AddGlobalEventListener(ProjectileEvent.DestroyProjectile, TidySpawnedItems);
			gameObject.AddGlobalEventListener(ProjectileEvent.ProjectileHitPlayer, ProjectileHitSomePlayer);
			gameObject.AddGlobalEventListener(ProjectileEvent.MeleeHitPlayer, MeleeHitSomePlayer);
			foreach(Transform child in transform) if(child.CompareTag("ProjectileLauncher")) projectileLauncher = child; // Yay messy! But works.
		}

		void Start() {
			playerInfo = gameObject.GetComponent<BasePlayerInfo>();
			fireballPrefab = Resources.Load<GameObject>("Prefabs/Player/Fireball");
			playerInfo.personalObjectPooler.CreatePool(fireballPrefab, fireballPoolCount);
		}


		void Update() {
			if(isLocalPlayer) {
				RecieveInput();
			}
		}
		
		private void RecieveInput() {
			if (Input.GetMouseButtonDown(0)) {
				CmdSpawn();
			}            
		}

		[Command]
		private void CmdSpawn()	{
			GameObject fireBall = playerInfo.personalObjectPooler.SpawnFromPool(fireballPrefab, null, projectileLauncher.position, projectileLauncher.eulerAngles);
            fireBall.GetComponent<Rigidbody>().AddForce(projectileLauncher.forward * 30f, ForceMode.Impulse); // Seems wrong
			NetworkServer.Spawn(fireBall);
		}

		// ----------------------
		// Event handlers
		// ----------------------

		private void TidySpawnedItems(EventObject evt) {
			if(playerInfo.personalObjectPooler != null && evt.Params != null) {
				GameObject toPass = (GameObject)evt.Params[0];
				if(playerInfo.personalObjectPooler.SpawnedItemsContains(toPass)) {
					playerInfo.personalObjectPooler.RecycleToPool(toPass);
				}
			}
		}

		private void ProjectileHitSomePlayer(EventObject evt) {
			if(evt.Params != null) {
				GameObject toPass = (GameObject)evt.Params[0];
				if(!playerInfo.personalObjectPooler.SpawnedItemsContains(toPass)) { // Was fired not from this player
					gameObject.DispatchGlobalEvent(ProjectileEvent.DestroyProjectile, new object[] {toPass});
					playerInfo.TakeDamage((int)evt.Params[1]);
				}
			}
		}

		private void MeleeHitSomePlayer(EventObject evt) {
			if (evt.Params != null) {
				if((BasePlayerInfo)evt.Params[0] == playerInfo) { // If the person hit was us
					playerInfo.TakeDamage((int)evt.Params[1]);
				}
			}
		}

	}
}