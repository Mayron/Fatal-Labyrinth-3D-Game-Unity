using UnityEngine;
using System.Collections;

public class GateController : MonoBehaviour {

	GameObject notificationText;
	bool showing = false;
	int disappearTimer = 450;
	int counter = 0;

	void Start() {
		notificationText = GameObject.FindGameObjectWithTag ("NotificationText");
		notificationText.SetActive (false);
	}

	public void Open() {
		notificationText.SetActive (true);
		showing = true;
		GameObject.Destroy (GameObject.FindGameObjectWithTag("Gate"));
	}

	void Update() {
		if (showing) {
			counter++;
			if (counter > disappearTimer) {
				showing = false;
				notificationText.SetActive (false);
			}

		}
	}

}
