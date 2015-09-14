using System;
using UnityEngine;
using UnityEngine.Networking;
using smoothstudio.heroesandvillians.player;
using smoothstudio.heroesandvillians.physics;

namespace smoothstudio.heroesandvillians.networking
{
	public class PlayerNetworkControl : NetworkBehaviour
	{
		public override void OnStartLocalPlayer() {
			gameObject.GetComponent<PlayerMove>().enabled = true;
			gameObject.GetComponent<PlayerMove>().playerCamera.enabled = true;
			gameObject.GetComponent<PlayerGravityBody>().localPlayer = true;
		}
	}
}