using UnityEngine;
using System.Collections;
using smoothstudio.heroesandvillains.player.events;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerNameFaceLocalPlayer : MonoBehaviour {

	private Transform cameraToLookAt;
	[SerializeField] private Text textComponent;
	[SerializeField] private CanvasGroup canvGroup;

	void Awake() {
		gameObject.AddGlobalEventListener(UIEVent.GotLocalCamera, (EventObject evt)=>{ cameraToLookAt = (Transform)evt.Params[0]; });
	}

	void Start () {
		gameObject.DispatchGlobalEvent(UIEVent.RequestLocalCamera);
		if(textComponent == null) textComponent = gameObject.GetComponentInChildren<Text>();
		if(canvGroup == null) canvGroup = gameObject.GetComponentInChildren<CanvasGroup>();
	}

	void Update () {
		if(cameraToLookAt != null) {
			float distance = Vector3.Distance(cameraToLookAt.position, transform.position);
			if(distance < 20) {
				textComponent.enabled = true;
				canvGroup.alpha = 1 - (distance) / 20; // distance fading

				Vector3 v = cameraToLookAt.transform.position - transform.position;
//				v.x = transform.eulerAngles.x;
//				v.z = transform.eulerAngles.z;
				v.x = v.z = 0f;
				transform.LookAt(cameraToLookAt.transform.position - v);
				transform.Rotate(0,180,0);

			} else {
				textComponent.enabled = false;
			}
		}
	}

}
