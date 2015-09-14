using UnityEngine;
using System.Collections;

namespace GeorgeUtils
{
	public class gRotation : MonoBehaviour {
		/// <summary>
		/// Rotate gameobject by x, y and z.
		/// </summary>
		/// <param name="g">The gameobject.</param>
		/// <param name="x">The x amount.</param>
		/// <param name="y">The y amount.</param>
		/// <param name="z">The z amount.</param>
		public static void Rotate(GameObject g, float x, float y, float z){
			g.transform.Rotate (x, y, z);
		}
		/// <summary>
		/// Rotate gameobject by x, y and z.
		/// </summary>
		/// <param name="g">The gameobject.</param>
		/// <param name="x">The x amount.</param>
		/// <param name="y">The y amount.</param>
		/// <param name="z">The z amount.</param>
		public static void Rotate(Transform t, float x, float y, float z){
			t.Rotate (x, y, z);
		}
	}
}
