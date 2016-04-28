using UnityEngine;

public class Settings : MonoBehaviour {

	// Player
	public const int BaseHealth = 100;
	public const int OverShield = 0;
	public const int BaseDamage = 10;
	public const int BaseMeleeDamage = 15;
	public const float BaseMoveSpeed = 15f;
	public const float BaseJumpHeight = 15f;
	public const bool DoubleJumpEnabled = false;
	public const float RocketJumpPower = 15f;

	// Game settings
	public const bool FirstPersonMode = true;
	public static SettingsGameMode currentGameMode = SettingsGameMode.TEAM_DEATHMATCH;
	public const bool TDMGameMode = true;
	public const int ScorePerKill = 1;
	public const int TDMWinScore = 50;
	public const int timeBeforeNextRound = 5;

	// Physics
    public const float Gravity = 16f;

	// Networking
	public const string HostIP = "localhost";

	// Teams
	public const string HeroTeam = "Settings.HeroTeam";
	public const string VillainTeam = "Settings.VillainTeam";

	// Powerup
	public static float PowerupRespawnTime = 15f;

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
