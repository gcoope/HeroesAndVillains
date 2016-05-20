using UnityEngine;
using UnityEngine.UI;
using System.Collections;
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
		public int metroMapCount = 0;
		public int borgMapCount = 0;
		public int candylandMapCount = 0;

        public void OnEnable()
        {
            _instance = this;
            _layout = playerListContentTransform.GetComponent<VerticalLayoutGroup>();
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

        public void RemovePlayer(LobbyPlayer player)
        {
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
			heroPlayerCount = villainPlayerCount = metroMapCount = borgMapCount = candylandMapCount = 0; // It works...
			int i = 0;
			foreach (LobbyPlayer p in _players) {
				// Team player count
				if(p.playerTeam == Settings.HeroTeam) heroPlayerCount++;
				else if(p.playerTeam == Settings.VillainTeam) villainPlayerCount++;

				// Map pref
				if(p.preferredMap == 0) metroMapCount++;
				else if(p.preferredMap == 1) borgMapCount++;
				else if(p.preferredMap == 2) candylandMapCount++;
				++i;
			}

			setupPanel.SetMapCountValues(metroMapCount, borgMapCount, candylandMapCount);
//			Debug.Log(Mathf.Max(metroMapCount, borgMapCount, candylandMapCount));

			// TODO Something with the highest value - what if two are even?
		}
    }
}
