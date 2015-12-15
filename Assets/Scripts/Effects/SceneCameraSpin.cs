using UnityEngine;
using System.Collections;

public class SceneCameraSpin : MonoBehaviour {

	private Transform pivot;
	private Camera cam;

	void Start() {
		pivot = transform.parent;
		cam = GetComponent<Camera>();
	}

	void Update() {
		if(cam.enabled) {
			pivot.Rotate(new Vector3(0,-1,0) * Time.deltaTime * 6f);
		}
	}

}
