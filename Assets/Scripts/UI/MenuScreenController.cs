using UnityEngine;
using System.Collections;
using smoothstudio.heroesandvillains.player.events;
using DG.Tweening;

public class MenuScreenController : MonoBehaviour {


	[SerializeField] private GameObject MainMenuCanvas;
	[SerializeField] private GameObject MultiplayerCanvas;

	[SerializeField] private CanvasGroup MainMenuButtonsGroup;
	[SerializeField] private CanvasGroup MainMenuOptions;
	private CanvasGroup mainMenuCanvasGroup;
	private CanvasGroup multiplayerCanvasGroup;

	private MainMenuCameraMove cameraMove;

	void Awake () {
		cameraMove = gameObject.GetComponent<MainMenuCameraMove>();

		mainMenuCanvasGroup = MainMenuCanvas.GetComponent<CanvasGroup>();
		multiplayerCanvasGroup = MultiplayerCanvas.GetComponent<CanvasGroup>();

		MainMenuCanvas.SetActive(true);
		MultiplayerCanvas.SetActive(false);

		MainMenuOptions.alpha = 0;
		MainMenuButtonsGroup.alpha = 1;
		MainMenuOptions.blocksRaycasts = false;
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

	public void ShowOptions() {
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
		Application.Quit();
	}
}
