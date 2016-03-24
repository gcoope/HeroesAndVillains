using UnityEngine;
using System.Collections;
using smoothstudio.heroesandvillains.player.events;
using UnityEngine.Networking;

public class OnlineOrLanController : MonoBehaviour {

	public GameObject lanPanel;
	public GameObject onlinePanel;

	void Start () {
		ShowLAN();
	}

	public void ShowLAN() {
		gameObject.DispatchGlobalEvent(MenuEvent.StopMatchMaker);
		lanPanel.SetActive(true);
		onlinePanel.SetActive(false);
	}

	public void ShowOnline() {
		gameObject.DispatchGlobalEvent(MenuEvent.StartMatchMaker);
		lanPanel.SetActive(false);
		onlinePanel.SetActive(true);
	}
}
