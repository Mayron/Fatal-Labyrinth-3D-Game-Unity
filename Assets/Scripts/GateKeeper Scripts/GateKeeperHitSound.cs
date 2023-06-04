using UnityEngine;
using System.Collections;

public class GateKeeperHitSound: MonoBehaviour {

	void OnTriggerEnter(Collider col) {
		if (col.tag == "PlayerSword") {
			GetComponent<AudioSource> ().Play ();
		}
	}
}
