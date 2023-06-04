using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Statistics : MonoBehaviour {

	public static Dictionary<string, Dictionary<string, object>> creatures;

	//static int difficultyIndex;
	
	void Awake() {
//		difficultyIndex = 0;
		AddSettings ();
	}

	static void AddSettings() {
		creatures = new Dictionary<string, Dictionary<string, object>>();
		
		Dictionary<string, object> skeleton = new Dictionary<string, object> ();
		skeleton.Add ("health", 90f);
		skeleton.Add ("damage", 10);
		skeleton.Add ("critDamage", 20);
		skeleton.Add ("critChance", 0.2f);
		skeleton.Add ("parryAmount", 0.2f);
		skeleton.Add ("parryChance", 0.1f);
		skeleton.Add("dodgeChance", 0.1f);
		
		Dictionary<string, object> gateKeeper = new Dictionary<string, object> ();
		gateKeeper.Add ("health", 400f);
		gateKeeper.Add ("damage", 100);
		gateKeeper.Add ("critDamage", 50);
		gateKeeper.Add ("critChance", 0.3f);
		gateKeeper.Add ("parryAmount", 0.5f);
		gateKeeper.Add ("parryChance", 0.2f);
		gateKeeper.Add("dodgeChance", 0.2f);
		
		Dictionary<string, object> player = new Dictionary<string, object> ();
		player.Add ("health", 160f);
		player.Add ("damage", 40);
		player.Add ("critDamage", 60);
		player.Add ("critChance", 0.4f);
		player.Add ("parryAmount", 0.4f);
		player.Add ("parryChance", 0.2f);
		player.Add ("dodgeChance", 0.3f);
		player.Add ("xpNeeded", 200f);
		
		Dictionary<string, object> healthPickUp = new Dictionary<string, object> ();
		healthPickUp.Add ("value", 5f);
		healthPickUp.Add ("maxDropped_Skeleton", 3);
		healthPickUp.Add ("minDropped_Skeleton", 1);
		healthPickUp.Add ("maxDropped_GateKeeper", 8);
		healthPickUp.Add ("minDropped_GateKeeper", 5);
		
		Dictionary<string, object> xpPickUp = new Dictionary<string, object> ();
		xpPickUp.Add ("value", 15f);
		xpPickUp.Add ("maxDropped_Skeleton", 5);
		xpPickUp.Add ("minDropped_Skeleton", 2);
		xpPickUp.Add ("maxDropped_GateKeeper", 20);
		xpPickUp.Add ("minDropped_GateKeeper", 14);
		
		creatures.Add ("Skeleton", skeleton);
		creatures.Add ("GateKeeper", gateKeeper);
		creatures.Add ("Player", player);
		creatures.Add ("HealthPickUp", healthPickUp);
		creatures.Add ("XPPickUp", xpPickUp);
	}

	public static void PlayerLevelUp(int level) {
		Dictionary<string, object> player = creatures ["Player"];

		player["health"] = ((float) player["health"]) + (level * 10f);
		player["damage"] = ((int) player["damage"]) + (level * 5);
		player["critDamage"] = ((int) player["critDamage"]) + 5;
		player["xpNeeded"] = ((float) player["xpNeeded"]) + 10f;

		Dictionary<string, object> gateKeeper = creatures ["GateKeeper"];
		gateKeeper["health"] = ((float) player["health"]) - (level * 10f);
		gateKeeper["damage"] = ((int) player["damage"]) - (level * 5);
		gateKeeper["critDamage"] = ((int) player["critDamage"]) - 5;
	}

	public static int CalculateDamage(string source, string target) {
		Dictionary<string, object> sourceData;
		Dictionary<string, object> targetData;
		if (creatures.TryGetValue (source, out sourceData) && creatures.TryGetValue (target, out targetData)) {

			int damage = (int) sourceData["damage"];
			float critChance = (float) sourceData["critChance"];
			float parryChance = (float) targetData["parryChance"];

			float chance = Random.Range (0.0f, 1.0f);
			if (critChance >= chance)
				damage = (int) sourceData["critDamage"];
			chance = Random.Range (0.0f, 1.0f); 
			if (parryChance >= chance) {
				damage = (int) (((float) damage) * (1.0f - ((float) targetData["parryAmount"])));
			}
			return damage;
		}
		return 0;
	}

	public static object GetValue(string creature, string property) {
		Dictionary<string, object> data;
		if (creatures.TryGetValue (creature, out data)) {
			return data[property];
		}
		return null;
	}

	public static void ChangeDifficulty(int id) {
		//difficultyIndex = id;
	}
}
