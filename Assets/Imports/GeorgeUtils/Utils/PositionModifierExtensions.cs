using UnityEngine;
using System.Collections;

public static class PositionModifierExtensions {

	#region x position
	public static void SetX(this Transform t, float newX) {
		t.position = new Vector3(newX, t.position.y, t.position.z);
	}
	public static float GetX(this Transform t) {
		return t.position.x;
	}

	public static void SetLocalX(this Transform t, float newX) {
		t.localPosition = new Vector3(newX, t.localPosition.y, t.localPosition.z);
	}
	public static float GetLocalX(this Transform t) {
		return t.localPosition.x;
	}

	public static void SetAnchoredX(this RectTransform t, float newX) {
		t.anchoredPosition = new Vector2(newX, t.anchoredPosition.y);
	}
	public static float GetAnchoredX(this RectTransform t) {
		return t.anchoredPosition.x;
	}
	#endregion

	#region y position
	public static void SetY(this Transform t, float newY) {
		t.position = new Vector3(t.position.x, newY, t.position.z);
	}
	public static float GetY(this Transform t) {
		return t.position.y;
	}
	
	public static void SetLocalY(this Transform t, float newY) {
		t.localPosition = new Vector3(t.localPosition.x, newY, t.localPosition.z);
	}
	public static float GetLocalY(this Transform t) {
		return t.localPosition.y;
	}
	
	public static void SetAnchoredY(this RectTransform t, float newY) {
		t.anchoredPosition = new Vector2(t.anchoredPosition.x, newY);
	}
	public static float GetAnchoredY(this RectTransform t) {
		return t.anchoredPosition.y;
	}
	#endregion


	#region get components
	public static RectTransform GetRectTransform(this GameObject g) {
		RectTransform rt = g.GetComponent<RectTransform>();
		return rt ?? null;
	}
	#endregion
}
