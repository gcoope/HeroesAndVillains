﻿using UnityEngine;
using System.Collections;

public class Settings : MonoBehaviour {

	// Player
	public const int BaseHealth = 100;
	public const int BaseArmour = 0;
	public const int BaseDamage = 10;
	public const int BaseMeleeDamage = 15;
	public const float BaseMoveSpeed = 15f;
	public const float BaseJumpHeight = 15f;
	public const bool DoubleJumpEnabled = false;
	public const float RocketJumpPower = 15f;

	// Game settings
	public const bool FirstPersonMode = true;
	public const int ScorePerKill = 10;

	// Physics
    public const float Gravity = -16f;

	// Networking
	public const string HostIP = "localhost";

	// Teams
	public const string HeroTeam = "Settings.HeroTeam";
	public const string VillainTeam = "Settings.VillainTeam";

}
