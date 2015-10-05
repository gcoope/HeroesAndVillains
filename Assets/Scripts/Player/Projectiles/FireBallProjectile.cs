using System;
using UnityEngine;
using System.Collections;


namespace smoothstudio.heroesandvillains.player.projectiles
{
	public class FireBallProjectile : MonoBehaviour, IProjectile	{

		public float lifeTime = 5f;

		void Awake() {
			StartCoroutine("SelfDestruct");
		}

		IEnumerator SelfDestruct() {
			yield return new WaitForSeconds(lifeTime);
			TidyUp();
		}

		public void TidyUp() {
			Destroy(gameObject); // TODO Pooling (please)
		}

	}
}

