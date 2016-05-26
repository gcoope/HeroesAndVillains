using UnityEngine;
using UnityEngine.UI;
using smoothstudio.heroesandvillains.player.events;

public class LobbySetupPanel : MonoBehaviour {

	public Text serverIPText;
	public Text heroCountText;
	public Text villainCountText;

	public Button outfitButton;
	public GameObject outfitPanel;


	public MapSelectController mapPanelController; // Used to set values back from lobby manager
	public GameModeSelectController gameSelectController;

	void Awake() {
		TurnOffPanel(outfitPanel);
		outfitButton.onClick.AddListener(()=>{TogglePanel(outfitPanel);});
	}

	private void TogglePanel(GameObject panel) { 
		panel.SetActive(!panel.activeSelf);
	}

	private void TurnOffPanel(GameObject panel) { 
		panel.SetActive(false);
	}

	// Publics
	public void SetHostIP(string ip) {
		serverIPText.text = ip;
	}
	public void SetPlayerCounts(int heroCount, int villainCount) {
		heroCountText.text = heroCount.ToString();
		villainCountText.text = villainCount.ToString();
	}

	public void HideOutfitPanel() {
		TurnOffPanel(outfitPanel);
	}

	// Send info back to panels from lobby maanager if needed
	public void SetMapCountValues(int metroCount, int borgCount, int candyCount, int desertCount){
		mapPanelController.SetMapVoteCountValues(metroCount, borgCount, candyCount, desertCount);
	}

	public void SetGameCountValues(int arena, int ctf, int zone, int superior){
		gameSelectController.SetGameVoteCountValues(arena, ctf, zone, superior);
	}

}
