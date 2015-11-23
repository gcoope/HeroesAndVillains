using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using smoothstudio.heroesandvillains.player.events;


namespace smoothstudio.heroesandvillains.player.projectiles
{
	public class FireBallProjectile : NetworkBehaviour	{

		private float lifeTime = 5f;
		private int damage = 10;

		void OnEnable() {
			StartCoroutine("SelfDestruct");			
		}

		IEnumerator SelfDestruct() {
			yield return new WaitForSeconds(lifeTime);
			TidyUp();
		}

		public void TidyUp() {
			gameObject.DispatchGlobalEvent(ProjectileEvent.DestroyProjectile, new object[] { gameObject });
		}

		void OnCollisionEnter(Collision col) {
			if(col.gameObject.CompareTag("Player")) {
				gameObject.DispatchGlobalEvent(ProjectileEvent.ProjectileHitPlayer, new object[] {gameObject, damage});
			}
		}

	}
}

