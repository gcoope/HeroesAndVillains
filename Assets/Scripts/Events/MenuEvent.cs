using UnityEngine;
using System.Collections;

namespace smoothstudio.heroesandvillains.player.events {
	public class MenuEvent : MonoBehaviour {
		
		public static string JoinLocal = "MenuEvent.JoinLocal";
		public static string HostLocal = "MenuEvent.HostLocal";
		public static string HostServer = "MenuEvent.HostServer";
		public static string CloseGame = "MenuEvent.CloseGame";
		public static string ClientDisconnect = "MenuEvent.ClientDisconnect";

		public static string InputFieldChange = "MenuEvent.InputFieldChange";
	}
}
