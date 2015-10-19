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
			foreach(Transform child in transform) if(child.CompareTag("ProjectileLauncher")) projectileLauncher = child; // Yay messy! But works.
		}

		void Start() {
			playerInfo = gameObject.GetComponent<BasePlayerInfo>();
			fireballPrefab = Resources.Load<GameObject>("Prefabs/Player/Fireball");
			if(!playerInfo.personalObjectPooler.PoolExists(fireballPrefab)) {
				playerInfo.personalObjectPooler.CreatePool(fireballPrefab, fireballPoolCount);
			} else {
				playerInfo.personalObjectPooler.AppendToPool(fireballPrefab, fireballPoolCount);
			}

			fireballPool = playerInfo.personalObjectPooler.GetPrefabList(fireballPrefab);
			if(fireballPool != null) {
				foreach(GameObject fireball in fireballPool) {
					fireball.SetActive(true);
					fireball.GetComponent<FireBallProjectile>().Init(playerInfo);
					fireball.SetActive(false);
				}
				playerInfo.personalObjectPooler.ReplacePrefabList(fireballPrefab, fireballPool);
			} else {
				Debug.LogWarning("Unable to get pool from personalPooler");
			}
		}


		void Update() {
			if(isLocalPlayer) {
				RecieveInput();
			}
		}
		
		private void RecieveInput() {
			if (Input.GetKeyDown(KeyCode.F)) {
				CmdSpawn();
			}            
		}

		[Command]
		private void CmdSpawn()	{
			GameObject fireBall = playerInfo.personalObjectPooler.SpawnFromPool(fireballPrefab, null, projectileLauncher.position, projectileLauncher.eulerAngles);
			fireBall.GetComponent<Rigidbody>().AddForce(projectileLauncher.forward * 30f, ForceMode.Impulse); 
			fireBall.GetComponent<FireBallProjectile>().Fire(playerInfo); // Eugh, GetComponent TODO improve me
			NetworkServer.Spawn(fireBall);
		}

	}
}