using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


namespace smoothstudio.heroesandvillains.player.projectiles
{
	public class FireBallProjectile : NetworkBehaviour, IProjectile	{

		public float lifeTime = 5f;
		[SerializeField] private BasePlayerInfo player;

		void OnEnable() {
//			StartCoroutine("SelfDestruct");			
		}

		public void Fire(BasePlayerInfo playerInfo) {
			StartCoroutine("SelfDestruct");		
			if(this.player == null) this.player = playerInfo;
		}

		public void Init(BasePlayerInfo player) {
			this.player = player;
		}

		IEnumerator SelfDestruct() {
			yield return new WaitForSeconds(lifeTime);
			TidyUp();
		}

		public void TidyUp() {
			player.personalObjectPooler.RecycleToPool(gameObject);
		}

		void OnCollisionEnter(Collision col) {
			if(col.gameObject.CompareTag("Player")) {
				if(col.gameObject.GetComponent<BasePlayerInfo>() != this.player) {
					Debug.Log("hit someone else");
					TidyUp();
				}
			}
		}

	}
}

