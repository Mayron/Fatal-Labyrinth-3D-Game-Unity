using UnityEngine;
using System.Collections;

public class FloatingPickUp : MonoBehaviour {

	public float playerDetectionRadius = 20f;
	public float speed = 10f;

	GameObject player;
	PlayerController playerController;

	void Start() {
		player = GameObject.FindGameObjectWithTag ("Player");
		playerController = player.GetComponent<PlayerController>();
	}

	void Update() {
		transform.Rotate (0, 0, 50 * Time.deltaTime);
		float distance = Vector3.Distance (player.transform.position, transform.position);
		if (distance <= playerDetectionRadius) {
			float step = speed * Time.deltaTime;
			Vector3 stepVector = Vector3.MoveTowards(transform.position, player.transform.position, step);
			transform.position = new Vector3(stepVector.x, transform.position.y, stepVector.z); 
		}
	}

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag == "Player") {
			PickUp();
		}
	}

	void PickUp() {
		playerController.PickUp (gameObject.tag);
		Destroy (gameObject);
	}

}
