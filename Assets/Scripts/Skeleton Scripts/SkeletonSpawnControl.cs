using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* This class controls the spawn rate for groups of skeletons on the map
 */
public class SkeletonSpawnControl : MonoBehaviour {

	public GameObject skeleton;
	public int maxActiveGroups = 4;
	public int spawnRadius = 20;
	public int groupSize = 5;

	List<SkeletonGroupController> activeGroups;

	int totalGroupsAlive = 0;

	// Use this for initialization
	void Start () {
		activeGroups = new List<SkeletonGroupController>();
	}
	
	// Update is called once per frame
	void Update () {	
		if (totalGroupsAlive < maxActiveGroups) {
			CreateGroup ();
		} else {
			SkeletonGroupController[] temp = new SkeletonGroupController[activeGroups.Capacity];
			activeGroups.CopyTo(temp);
			foreach (SkeletonGroupController gc in temp) {
				if (!gc.IsActive ()) {
					activeGroups.Remove (gc);
					totalGroupsAlive--;
				}
			}
		}
	}

	GameObject GetRandomSpawnPoint() {
		GameObject[] spawners = GameObject.FindGameObjectsWithTag ("Spawner");
		float length = spawners.Length;
		int index = (int) Random.Range (0f, length);		
		return spawners[index];
	}

	void CreateGroup() {
		GameObject spawnPoint;
		do {
			spawnPoint = GetRandomSpawnPoint ();
		} while (SpawnPointExists(spawnPoint));

		SkeletonGroupController gc = new SkeletonGroupController(spawnPoint);
		
		for (int i = 0; i < groupSize; i++) {
			float randomDirection = Random.Range (0f, 360f);
			gc.CreateAgent (randomDirection, skeleton);
		}
		activeGroups.Add (gc);
		totalGroupsAlive++;
	}

	bool SpawnPointExists(GameObject spawnPoint) {
		foreach (SkeletonGroupController gc in activeGroups) {
			if (spawnPoint == gc.origin) return true;
		}
		return false;
	}
}
