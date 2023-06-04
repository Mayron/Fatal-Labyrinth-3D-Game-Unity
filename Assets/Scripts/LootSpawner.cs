using UnityEngine;
using System.Collections;

public class LootSpawner : MonoBehaviour {

	public GameObject healthPickUp;
	public GameObject xPPickUp;

	public int pickUpRadius = 20;

	public void SpawnLoot(string creature, Vector3 origin) {
		string[] names = new string[]{"HealthPickUp", "XPPickUp"};
		foreach (string pickUpName in names) {
			int minPickUps = (int) Statistics.GetValue (pickUpName, "minDropped_" + creature);
			int maxPickUps = (int) Statistics.GetValue (pickUpName, "maxDropped_" + creature);
			int amount = Random.Range (minPickUps, maxPickUps);
			
			for (int i = minPickUps; i <= amount; i++) {
				Vector3 distance = new Vector3(Random.Range (-pickUpRadius, pickUpRadius), 0f, 0f);
				Vector3 position = origin + distance;
				GameObject pickUp;
				if (pickUpName == "HealthPickUp")
					pickUp = Instantiate(healthPickUp);
				else
					pickUp = Instantiate(xPPickUp);
				pickUp.transform.position = new Vector3(position.x, pickUp.transform.position.y, position.z);
				pickUp.transform.RotateAround (origin, Vector3.up, Random.Range (0f, 360f));
			}
		}
	}
}
