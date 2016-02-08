using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerName : NetworkBehaviour	{

	public Text textObj;
		
	public void SetName(string name) {
		textObj.text = name;
	}

	public void EnableText() { 
		if(!isLocalPlayer) textObj.gameObject.SetActive(true);
		else DisableText();
	}
	public void DisableText() { textObj.gameObject.SetActive(false); }
}