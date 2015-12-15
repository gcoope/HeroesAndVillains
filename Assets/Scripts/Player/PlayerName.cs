using System;
using UnityEngine;

public class PlayerName : MonoBehaviour	{

	public TextMesh textMesh;
		
	public void SetName(string name) {
		textMesh.text = name;
	}
}