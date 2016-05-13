using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using smoothstudio.heroesandvillains.player.events;

namespace Prototype.NetworkLobby
{
    //Player entry in the lobby. Handle selecting color/setting name & getting ready for the game
    //Any LobbyHook can then grab it and pass those value to the game player prefab (see the Pong Example in the Samples Scenes)
    public class LobbyPlayer : NetworkLobbyPlayer
    {
        public Button teamButton;
        public InputField nameInput;
        public Button readyButton;
        public Button waitingPlayerButton;
        public Button removePlayerButton;

        public GameObject localIcon;
        public GameObject remoteIcon;

        [SyncVar(hook = "OnMyName")]
        public string playerName = "";
        [SyncVar(hook = "OnMyTeam")]
		public string playerTeam = Random.value > 0.4f ? Settings.HeroTeam : Settings.VillainTeam;

        public Color OddRowColor = new Color(250.0f / 255.0f, 250.0f / 255.0f, 250.0f / 255.0f, 1.0f);
        public Color EvenRowColor = new Color(200.0f / 255.0f, 200.0f / 255.0f, 200.0f / 255.0f, 1.0f);

		public Color JoinColor = new Color(240.0f/255.0f, 0.0f, 0.0f/255.0f,1.0f);
		public Color NotReadyColor = new Color(34.0f / 255.0f, 44 / 255.0f, 55.0f / 255.0f, 1.0f);
		public Color ReadyColor = new Color(0.0f, 204.0f / 255.0f, 204.0f / 255.0f, 1.0f);
		public Color TransparentColor = new Color(0, 0, 0, 0);

		public Color heroTeamColor;
		public Color villainTeamColor;

		void Awake() {
			gameObject.AddGlobalEventListener(MenuEvent.ExitLobbyButton, (delegate(EventObject obj) {
				OnRemovePlayerClick();	
			}));
		}

        public override void OnClientEnterLobby() {
            base.OnClientEnterLobby();

            if (LobbyManager.s_Singleton != null) LobbyManager.s_Singleton.OnPlayersNumberModified(1);

            LobbyPlayerList._instance.AddPlayer(this);
            LobbyPlayerList._instance.DisplayDirectServerWarning(isServer && LobbyManager.s_Singleton.matchMaker == null);

            if (isLocalPlayer)
            {
                SetupLocalPlayer();
            }
            else
            {
                SetupOtherPlayer();
            }

            //setup the player data on UI. The value are SyncVar so the player
            //will be created with the right value currently on server
            OnMyName(playerName);
            OnMyTeam(playerTeam);
        }

        public override void OnStartAuthority()
        {
            base.OnStartAuthority();

            // If we return from a game, color of text can still be the one for "Ready"
			readyButton.transform.GetChild(0).GetComponent<Text>().color = Color.black;

           SetupLocalPlayer();
        }

        void ChangeReadyButtonColor(Color c)
        {
            ColorBlock b = readyButton.colors;
            b.normalColor = c;
            b.pressedColor = c;
            b.highlightedColor = c;
            b.disabledColor = c;
            readyButton.colors = b;
        }

        void SetupOtherPlayer()
        {
            nameInput.interactable = false;
            removePlayerButton.interactable = NetworkServer.active;

            ChangeReadyButtonColor(NotReadyColor);

            readyButton.transform.GetChild(0).GetComponent<Text>().text = "...";
            readyButton.interactable = false;

            OnClientReady(false);
        }

        void SetupLocalPlayer()
        {
            nameInput.interactable = true;
            remoteIcon.gameObject.SetActive(false);
            localIcon.gameObject.SetActive(true);

            CheckRemoveButton();

            ChangeReadyButtonColor(JoinColor);

            readyButton.transform.GetChild(0).GetComponent<Text>().text = "READY UP";
            readyButton.interactable = true;

            if (playerName == "") CmdNameChanged(NameGenerator.GetRandomName());
//          CmdNameChanged("Player" + (LobbyPlayerList._instance.playerListContentTransform.childCount-1));

            // We switch from simple name display to name input
            teamButton.interactable = true;
            nameInput.interactable = true;

            nameInput.onEndEdit.RemoveAllListeners();
            nameInput.onEndEdit.AddListener(OnNameChanged);

            teamButton.onClick.RemoveAllListeners();
            teamButton.onClick.AddListener(OnTeamButtonClicked);

            readyButton.onClick.RemoveAllListeners();
            readyButton.onClick.AddListener(OnReadyClicked);

            //when OnClientEnterLobby is called, the local PlayerController is not yet created, so we need to redo that here to disable
            //the add button if we reach maxLocalPlayer. We pass 0, as it was already counted on OnClientEnterLobby
            if (LobbyManager.s_Singleton != null) LobbyManager.s_Singleton.OnPlayersNumberModified(0);
        }

