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
		public const string ToggleVisibleCanvas = "MenuEvent.ToggleVisibleCanvas";

		public const string UpdateAudioVolumes = "MenuEvent.UpdateAudioVolumes";
		public const string EnableMouseSmoothing = "MenuEvent.EnableMouseSmoothing";
		public const string DisableMouseSmoothing = "MenuEvent.DisableMouseSmoothing";

		public const string StartMatchMaker = "MenuEvent.StartMatchMaker";
		public const string StopMatchMaker = "MenuEvent.StopMatchMaker";
		public const string CreateOnlineRoom = "MenuEvent.CreateOnlineRoom";
		public const string JoinDefaultRoom = "MenuEvent.JoinDefaultRoom";

		public const string ExitLobbyButton = "MenuEvent.ExitLobbyButton";
	}
}
