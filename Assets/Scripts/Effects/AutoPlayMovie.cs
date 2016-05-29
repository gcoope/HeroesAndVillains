using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class AutoPlayMovie : MonoBehaviour {

	Renderer rend;
	MovieTexture mTexture;
	AudioSource audioSrc;

	bool vidStarted = false;
	bool vidEnded = false;

	float videoDuration;

	 void Awake() {
		rend = GetComponent<Renderer>();
		mTexture = (MovieTexture)rend.material.mainTexture;
		videoDuration = mTexture.duration;
		audioSrc = GetComponent<AudioSource>();
		audioSrc.clip = mTexture.audioClip;
	}

	 void Start () 	{		
		if(!mTexture.isPlaying) {
			StartCoroutine("ChangeAtEnd");
			mTexture.Play();
			audioSrc.Play();
		}
	}

	void Update() {
		float quadHeight = Camera.main.orthographicSize * 2.0f;
		float quadWidth = quadHeight * Screen.width / Screen.height;
		transform.localScale = new Vector3(quadWidth, quadHeight, 1);

		if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape)) {
			StopCoroutine("ChangeAtEnd");
			GoToStartScene();
		}
	}

	IEnumerator ChangeAtEnd() {
		yield return new WaitForSeconds(videoDuration);
		GoToStartScene();
	}

	private void GoToStartScene() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

	}

}
