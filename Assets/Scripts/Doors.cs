using UnityEngine;
using System.Collections;

public class Doors : MonoBehaviour {

	public float smoothing = 2f;
	public float changeDirection = -90f;
	private bool animate;
	private bool open; // whether the door is open already
	private int doorTriggeredID = 0; // which door triggered it

	void Start() {
		animate = false;
		open = false;
	}

	void Update() {
		if (animate) {
			Quaternion openRotation;
			if (open) {
				openRotation = Quaternion.Euler (0f, 0f, 0f);
				transform.localRotation = Quaternion.Slerp (transform.localRotation, openRotation, smoothing * Time.deltaTime);
			} else {
				openRotation = Quaternion.Euler (0f, changeDirection, 0f);
				transform.localRotation = Quaternion.Slerp (transform.localRotation, openRotation, smoothing * Time.deltaTime);
			}

			float angle = Quaternion.Angle (transform.rotation, openRotation);
			if (angle < 1) {
				animate = false;
				open = !open;
				if (!open) doorTriggeredID = 0;
			}
		}
	}

	public void SetForward(bool forward) {
		if ((forward && doorTriggeredID == 2) || (!forward && doorTriggeredID == 1))
			return;

		changeDirection = Mathf.Abs(changeDirection);
		if (!forward) {
			changeDirection = -changeDirection;
			doorTriggeredID = 1;
		} else {
			doorTriggeredID = 2;
		}
		animate = true;
	}
}
