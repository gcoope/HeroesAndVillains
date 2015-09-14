using System;
using UnityEngine;
using UnityEngine.Networking;
using smoothstudio.heroesandvillains.player;
using smoothstudio.heroesandvillains.physics;

namespace smoothstudio.heroesandvillains.networking
{
	public class PlayerNetworkControl : NetworkBehaviour
	{
		public override void OnStartLocalPlayer() {
			gameObject.GetComponent<PlanetPlayerMove>().enabled = true;
			gameObject.GetComponent<PlanetPlayerMove>().playerCamera.enabled = true;
			gameObject.GetComponent<PlayerGravityBody>().localPlayer = true;
		}
	}
}