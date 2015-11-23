using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.Networking;

public class LineDrawer : NetworkBehaviour {

	private LineRenderer line;
	private float lineShowDuration = 0.3f;
	private Color fadeCol;

	[SyncVar]
	private Vector3 startPos;
	[SyncVar]
	private Vector3 endPos;

	void Awake() {
		line = GetComponent<LineRenderer>();
	}

	void Start() {
		DrawLine();
		StartCoroutine("DestroySelf"); // Was an issue with destroying after fade tween - it caused null reference for the gameObject..
	}

	public void Setup(Vector3 start, Vector3 end) {
		this.startPos = start;
		this.endPos = end;
	}

	public void LocalDrawLine(Vector3 start, Vector3 end) {
		this.startPos = start;
		this.endPos = end;
		DrawLine();
	}
	public void DrawLine() {
		if(line == null) line = GetComponent<LineRenderer>();
		line.enabled = true;
		fadeCol = new Color(1,1,1,1);

		line.SetColors(fadeCol, fadeCol);
		line.SetPosition(0, startPos);
		line.SetPosition(1, endPos);

		DOTween.To(()=> fadeCol, x => fadeCol = x, new Color(1,1,1,0), lineShowDuration).OnUpdate(OnLineFadeUpdate);
	}

	IEnumerator DestroySelf() {
		yield return new WaitForSeconds(1f);
		NetworkServer.Destroy(this.gameObject);
	}

	private void OnLineFadeUpdate() {
		if(line != null) {
			line.SetColors(fadeCol, fadeCol);	
		}
	}

}
