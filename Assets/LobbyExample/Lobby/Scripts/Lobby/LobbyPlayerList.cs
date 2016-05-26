using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Prototype.NetworkLobby
{
    //List of players in the lobby
    public class LobbyPlayerList : MonoBehaviour
    {
        public static LobbyPlayerList _instance = null;

		public LobbySetupPanel setupPanel;
        public RectTransform playerListContentTransform;
        public GameObject warningDirectPlayServer;
        public Transform addButtonRow;

        protected VerticalLayoutGroup _layout;
        protected List<LobbyPlayer> _players = new List<LobbyPlayer>();

		private int heroPlayerCount = 0;
		private int villainPlayerCount = 0;

		// Map preferences
		private Dictionary<SettingsGameWorld, int> mapVotes;

		// Game preferences
		private Dictionary<SettingsGameMode, int> gameVotes;



        public void OnEnable()
        {
            _instance = this;
			ResetMapVotes();
			ResetGameVotes();
            _layout = playerListContentTransform.GetComponent<VerticalLayoutGroup>();
        }

		private void ResetMapVotes() {
			mapVotes = new Dictionary<SettingsGameWorld, int>();
			mapVotes.Add(SettingsGameWorld.METROPOLIS, 0);
			mapVotes.Add(SettingsGameWorld.BORG, 0);
			mapVotes.Add(SettingsGameWorld.CANDYLAND, 0);
			mapVotes.Add(SettingsGameWorld.DESERT, 0);
		}

		private void ResetGameVotes() {
			gameVotes = new Dictionary<SettingsGameMode, int>();
			gameVotes.Add(SettingsGameMode.ARENA, 0);
			gameVotes.Add(SettingsGameMode.CAPTURE_THE_FLAG, 0);
			gameVotes.Add(SettingsGameMode.ZONE_CAPTURE, 0);
			gameVotes.Add(SettingsGameMode.SUPERIORITY, 0);
		}

        public void DisplayDirectServerWarning(bool enabled)
        {
            if(warningDirectPlayServer != null)
				warningDirectPlayServer.SetActive(false);
//				warningDirectPlayServer.SetActive(enabled);
        }

        void Update() {
            //this dirty the layout to force it to recompute evryframe (a sync problem between client/server
            //sometime to child being assigned before layout was enabled/init, leading to broken layouting)
            
            if(_layout) _layout.childAlignment = Time.frameCount%2 == 0 ? TextAnchor.UpperCenter : TextAnchor.UpperLeft;
        }

        public void AddPlayer(LobbyPlayer player)
        {
            if (_players.Contains(player))
                return;

            _players.Add(player);

            player.transform.SetParent(playerListContentTransform, false);
            PlayerListModified();
        }

        public void RemovePlayer(LobbyPlayer player) {
			if(_players != null && _players.Count > 0) {
	            _players.Remove(player);
	            PlayerListModified();
			}
        }

		public void RemoveAllPlayers() {
			for(int i = 0; i < _players.Count; i++) {
				_players.Remove(_players[i]);
			}
			for(int i = 0; i < playerListContentTransform.childCount-1; i++) {
				if(playerListContentTransform.GetChild(i)) Destroy(playerListContentTransform.GetChild(i).gameObject);
				_players.Remove(_players[i]);
			}
			PlayerListModified();
		}

        public void PlayerListModified() {
            int i = 0;
            foreach (LobbyPlayer p in _players) {
                p.OnPlayerListChanged(i);
                ++i;
            }

			PlayerPreferenceModified();
        }

		public void PlayerPreferenceModified() {
			heroPlayerCount = villainPlayerCount = 0; 
			ResetMapVotes();
			ResetGameVotes();
			int i = 0;
			foreach (LobbyPlayer p in _players) {
				// Team player count
				if(p.playerTeam == Settings.HeroTeam) heroPlayerCount++;
				else if(p.playerTeam == Settings.VillainTeam) villainPlayerCount++;

				// Map pref
				if(p.preferredMap == 0) mapVotes[SettingsGameWorld.METROPOLIS]++;
				else if(p.preferredMap == 1) mapVotes[SettingsGameWorld.BORG]++;
				else if(p.preferredMap == 2) mapVotes[SettingsGameWorld.CANDYLAND]++;
				else if(p.preferredMap == 3) mapVotes[SettingsGameWorld.DESERT]++;

				// Game pref
				if(p.preferredGameMode == 0) gameVotes[SettingsGameMode.ARENA]++;
				else if(p.preferredGameMode == 1) gameVotes[SettingsGameMode.CAPTURE_THE_FLAG]++;
				else if(p.preferredGameMode == 2) gameVotes[SettingsGameMode.ZONE_CAPTURE]++;
				else if(p.preferredGameMode == 3) gameVotes[SettingsGameMode.SUPERIORITY]++;

				++i;
			}

			setupPanel.SetPlayerCounts(heroPlayerCount, villainPlayerCount);
			setupPanel.SetMapCountValues(mapVotes[SettingsGameWorld.METROPOLIS], mapVotes[SettingsGameWorld.BORG], mapVotes[SettingsGameWorld.CANDYLAND], mapVotes[SettingsGameWorld.DESERT]);
			setupPanel.SetGameCountValues(gameVotes[SettingsGameMode.ARENA], gameVotes[SettingsGameMode.CAPTURE_THE_FLAG], gameVotes[SettingsGameMode.ZONE_CAPTURE], gameVotes[SettingsGameMode.SUPERIORITY]);

			#region map votes
			List<SettingsGameWorld> votedWorlds = new List<SettingsGameWorld>(4);
			SettingsGameWorld highestMap = SettingsGameWorld.METROPOLIS;
			int highestMapVotes = 0;

			if(mapVotes[SettingsGameWorld.METROPOLIS] > highestMapVotes) { 
				highestMap = SettingsGameWorld.METROPOLIS;
				highestMapVotes = mapVotes[SettingsGameWorld.METROPOLIS];
			}
			if(mapVotes[SettingsGameWorld.BORG] > highestMapVotes) { 
				highestMap = SettingsGameWorld.BORG;
				highestMapVotes = mapVotes[SettingsGameWorld.BORG];
			}
			if(mapVotes[SettingsGameWorld.CANDYLAND] > highestMapVotes) {
				highestMap = SettingsGameWorld.CANDYLAND;
				highestMapVotes = mapVotes[SettingsGameWorld.CANDYLAND];
			}
			if(mapVotes[SettingsGameWorld.DESERT] > highestMapVotes) {
				highestMap = SettingsGameWorld.DESERT;
				highestMapVotes = mapVotes[SettingsGameWorld.DESERT];
			}

			votedWorlds.Add(highestMap);

			if(mapVotes[SettingsGameWorld.METROPOLIS] == mapVotes[highestMap] && highestMap != SettingsGameWorld.METROPOLIS) votedWorlds.Add(SettingsGameWorld.METROPOLIS);
			if(mapVotes[SettingsGameWorld.BORG] == mapVotes[highestMap] && highestMap != SettingsGameWorld.BORG) votedWorlds.Add(SettingsGameWorld.BORG);
			if(mapVotes[SettingsGameWorld.CANDYLAND] == mapVotes[highestMap] && highestMap != SettingsGameWorld.CANDYLAND) votedWorlds.Add(SettingsGameWorld.CANDYLAND);
			if(mapVotes[SettingsGameWorld.DESERT] == mapVotes[highestMap] && highestMap != SettingsGameWorld.DESERT) votedWorlds.Add(SettingsGameWorld.DESERT);

			highestMap = votedWorlds[Random.Range(0, votedWorlds.Count)];

			switch(highestMap) {
			case SettingsGameWorld.METROPOLIS:
				LobbyManager.s_Singleton.playScene = LevelNames.Metropolis;
				break;
			case SettingsGameWorld.BORG:
				LobbyManager.s_Singleton.playScene = LevelNames.BorgWorld;
				break;
			case SettingsGameWorld.CANDYLAND:
				LobbyManager.s_Singleton.playScene = LevelNames.Candyland;
				break;
			case SettingsGameWorld.DESERT:
				LobbyManager.s_Singleton.playScene = LevelNames.Desert;
				break;
			}
			#endregion

			#region game votes
			List<SettingsGameMode> votedGames = new List<SettingsGameMode>(4);
			SettingsGameMode highestGame = SettingsGameMode.ARENA;
			int highestGameVotes = 0;

			if(gameVotes[SettingsGameMode.ARENA] > highestGameVotes) { 
				highestGame = SettingsGameMode.ARENA;
				highestGameVotes = gameVotes[SettingsGameMode.ARENA];
			}
			if(gameVotes[SettingsGameMode.CAPTURE_THE_FLAG] > highestGameVotes) { 
				highestGame = SettingsGameMode.CAPTURE_THE_FLAG;
				highestGameVotes = gameVotes[SettingsGameMode.CAPTURE_THE_FLAG];
			}
			if(gameVotes[SettingsGameMode.ZONE_CAPTURE] > highestGameVotes) {
				highestGame = SettingsGameMode.ZONE_CAPTURE;
				highestGameVotes = gameVotes[SettingsGameMode.ZONE_CAPTURE];
			}
			if(gameVotes[SettingsGameMode.SUPERIORITY] > highestGameVotes) {
				highestGame = SettingsGameMode.SUPERIORITY;
				highestGameVotes = gameVotes[SettingsGameMode.SUPERIORITY];
			}

			votedGames.Add(highestGame);

			if(gameVotes[SettingsGameMode.ARENA] == gameVotes[highestGame] && highestGame != SettingsGameMode.ARENA) votedGames.Add(SettingsGameMode.ARENA);
			if(gameVotes[SettingsGameMode.CAPTURE_THE_FLAG] == gameVotes[highestGame] && highestGame != SettingsGameMode.CAPTURE_THE_FLAG) votedGames.Add(SettingsGameMode.CAPTURE_THE_FLAG);
			if(gameVotes[SettingsGameMode.ZONE_CAPTURE] == gameVotes[highestGame] && highestGame != SettingsGameMode.ZONE_CAPTURE) votedGames.Add(SettingsGameMode.ZONE_CAPTURE);
			if(gameVotes[SettingsGameMode.SUPERIORITY] == gameVotes[highestGame] && highestGame != SettingsGameMode.SUPERIORITY) votedGames.Add(SettingsGameMode.SUPERIORITY);

			highestGame = votedGames[Random.Range(0, votedGames.Count)];

			switch(highestGame) { // TODO Different game mode votes here
			case SettingsGameMode.ARENA:
//				LobbyManager.s_Singleton.playScene = LevelNames.Metropolis;
				break;
			case SettingsGameMode.CAPTURE_THE_FLAG:
//				LobbyManager.s_Singleton.playScene = LevelNames.BorgWorld;
				break;
			case SettingsGameMode.ZONE_CAPTURE:
//				LobbyManager.s_Singleton.playScene = LevelNames.Candyland;
				break;
			case SettingsGameMode.SUPERIORITY:
//				LobbyManager.s_Singleton.playScene = LevelNames.Desert;
				break;
			}
			#endregion
		}
    }
}
