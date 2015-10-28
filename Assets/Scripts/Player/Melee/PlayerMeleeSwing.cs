using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.Networking;
using smoothstudio.heroesandvillains.player.events;

public class PlayerMeleeSwing : NetworkBehaviour {

	private Vector3 swingStartEuler;
	private Vector3 swingEndEuler;
	private GameObject meleeCollider;
	public GameObject meleeColliderHolder;

	private BasePlayerInfo playerInfo;

	void Start () {
		playerInfo = gameObject.GetComponent<BasePlayerInfo> ();
		meleeCollider = meleeColliderHolder.transform.GetChild (0).gameObject; // Only ever one
		meleeCollider.SetActive (false);

		swingStartEuler = new Vector3 (0, 80, 0);
		swingEndEuler = new Vector3 (0, -80, 0);
	}
	

	void Update () {
		if (isLocalPlayer) {
			if (Input.GetKeyDown (KeyCode.M)) {
				CmdSwing ();
			}
		}
	}

	[Command]
	private void CmdSwing() {
		DOTween.Kill (meleeColliderHolder.transform);
		meleeColliderHolder.transform.localEulerAngles = swingStartEuler;
		meleeCollider.SetActive (true);
		meleeColliderHolder.transform.DOLocalRotate (swingEndEuler, 0.2f).OnComplete (()=>{
			meleeCollider.SetActive(false);
		});
	}

	public void MeleeSwingHit(Collider col) {
		if (col.CompareTag ("Player")) {
			BasePlayerInfo hitPlayerInfo = col.gameObject.GetComponent<BasePlayerInfo>();
			if(hitPlayerInfo.playerName != playerInfo.playerName) {
				gameObject.DispatchGlobalEvent(ProjectileEvent.MeleeHitPlayer, new object[] {hitPlayerInfo, playerInfo.meleeDamage});
			}
		}
	}

}
