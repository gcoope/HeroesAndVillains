using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public static class SaveOnPlay {

	static SaveOnPlay () {
		EditorApplication.playmodeStateChanged += OnPlaymodeStateChange;
	}

	private static void OnPlaymodeStateChange() {
		if(!EditorApplication.isPlaying && EditorApplication.isPlayingOrWillChangePlaymode) {
			EditorSceneManager.SaveOpenScenes();
			Debug.Log("Saving!");
		}
	}
}
