using UnityEngine;
using System.Collections;

public class BasePlayerInfo : MonoBehaviour {

	// Should we JSON this up?

	public enum Team {
		ATTACKER,
		DEFENDER
	}

	public float health = Settings.BaseHealth; // Variable per class? Maybe have a stat setup at the start
	public float armor = Settings.BaseArmour;  // as above
	public float damage = Settings.BaseDamage; // How much they can dish out
	public float speed = Settings.BaseMoveSpeed; // Move speed

	// Unique
	public float visibilityLevel; // If they're how well hidden - maybe enum this for effects such as wet/covered in paint etc.
	public bool cloaked; // Invisible / cloaked?
	public float noiseLevel; // Amount of noise they're making

	void Start () {
	
	}
}
