using UnityEngine;
using System.Collections;

namespace GeorgeUtils
{
	public class gRaycast {
		/// <summary>
		/// Returns name of gameobject clicked on.
		/// </summary>
		/// <returns>The gameobject name.</returns>
		public static string RaycastColliderName(Camera camera){
			RaycastHit hit;
			Ray ray = camera.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit, 100f)) {
				return hit.collider.gameObject.name;
			} else {
				return "null";
			}
		}

		/// <summary>
		/// Raycasts down.
		/// </summary>
		/// <returns><c>true</c>, if raycast hits, <c>false</c> otherwise.</returns>
		/// <param name="distance">Distance.</param>
		/// <param name="gameobject">Gameobject.</param>
		public static bool RaycastDown(float distance, GameObject gameobject) {
			Vector3 down = new Vector3 (0, -1, 0);
			if (Physics.Raycast (gameobject.transform.position, down, distance)) {
				return true;
			} else {
				return false;
			}
		}


	}
}
