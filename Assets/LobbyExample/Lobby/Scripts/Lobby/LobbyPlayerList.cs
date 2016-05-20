using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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
		public int metroMapCount = 0;
		public int borgMapCount = 0;
		public int candylandMapCount = 0;
		private Dictionary<SettingsGameWorld, int> mapVotes;

        public void OnEnable()
        {
            _instance = this;
			ResetMapVotes();
            _layout = playerListContentTransform.GetComponent<VerticalLayoutGroup>();
        }

		private void ResetMapVotes() {
			mapVotes = new Dictionary<SettingsGameWorld, int>();
			mapVotes.Add(SettingsGameWorld.METROPOLIS, 0);
			mapVotes.Add(SettingsGameWorld.BORG, 0);
			mapVotes.Add(SettingsGameWorld.CANDYLAND, 0);
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
            
            if(_layout)
                _layout.childAlignment = Time.frameCount%2 == 0 ? TextAnchor.UpperCenter : TextAnchor.UpperLeft;
        }

        public void AddPlayer(LobbyPlayer player)
        {
            if (_players.Contains(player))
                return;

            _players.Add(player);

            player.transform.SetParent(playerListContentTransform, false);
         //   addButtonRow.transform.SetAsLastSibling();
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

			setupPanel.SetPlayerCounts(heroPlayerCount, villainPlayerCount);
        }

		public void PlayerPreferenceModified() {
			heroPlayerCount = villainPlayerCount = 0; 
			ResetMapVotes();
			int i = 0;
			foreach (LobbyPlayer p in _players) {
				// Team player count
				if(p.playerTeam == Settings.HeroTeam) heroPlayerCount++;
				else if(p.playerTeam == Settings.VillainTeam) villainPlayerCount++;

				// Map pref
				if(p.preferredMap == 0) mapVotes[SettingsGameWorld.METROPOLIS]++;
				else if(p.preferredMap == 1) mapVotes[SettingsGameWorld.BORG]++;
				else if(p.preferredMap == 2) mapVotes[SettingsGameWorld.CANDYLAND]++;
				++i;
			}

			setupPanel.SetMapCountValues(mapVotes[SettingsGameWorld.METROPOLIS], mapVotes[SettingsGameWorld.BORG], mapVotes[SettingsGameWorld.CANDYLAND]);

			// Ugly but can't think of an elegant solution TODO

			List<SettingsGameWorld> votedWorlds = new List<SettingsGameWorld>(3);
			SettingsGameWorld highestMap = SettingsGameWorld.METROPOLIS;
			int highestVotes = 0;

			if(mapVotes[SettingsGameWorld.METROPOLIS] > highestVotes) { 
				highestMap = SettingsGameWorld.METROPOLIS;
				highestVotes = mapVotes[SettingsGameWorld.METROPOLIS];
			}
			if(mapVotes[SettingsGameWorld.BORG] > highestVotes) { 
				highestMap = SettingsGameWorld.BORG;
				highestVotes = mapVotes[SettingsGameWorld.BORG];
			}
			if(mapVotes[SettingsGameWorld.CANDYLAND] > highestVotes) {
				highestMap = SettingsGameWorld.CANDYLAND;
				highestVotes = mapVotes[SettingsGameWorld.CANDYLAND];
			}

			votedWorlds.Add(highestMap);

			if(mapVotes[SettingsGameWorld.METROPOLIS] == mapVotes[highestMap] && highestMap != SettingsGameWorld.METROPOLIS) votedWorlds.Add(SettingsGameWorld.METROPOLIS);
			if(mapVotes[SettingsGameWorld.BORG] == mapVotes[highestMap] && highestMap != SettingsGameWorld.BORG) votedWorlds.Add(SettingsGameWorld.BORG);
			if(mapVotes[SettingsGameWorld.CANDYLAND] == mapVotes[highestMap] && highestMap != SettingsGameWorld.CANDYLAND) votedWorlds.Add(SettingsGameWorld.CANDYLAND);

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
			}
		}
    }
}