        //This enable/disable the remove button depending on if that is the only local player or not
        public void CheckRemoveButton()
        {
            if (!isLocalPlayer)
                return;

            int localPlayerCount = 0;
            foreach (PlayerController p in ClientScene.localPlayers)
                localPlayerCount += (p == null || p.playerControllerId == -1) ? 0 : 1;

            removePlayerButton.interactable = localPlayerCount > 1;
        }

        public override void OnClientReady(bool readyState)
        {
            if (readyState)
            {
                ChangeReadyButtonColor(TransparentColor);

                Text textComponent = readyButton.transform.GetChild(0).GetComponent<Text>();
                textComponent.text = "READY";
                textComponent.color = ReadyColor;
                readyButton.interactable = false;
                teamButton.interactable = false;
                nameInput.interactable = false;
            }
            else
            {
                ChangeReadyButtonColor(isLocalPlayer ? JoinColor : NotReadyColor);

                Text textComponent = readyButton.transform.GetChild(0).GetComponent<Text>();
                textComponent.text = isLocalPlayer ? "READY UP" : "...";
				textComponent.color = Color.black;
                readyButton.interactable = isLocalPlayer;
                teamButton.interactable = isLocalPlayer;
                nameInput.interactable = isLocalPlayer;
            }
        }

        public void OnPlayerListChanged(int idx)
        { 
			GetComponent<Image>().color = (idx % 2 == 0) ? EvenRowColor : OddRowColor;
        }

        ///===== callback from sync var

        public void OnMyName(string newName) {
            playerName = newName;
            nameInput.text = playerName;
        }

		public void OnMyTeam(string newTeam) {
            playerTeam = newTeam;
			if(newTeam == Settings.HeroTeam) {
				teamButton.GetComponent<Image>().color = heroTeamColor;
			} else {
				teamButton.GetComponent<Image>().color = villainTeamColor;
			}

//			CmdTeamChanged(playerTeam);
        }

        //===== UI Handler

        //Note that those handler use Command function, as we need to change the value on the server not locally
        //so that all client get the new value throught syncvar
        public void OnTeamButtonClicked() {
            CmdChangeTeam();
        }

        public void OnReadyClicked() {
            SendReadyToBeginMessage();
        }

        public void OnNameChanged(string str) {
            CmdNameChanged(str);
        }

        public void OnRemovePlayerClick() {
            if (isLocalPlayer)
            {
				readyToBegin = false;
                RemovePlayer();
//				NetworkServer.DisconnectAll();
//				 TODO Disconnect properly
            }
//            else if (isServer)
//                LobbyManager.s_Singleton.KickPlayer(connectionToClient);
//                
        }

        public void ToggleJoinButton(bool enabled) {
            readyButton.gameObject.SetActive(enabled);
            waitingPlayerButton.gameObject.SetActive(!enabled);
        }

        [ClientRpc]
        public void RpcUpdateCountdown(int countdown) {
            LobbyManager.s_Singleton.countdownPanel.UIText.text = "Match Starting in " + countdown;
            LobbyManager.s_Singleton.countdownPanel.gameObject.SetActive(countdown != 0);
        }

        [ClientRpc]
        public void RpcUpdateRemoveButton() {
            CheckRemoveButton();
        }

        //====== Server Command

        [Command]
        public void CmdChangeTeam() {
			if(playerTeam == Settings.HeroTeam) {
				playerTeam = Settings.VillainTeam;
			} else {
				playerTeam = Settings.HeroTeam;
			}
        }

        [Command]
        public void CmdNameChanged(string name) {
            playerName = name;
        }

		[Command]
		public void CmdTeamChanged(string team) {
			playerTeam = team;
		}
       
		public void OnDestroy() { // Cleanup thing when get destroy (which happen when client kick or disconnect)
            LobbyPlayerList._instance.RemovePlayer(this);
            if (LobbyManager.s_Singleton != null) LobbyManager.s_Singleton.OnPlayersNumberModified(-1);
        }
    }
}
