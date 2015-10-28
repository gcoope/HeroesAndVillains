using UnityEngine;
using System.Collections;

public class Settings : MonoBehaviour {

	// Player
	public static int BaseHealth = 100;
	public static int BaseArmour = 0;
	public static int BaseDamage = 10;
	public static int BaseMeleeDamage = 15;
	public static float BaseMoveSpeed = 30f;
	public static float BaseJumpHeight = 15f;

	// Physics
    public static float Gravity = -15f;

	// Networking
	public static string HostIP = "localhost";

}
