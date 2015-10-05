using UnityEngine;
using System.Collections;
using smoothstudio.heroesandvillains.physics;
using UnityEngine.Networking;
using smoothstudio.heroesandvillains.player.events;
using System.Collections.Generic;

namespace smoothstudio.heroesandvillains.player
{
	public class PlayerAttack : NetworkBehaviour
	{	
		private BasePlayerInfo playerInfo;
		private float attackCooldown = 0.5f;

		// Projectile firing
		private Transform projectileLauncher;
		private GameObject fireballPrefab;
		private List<GameObject> fireballPool; // #soon

		void Awake() {
			playerInfo = gameObject.GetComponent<BasePlayerInfo>();
			foreach(Transform child in transform) if(child.CompareTag("ProjectileLauncher")) projectileLauncher = child; // Messy
		}

		void Start() {
			fireballPrefab = Resources.Load<GameObject>("Prefabs/Player/Fireball");
		}

		void Update() {
			if(isLocalPlayer) {
				RecieveInput();
			}
		}
		
		private void RecieveInput() {
			if (Input.GetKeyDown(KeyCode.F)) {
				RangedAttack();
			}            
		}

		private void RangedAttack() {
//			RaycastHit rayHit;
//			if(Physics.Raycast(transform.position, transform.forward, out rayHit, 20)){
//				Debug.DrawLine (transform.position, rayHit.point, Color.cyan);
//			}

			// Lets throw some fire/energy balls
			GameObject fireBall = Instantiate<GameObject>(fireballPrefab);
			fireBall.transform.position = projectileLauncher.position;
			fireBall.transform.eulerAngles = projectileLauncher.eulerAngles;
			fireBall.GetComponent<Rigidbody>().AddForce(projectileLauncher.forward * 30f, ForceMode.Impulse);
		}
	}
}