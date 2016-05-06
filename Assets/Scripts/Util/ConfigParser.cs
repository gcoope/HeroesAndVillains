using UnityEngine;
using System.Collections;
using System.IO;

public class ConfigParser : MonoBehaviour {

	void Awake() {
		StreamReader reader;
		string path = Application.streamingAssetsPath + "/config.txt";
		if(File.Exists(path)) {			
			reader = File.OpenText(path);
			ParseConfig(reader.ReadToEnd());
		}

	}

	void ParseConfig(string config) {
		string[] lines = config.Split('\n');
		for(int i = 0; i < lines.Length; i++) {
			string line = lines[i];
			// Player
			if(line.StartsWith("[BaseHealth]")) Settings.BaseHealth = int.Parse(line.Replace("[BaseHealth]", ""));
			if(line.StartsWith("[BaseJumpHeight]")) Settings.BaseJumpHeight = float.Parse(line.Replace("[BaseJumpHeight]", ""));

			// Game settings
			if(line.StartsWith("[ScorePerKill]")) Settings.ScorePerKill = int.Parse(line.Replace("[ScorePerKill]", ""));
			if(line.StartsWith("[TDMWinScore]")) Settings.TDMWinScore = int.Parse(line.Replace("[TDMWinScore]", ""));

			// Physics
			if(line.StartsWith("[Gravity]")) Settings.Gravity = int.Parse(line.Replace("[Gravity]", ""));

			// Powerups
			if(line.StartsWith("[PowerupRespawnTime]")) Settings.PowerupRespawnTime = float.Parse(line.Replace("[PowerupRespawnTime]", ""));
			if(line.StartsWith("[RapidFireRespawnTime]")) Settings.RapidFireRespawnTime = float.Parse(line.Replace("[RapidFireRespawnTime]", ""));
			if(line.StartsWith("[HealthPackRespawnTime]")) Settings.HealthPackRespawnTime = float.Parse(line.Replace("[HealthPackRespawnTime]", ""));
		}
	}

}
