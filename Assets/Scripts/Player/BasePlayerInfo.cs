using UnityEngine;
using System.Collections;

public class BasePlayerInfo : MonoBehaviour {

	public enum Team {
		ATTACKER,
		DEFENDER
	}

	[Header("Stats")]
	public float health = Settings.BaseHealth; // Variable per class? Maybe have a stat setup at the start
	public float armor = Settings.BaseArmour;  // as above
	public float damage = Settings.BaseDamage;
	public float speed = Settings.BaseMoveSpeed;
	public float jumpHeight = Settings.BaseJumpHeight;

	[Header("Other")]
	public float visibilityLevel; // If they're how well hidden - maybe enum this for effects such as wet/covered in paint etc.
	public bool cloaked; // Invisible / cloaked?
	public float noiseLevel; // Amount of noise they're making - could affect stealth? (Note: this might be too much work)

	void Start () {
	
	}
}
