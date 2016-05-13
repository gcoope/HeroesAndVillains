using UnityEngine;
using System.Collections;
using smoothstudio.heroesandvillains.player.events;
using DG.Tweening;
using UnityEngine.UI;

public class MenuScreenController : MonoBehaviour {

	#pragma warning disable 169
	[SerializeField] private GameObject MainMenuCanvas;
	[SerializeField] private GameObject MultiplayerCanvas;
	[SerializeField] private GameObject LobbyCanvas;

	[SerializeField] private CanvasGroup MainMenuButtonsGroup;
	[SerializeField] private CanvasGroup MainMenuOptions;
	[SerializeField] private GameObject CreditsPanel;
	[SerializeField] private GameObject ActualOptions;
	[SerializeField] private GameObject QuitPanel;

	private CanvasGroup mainMenuCanvasGroup;
	private CanvasGroup multiplayerCanvasGroup;

	private MainMenuCameraMove cameraMove;

	[Header("Options Menu")]
	[SerializeField] private Slider sfxSlider;
	[SerializeField] private Text sfxSliderLabel;
	[SerializeField] private Slider musicSlider;
	[SerializeField] private Text musicSliderLabel;
	#pragma warning restore 169

	void Awake () {
		cameraMove = gameObject.GetComponent<MainMenuCameraMove>();

		mainMenuCanvasGroup = MainMenuCanvas.GetComponent<CanvasGroup>();
		multiplayerCanvasGroup = MultiplayerCanvas.GetComponent<CanvasGroup>();

		MainMenuCanvas.SetActive(true);
		MultiplayerCanvas.SetActive(false);
		LobbyCanvas.SetActive(false);
		QuitPanel.SetActive (false);

		MainMenuOptions.alpha = 0;
		MainMenuButtonsGroup.alpha = 1;
		MainMenuOptions.blocksRaycasts = false;

		HideCredits();

		// Options setup
		sfxSlider.onValueChanged.AddListener(SFXVolumeChange);
		musicSlider.onValueChanged.AddListener(MusicVolumeChange);

		// Playing menu loop here
		AudioKeys.MenuLoop1.PlayMusic();
	}

	void OnLevelWasLoaded() {
		sfxSlider.value = SoundManager.instance.SFXVolume;
		musicSlider.value = SoundManager.instance.MusicVolume;
		SFXVolumeChange(SoundManager.instance.SFXVolume);
		MusicVolumeChange(SoundManager.instance.MusicVolume);
	}

	public void ShowMultiplayerCanvas() {
		mainMenuCanvasGroup.DOFade(0, 0.25f).OnComplete(()=>{MainMenuCanvas.SetActive(false);});

		MultiplayerCanvas.SetActive(true);
		multiplayerCanvasGroup.alpha = 0;
		multiplayerCanvasGroup.DOFade(1, 0.25f);

		cameraMove.MoveToMultiplayerPosition();
	}

	public void ShowMainMenuCanvas() {
		BackFromOptions();

		multiplayerCanvasGroup.DOFade(0, 0.25f).OnComplete(()=>{MultiplayerCanvas.SetActive(false);});

		MainMenuCanvas.SetActive(true);
		mainMenuCanvasGroup.alpha = 0;
		mainMenuCanvasGroup.DOFade(1, 0.25f);

		cameraMove.MoveToStartPosition();
	}

	public void ShowLobbyCanvas() {
		MainMenuCanvas.SetActive(false);
		MultiplayerCanvas.SetActive(false);
		LobbyCanvas.SetActive(true);
	}

	public void HideLobbyCanvas() {
		LobbyCanvas.SetActive(false);
	}

	public void ShowMainMenuCanvasFromLobby() {
		mainMenuCanvasGroup.alpha = 1;
		MainMenuCanvas.SetActive(true);
		LobbyCanvas.SetActive(false);
		gameObject.DispatchGlobalEvent(MenuEvent.ExitLobbyButton);
	}

	public void ShowOptions() {
		AudioKeys.UIClick.PlaySound ();
		MainMenuOptions.DOFade(1, 0.25f);
		MainMenuOptions.blocksRaycasts = true;
		MainMenuButtonsGroup.DOFade(0, 0.25f);
		cameraMove.MoveToOptionsPosition();
	}

	public void BackFromOptions() {
		MainMenuOptions.DOFade(0, 0.25f);
		MainMenuOptions.blocksRaycasts = false;
		MainMenuButtonsGroup.DOFade(1, 0.25f);
		cameraMove.MoveToStartPosition();
	}

	public void Quit() {
		MainMenuButtonsGroup.alpha = 0;
		QuitPanel.SetActive (true);
	}

	public void ConfirmQuit() {
		Application.Quit ();
	}

	public void CancelQuit() {
		QuitPanel.SetActive (false);
		MainMenuButtonsGroup.alpha = 0;
		MainMenuButtonsGroup.DOFade (1, 0.25f);
	}

	// Options menu handlers
	public void SFXVolumeChange(float vol) {
		SoundManager.instance.SetSFXVolume(vol);
		sfxSliderLabel.text = Mathf.Round(vol*100) + "%";
		gameObject.DispatchGlobalEvent(MenuEvent.UpdateAudioVolumes);
	}

	public void MusicVolumeChange(float vol) {
		SoundManager.instance.SetMusicVolume(vol);
		musicSliderLabel.text = Mathf.Round(vol*100) + "%";
		gameObject.DispatchGlobalEvent(MenuEvent.UpdateAudioVolumes);
	}

	public void ShowCredits() {
		ActualOptions.SetActive (false);
		CreditsPanel.SetActive(true);
	}

	public void HideCredits() {
		ActualOptions.SetActive (true);
		CreditsPanel.SetActive(false);
	}
}
