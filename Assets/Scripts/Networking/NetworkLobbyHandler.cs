using UnityEngine;
using Prototype.NetworkLobby;
using UnityEngine.Networking;

public class NetworkLobbyHandler : LobbyHook {

	public override void OnLobbyServerSceneLoadedForPlayer (UnityEngine.Networking.NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer) {
		LobbyPlayer lobbyInfo = lobbyPlayer.GetComponent<LobbyPlayer>();
		gamePlayer.GetComponent<BasePlayerInfo>().playerName = lobbyInfo.playerName;
		PlayerPrefs.SetString(PlayerPrefKeys.LocalPlayerName, lobbyInfo.playerName);
		PlayerPrefs.SetString(PlayerPrefKeys.LocalPlayerTeam, lobbyInfo.playerTeam);
//		NetworkServer.ReplacePlayerForConnection(manager.client.connection, gamePlayer, manager.client.connection.playerControllers[0].playerControllerId);
		base.OnLobbyServerSceneLoadedForPlayer (manager, lobbyPlayer, gamePlayer);
    }

}
