using System;
using UnityEngine;

public class EventButton : MonoBehaviour {

	public string eventToFire = "";

	public void Clicked() {
	 if(eventToFire != "") {
			gameObject.DispatchGlobalEvent(eventToFire);
		}
	}

}
