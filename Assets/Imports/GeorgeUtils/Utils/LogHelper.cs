using System;
using System.Collections.Generic;
using UnityEngine;

/*
 Filename:    LogHelper.cs
 Description: Helper class for logging out variables within Unity Editor.
 Author:      @georgecoope
 Initial Implementation:  06/02/2015
*/

public class LogHelper {

	public enum LogColours {
		RED,
		GREEN,
		BLUE,
		CYAN,
		BLACK,
		BROWN,
		DARKBLUE,
		FUCHSIAfuchsia,	
		GREY,
		LIGHTBLUE,
		LIME,
		MAGENTA,
		MAROON,
		NAVY,
		OLIVE,
		ORANGE,
		PURPLE,
		SILVER,
		TEAL,
		WHITE,
		YELLOW
	}

	/// <summary>
	/// Log the values passed in.
	/// </summary>
	/// <param name="Values">Values to log.</param>
	public static void Log(params object[] Values) {
		if(Values != null) {
			string outputLog = "";		
			foreach(object log in Values) {
				if(log != null)	outputLog += ((log.GetType()).Name + ": " + log.ToString() + "\n");
			}		
			Debug.Log(outputLog);
		}
		else Debug.LogWarning("[LogHelper] Error when logging. Check all values passed are not null");
	}

	/// <summary>
	/// Log the values and sender object. Use 'this' keyword for the current object.
	/// </summary>
	/// <param name="senderObject">Object log is being called from.</param>
	/// <param name="Values">Values to log.</param>
	public static void Log(object senderObject, params object[] Values) {
		if(senderObject != null && Values != null) {
			Type senderName = senderObject.GetType();
			string outputLog = "";
			foreach(object log in Values) {
				if(log != null)	outputLog += ((log.GetType()).Name + ": " + log.ToString() + "\n");
			}
			Debug.Log("Log from: [" + senderName.ToString() + "]\n" + outputLog);
		}
		else Debug.LogWarning("[LogHelper] Error when logging. Check all values passed are not null");
	}

	/// <summary>
	/// Log the value passed in using the specified colour.
	/// </summary>
	/// <param name="Colour">colour log will be.</param>
	/// <param name="Values">Values to log.</param>
	public static void LogColour(LogColours Colour, object Value) {
		if(Value != null) {
			string logColour = GetTextColour(Colour);
			string outputLog = "";		
			outputLog += ((Value.GetType()).Name + ": " + Value.ToString() + "\n");
			Debug.Log("<color=" + logColour + ">" + outputLog + "</color>");
		}
		else Debug.LogWarning("[LogHelper] Error when logging. Check all values passed are not null");
	}

	/// <summary>
	/// Log the values passed in using the specified colour. Use 'this' keyword for the current object.
	/// </summary>
	/// <param name="senderObject">Object log is being called from.</param>
	/// <param name="Colour">colour log will be.</param>
	/// <param name="Values">Values to log.</param>
	public static void LogColour(object senderObject, LogColours Colour, object Value) {
		if(senderObject != null && Value != null) {
			Type senderName = senderObject.GetType();
			string logColour = GetTextColour(Colour);
			string outputLog = "";		
			outputLog += ((Value.GetType()).Name + ": " + Value.ToString() + "\n");
			Debug.Log("<color=" + logColour + ">Log from: [" + senderName.ToString() + "]</color>\n" + outputLog );
		}
		else Debug.LogWarning("[LogHelper] Error when logging. Check all values passed are not null");
	}

	/// <summary>
	/// Logs the list.
	/// </summary>
	/// <param name="listToLog">List to log.</param>
	public static void LogList<T>(List<T> listToLog) {
		if(listToLog != null) {
			Type listType = listToLog[0].GetType();
			string outputLog = "";
			foreach(object log in listToLog) {
				if(log != null)	outputLog += (log.ToString() + "\n");
			}
			Debug.Log("List log (" +listType.Name + "):\n" + outputLog);
		}
		else Debug.LogWarning("[LogHelper] Error when logging. Check all values passed are not null");
		
	}

