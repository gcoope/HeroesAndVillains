using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof (Text))]
public class BuildText : MonoBehaviour
{
	private Text textBox;

    void Start() {
		DontDestroyOnLoad(this.transform.parent);
		textBox = GetComponent<Text>();
		textBox.text = "Alpha build " +  Application.productName + " - " + DateTime.Today.Date.Day.ToString() + "/" + DateTime.Today.Month.ToString() + "/" + DateTime.Today.Year.ToString();
    }

}
