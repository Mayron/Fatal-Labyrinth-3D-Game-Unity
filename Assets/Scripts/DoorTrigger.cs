using UnityEngine;
using System.Collections;

public class DoorTrigger : MonoBehaviour {

	private GameObject doorPivot;
	public bool forward;
	
	void Start() {
		doorPivot = transform.parent.gameObject;
	}

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag == "Player" || col.gameObject.tag == "Enemy") {
			doorPivot.GetComponent<Doors>().SetForward(forward);
		}
	}
}
