using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.Networking;

public class LocalLineDrawer : MonoBehaviour {

	private LineRenderer line;
	private float lineShowDuration = 0.3f;
	private Color fadeCol;
	private Color teamSpecificColour;

	[SerializeField] private Color heroColour;
	[SerializeField] private Color villainColour;

	void Awake() {
		line = GetComponent<LineRenderer>();
	}

	public void DrawLine(Vector3 start, Vector3 end, string teamSide) {
		teamSpecificColour = teamSide == Settings.HeroTeam ? heroColour : villainColour;

		if(line == null) line = GetComponent<LineRenderer>();
		line.enabled = true;
		fadeCol = new Color(teamSpecificColour.r, teamSpecificColour.g, teamSpecificColour.b, 1);

		line.SetColors(fadeCol, fadeCol);
		line.SetPosition(0, start);
		line.SetPosition(1, end);

		DOTween.To(()=> fadeCol, x => fadeCol = x, new Color(teamSpecificColour.r, teamSpecificColour.g, teamSpecificColour.b,0), lineShowDuration).OnUpdate(OnLineFadeUpdate).OnComplete(DestroySelf);
	}

	private void DestroySelf() {
		ObjectPooler.instance.RecycleToPool(gameObject);
	}

	private void OnLineFadeUpdate() {
		if(line != null) {
			line.SetColors(fadeCol, fadeCol);	
		}
	}

}
