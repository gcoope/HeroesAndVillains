using UnityEngine;
using System.Collections;

public class Settings : MonoBehaviour {

	// Player
	public static int BaseHealth = 100;
	public static int BaseArmour = 0;
	public static int BaseDamage = 10;
	public static int BaseMeleeDamage = 15;
	public static float BaseMoveSpeed = 15f;
	public static float BaseJumpHeight = 15f;
	public static bool DoubleJumpEnabled = false;

	// Game settings
	public static bool FirstPersonMode = true;

	// Physics
    public static float Gravity = -15f;

	// Networking
	public static string HostIP = "localhost";

	// Teams
	public static string HeroTeam = "Settings.HeroTeam";
	public static string VillainTeam = "Settings.VillainTeam";

}
