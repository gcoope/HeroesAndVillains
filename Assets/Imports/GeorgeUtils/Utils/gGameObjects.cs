using UnityEngine;
using System.Collections;

namespace GeorgeUtils
{
	public class gGameObjects : MonoBehaviour {

		#region Activating
		/// <summary>
		/// Enables the gameobject.
		/// </summary>
		/// <param name="g">The gameobject.</param>
		public static void EnableObject(GameObject g){
			g.SetActive (true);
		}
		/// <summary>
		/// Enables the transform's gameobject.
		/// </summary>
		/// <param name="g">The transform.</param>
		public static void EnableObject(Transform t){
			t.gameObject.SetActive (true);
		}
		/// <summary>
		/// Enables the collider's gameobject.
		/// </summary>
		/// <param name="g">The collider.</param>
		public static void EnableObject(Collider c){
			c.gameObject.SetActive (true);
		}
		/// <summary>
		/// Enables the gameobject.
		/// </summary>
		/// <param name="g">The gameobject's name.</param>
		public static void EnableObject(string obj){
			GameObject go = GameObject.Find (obj);
			if (go != null) {
				go.SetActive (true);
			} else {
				Debug.Log("Could not find object with name " + obj + " to enable");
			}
		}
		
		/// <summary>
		/// Enables all gameobjects by tag.
		/// </summary>
		/// <param name="tag">Tag.</param>
		public static void EnableAllTags(string tag){
			GameObject[] go = GameObject.FindGameObjectsWithTag (tag);
			if (go != null) {
				foreach (GameObject g in go) {
					g.SetActive (true);
				}
			} else {
				Debug.Log("Could not find any objects with tag " + tag + " to enable");
			}
		}
		#endregion
		
		#region Disactivating
		/// <summary>
		/// Disables the gameobject.
		/// </summary>
		/// <param name="g">The gameobject.</param>
		public static void DisableObject(GameObject g){
			g.SetActive (false);
		}
		/// <summary>
		/// Disables the transform's gameobject.
		/// </summary>
		/// <param name="g">The transform.</param>
		public static void DisableObject(Transform t){
			t.gameObject.SetActive (false);
		}
		/// <summary>
		/// Disables the collider's gameobject.
		/// </summary>
		/// <param name="g">The collider.</param>
		public static void DisableObject(Collider c){
			c.gameObject.SetActive (false);
		}
		/// <summary>
		/// Disables the gameobject.
		/// </summary>
		/// <param name="g">The gameobject's name.</param>
		public static void DisableObject(string obj){
			GameObject go = GameObject.Find (obj);
			if (go != null) {
				go.SetActive (false);
			} else {
				Debug.Log("Could not find object with name " + obj + " to enable");
			}
		}
		
		/// <summary>
		/// Disables all gameobjects by tag.
		/// </summary>
		/// <param name="tag">Tag.</param>
		public static void DisableAllTags(string tag){
			GameObject[] go = GameObject.FindGameObjectsWithTag (tag);
			if (go != null) {
				foreach (GameObject g in go) {
					g.SetActive (false);
				}
			} else {
				Debug.Log("Could not find any objects with tag " + tag + " to enable");
			}
		}

		public static void DisableAllButThis(GameObject g){
			Object[] gos = GameObject.FindObjectsOfType (typeof(GameObject));
			foreach (GameObject go in gos) {
				if(go.name != "AppManager" && go.name != g.name){
					go.SetActive(false);
				}
			}
		}
		#endregion

	}
}
