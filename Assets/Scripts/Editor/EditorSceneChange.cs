using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class EditorSceneChange {

	[MenuItem("Scenes/MainMenu #1")]
	private static void LoadMainMenu() {
		EditorSceneManager.SaveOpenScenes();
		EditorSceneManager.OpenScene("Assets/Scenes/MainMenu.unity");
	}

	[MenuItem("Scenes/World1 #2")]
	private static void LoadWorld1() {
		EditorSceneManager.SaveOpenScenes();
		EditorSceneManager.OpenScene("Assets/Scenes/World1.unity");
	}

	[MenuItem("Scenes/Run Menu #`")]
	private static void LoadAndRunMainMenu() {
		EditorSceneManager.SaveOpenScenes();
		EditorSceneManager.OpenScene("Assets/Scenes/MainMenu.unity");
		if(!EditorApplication.isPlaying && !EditorApplication.isCompiling) {
			EditorApplication.isPlaying = true;
		}
	}

}
