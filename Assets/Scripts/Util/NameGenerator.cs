using System;
using UnityEngine;

public class NameGenerator
{
	static string[] names = {
		"Penguin Horse",
		"Reject",
		"Bad Player",
		"Lion",
		"The Sad Panda",
		"Power Ability",
		"Commander Evolution",
		"The Great Gumbo",
		"Doctor Face",
		"Sentry Woman",
		"The Lone Coffin",
		"King Lunch",
		"Professor Engine",
		"The Black Snake",
		"Scepter Bear",
		"Vanguard-Woman",
		"Lord Bash",
		"The False Cow",
		"Hippopotamus Woman",
		"Ability Girl",
		"Pyre Scepter",
		"Professor Union",
		"Dragon-Lad",
		"Master Water",
		"The True Thunder",
		"Monotonyman",
		"Master Man",
		"Octopus Kid",
		"Captain Fury",
		"Shield-Man",
		"Rat Man",
		"Parrot Boy",
		"Thunder Master",
		"Captain Earnestness",
		"King Liability",
		"Super Gossip",
		"The Fox",
		"Professor Crocodile"
	};

	public static string GetRandomName() {
		return names[UnityEngine.Random.Range(0, names.Length)];
	}
}

