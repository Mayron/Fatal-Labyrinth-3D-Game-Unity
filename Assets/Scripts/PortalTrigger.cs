using UnityEngine;
using System.Collections;

public class PortalTrigger : MonoBehaviour {

	void OnTriggerEnter(Collider col) {
		if (col.tag == "Player") {
			if (Application.levelCount >= Application.loadedLevel + 1) {
				Application.LoadLevel(Application.loadedLevel + 1);
			} else {
				Application.LoadLevel(0);
			}
		}
	}
}
