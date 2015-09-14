using UnityEngine;
using System.Collections;

public class Settings : MonoBehaviour {

	// Player
	public static float BaseHealth = 100f;
	public static float BaseArmour = 0f;
	public static float BaseDamage = 25f;
	public static float BaseMoveSpeed = 10f;
	public static float BaseJumpHeight = 10f;

	// Physics
    public static float Gravity = -15f;

	// Networking
	public static string HostIP = "localhost";

}
