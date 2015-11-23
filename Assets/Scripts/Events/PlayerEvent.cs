using UnityEngine;
using System.Collections;

namespace smoothstudio.heroesandvillains.player.events {
    public class PlayerEvent : MonoBehaviour {

        public static string PlayerJump = "PlayerMoveEvent.PlayerJump";
        public static string PlayerAttack = "PlayerMoveEvent.PlayerAttack";
		public static string PlayerDamaged = "PlayerEvent.PlayerDamaged";
		public static string PlayerFaint = "PlayerEvent.PlayerFaint";
		public static string PlayerRespawn = "PlayerEvent.PlayerRespawn";
    }
}