	/// <summary>
	/// Log the list and sender object. Use 'this' keyword for the current object.
	/// </summary>
	/// <param name="senderObject">Object log is being called from.</param>
	/// <param name="listToLog">List to log.</param>
	public static void LogList<T>(object senderObject, List<T> listToLog) {
		if(senderObject != null && listToLog != null) {
			Type senderName = senderObject.GetType();
			Type listType = listToLog[0].GetType();
			string outputLog = "";
			foreach(object log in listToLog) {
				if(log != null)	outputLog += (log.ToString() + "\n");
			}
			Debug.Log("Log from: [" + senderName.ToString() + "]\n List log (" +listType.Name + "):\n" + outputLog);
		}
		else Debug.LogWarning("[LogHelper] Error when logging. Check all values passed are not null");
		
	}

	/// <summary>
	/// Logs the array.
	/// </summary>
	/// <param name="arrayToLog">Array to log.</param>
	public static void LogArray<T>(T[] arrayToLog) {
		if(arrayToLog != null) {
			Type arrayType = arrayToLog.GetType();
			string outputLog = "";		
			foreach(object log in arrayToLog) {
				if(log != null)	outputLog += (log.ToString() + "\n");
			}		
			Debug.Log("Array log (" + arrayType.Name + "):\n" + outputLog);
		}
		else Debug.LogWarning("[LogHelper] Error when logging. Check all values passed are not null");		
	}

	/// <summary>
	/// Logs the array. Use 'this' keyword for the current object.
	/// </summary>
	/// <param name="senderObject">Object log is being called from.</param>
	/// <param name="arrayToLog">Array to log.</param>
	public static void LogArray<T>(object senderObject, T[] arrayToLog) {
		if(senderObject != null && arrayToLog != null) {
			Type senderName = senderObject.GetType();
			Type arrayType = arrayToLog.GetType();
			string outputLog = "";		
			foreach(object log in arrayToLog) {
				if(log != null)	outputLog += (log.ToString() + "\n");
			}		
			Debug.Log("Log from: [" + senderName.ToString() + "]\n Array log (" + arrayType.Name + "):\n" + outputLog);
		}
		else Debug.LogWarning("[LogHelper] Error when logging. Check all values passed are not null");
	}

	/// <summary>
	/// Logs the vector2.
	/// </summary>
	/// <param name="vector2ToLog">Vector2 to log.</param>
	public static void LogVector2(Vector2 vector2ToLog) {
		Debug.Log("x: " + vector2ToLog.x + ", y: " + vector2ToLog.y);	
	}

	/// <summary>
	/// Logs the vector2. Use 'this' keyword for the current object.
	/// </summary>
	/// <param name="senderObject">Sender object.</param>
	/// <param name="vector2ToLog">Vector2 to log.</param>
	public static void LogVector2(object senderObject, Vector2 vector2ToLog) {
		if(senderObject != null) {
			Type senderName = senderObject.GetType();
			Debug.Log("Log from: [" + senderName.ToString() + "]\n x: " + vector2ToLog.x + ", y: " + vector2ToLog.y);
		}
		else Debug.LogWarning("[LogHelper] Error when logging. Check all values passed are not null");
	}

	/// <summary>
	/// Logs the vector3.
	/// </summary>
	/// <param name="vector2ToLog">Vector2 to log.</param>
	public static void LogVector3(Vector3 vector3ToLog) {
		Debug.Log("x: " + vector3ToLog.x + ", y: " + vector3ToLog.y + ", z: " + vector3ToLog.z);
	}
	
	/// <summary>
	/// Logs the vector3. Use 'this' keyword for the current object.
	/// </summary>
	/// <param name="senderObject">Sender object.</param>
	/// <param name="vector2ToLog">Vector2 to log.</param>
	public static void LogVector3(object senderObject, Vector3 vector3ToLog) {
		if(senderObject != null) {
			Type senderName = senderObject.GetType();
			Debug.Log("Log from: [" + senderName.ToString() + "]\n x: " + vector3ToLog.x + ", y: " + vector3ToLog.y + ", z: " + vector3ToLog.z);
		}
		else Debug.LogWarning("[LogHelper] Error when logging. Check all values passed are not null");
	}

	private static string GetTextColour(LogColours col) {

		string returnString = col.ToString();
		returnString = returnString.Replace("LogHelper.LogColours.", "");
		return returnString.ToLower();

	}
}