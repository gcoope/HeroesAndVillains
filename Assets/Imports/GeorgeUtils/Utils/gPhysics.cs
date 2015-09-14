using UnityEngine;
using System.Collections;

namespace GeorgeUtils
{
	public class gPhysics {

		#region Rigidbody
		/// <summary>
		/// Check for a rigidbody.
		/// </summary>
		/// <returns><c>true</c>, if rigidbody is found, <c>false</c> otherwise.</returns>
		/// <param name="g">The gameobject component.</param>
		public static bool hasRigidbody(GameObject g){
			return (g.GetComponent<Rigidbody>() != null);
		}
		/// <summary>
		/// Check for a rigidbody.
		/// </summary>
		/// <returns><c>true</c>, if rigidbody is found, <c>false</c> otherwise.</returns>
		/// <param name="g">The transform component.</param>
		public static bool hasRigidbody(Transform t){
			return (t.gameObject.GetComponent<Rigidbody>() != null);
		}
		/// <summary>
		/// Check for a rigidbody.
		/// </summary>
		/// <returns><c>true</c>, if rigidbody is found, <c>false</c> otherwise.</returns>
		/// <param name="g">The collider component.</param>
		public static bool hasRigidbody(Collider c){
			return (c.gameObject.GetComponent<Rigidbody>() != null);
		}
		/// <summary>
		/// Check for a rigidbody.
		/// </summary>
		/// <returns><c>true</c>, if rigidbody is found, <c>false</c> otherwise.</returns>
		/// <param name="g">The gameobject's name.</param>
		public static bool hasRigidbody(string g){
			GameObject go = GameObject.Find (g);
			if (go != null) {
				return (go.gameObject.GetComponent<Rigidbody>() != null);
			} else {
				Debug.Log("Could not find GameObject " + g);
				return false;
			}
		}
		#endregion
	
		#region Collision
		/// <summary>
		/// Returns name of colliding object.
		/// </summary>
		/// <returns>The collider.</returns>
		/// <param name="c">C.</param>
		public static string CollidingObjectName(Collider c){
			return c.gameObject.name;
		}
		/// <summary>
		/// Returns name of colliding object.
		/// </summary>
		/// <returns>The collision.</returns>
		/// <param name="c">C.</param>
		public static string CollidingObjectName(Collision c){
			return c.collider.gameObject.name;
		}

		/// <summary>
		/// Returns tag of colliding object.
		/// </summary>
		/// <returns>The collider.</returns>
		/// <param name="c">C.</param>
		public static string CollidingObjectTag(Collider c){
			return c.gameObject.tag;
		}
		/// <summary>
		/// Returns tag of colliding object.
		/// </summary>
		/// <returns>The collision.</returns>
		/// <param name="c">C.</param>
		public static string CollidingObjectTag(Collision c){
			return c.collider.gameObject.tag;
		}
		
		#endregion
	}
}
