using UnityEngine;
using UnityEngine.UI;
using smoothstudio.heroesandvillains.player.events;

public class LobbySetupPanel : MonoBehaviour {

	public Text serverIPText;
	public Text heroCountText;
	public Text villainCountText;

	public Button outfitButton;
	public Button worldButton;
	public Button gamemodeButton;
	public Button randomNameButton;

	public GameObject outfitPanel;
	public GameObject worldPanel;
	public GameObject gamemodePanel;
	private GameObject currentPanel;

	public MapSelectController mapPanelController; // Used to set values back from lobby manager

	void Awake() {
		TurnOffPanel(outfitPanel);
		TurnOffPanel(worldPanel);
		TurnOffPanel(gamemodePanel);
		SetCurrentPanel(worldPanel);

		outfitButton.onClick.AddListener(()=>{SetCurrentPanel(outfitPanel);});
		worldButton.onClick.AddListener(()=>{SetCurrentPanel(worldPanel);});
		gamemodeButton.onClick.AddListener(()=>{SetCurrentPanel(gamemodePanel);});
		randomNameButton.onClick.AddListener(()=>{gameObject.DispatchGlobalEvent(MenuEvent.LobbyRandomNameButton);});
	}

	private void TurnOffPanel(GameObject panel) { 
		panel.SetActive(false);
	}

	private void SetCurrentPanel(GameObject newCurrentPanel) {
		if(currentPanel && currentPanel != newCurrentPanel) {
			currentPanel.SetActive(false);
			currentPanel = newCurrentPanel;
			currentPanel.SetActive(true);
		} else {
			currentPanel = newCurrentPanel;
			currentPanel.SetActive(true);
		}
	}

	// Publics
	public void SetHostIP(string ip) {
		serverIPText.text = ip;
	}
	public void SetPlayerCounts(int heroCount, int villainCount) {
		heroCountText.text = heroCount.ToString();
		villainCountText.text = villainCount.ToString();
	}

	// Send info back to panels from lobby maanager if needed
	public void SetMapCountValues(int metroCount, int borgCount, int candyCount){
		mapPanelController.SetVoteCountValues(metroCount, borgCount, candyCount);
	}
}
