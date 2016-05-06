using UnityEngine;

public class Settings : MonoBehaviour {

	// Player
	public static int BaseHealth = 100;
	public static int OverShield = 0;
	public static int BaseDamage = 10;
	public static int BaseMeleeDamage = 15;
	public static float BaseMoveSpeed = 15f;
	public static float BaseJumpHeight = 15f;
	public static bool DoubleJumpEnabled = false;
	public static float RocketJumpPower = 15f;

	// Game settings
	public static bool FirstPersonMode = true;
	public static SettingsGameMode currentGameMode = SettingsGameMode.TEAM_DEATHMATCH;
	public static bool TDMGameMode = true;
	public static int ScorePerKill = 1;
	public static int TDMWinScore = 20;
	public static int timeBeforeNextRound = 5;

	// Physics
    public static float Gravity = 16f;

	// Networking
	public static string HostIP = "localhost";

	// Teams
	public static string HeroTeam = "Settings.HeroTeam";
	public static string VillainTeam = "Settings.VillainTeam";

	// Pickups
	public static float PowerupRespawnTime = 30f;
	public static float RapidFireRespawnTime = 30f;
	public static float HealthPackRespawnTime = 15f;
	public static int HealthPackHealAmount = 50;

	public static float MoveSpeedPowerupDuration = 8f;
	public static float MoveSpeedPowerupSpeed = 20f;

	public static float RapidFirePowerupDuration = 8f;
	public static float RapidFireCooldownSpeed = 0.15f;

}

public enum SettingsGameMode {
	TEAM_DEATHMATCH,
	CAPTURE_THE_FLAG,
	CONTROL_POINTS,
	PAYLOAD
}
