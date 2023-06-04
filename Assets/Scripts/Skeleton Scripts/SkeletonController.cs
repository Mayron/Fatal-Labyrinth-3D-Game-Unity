using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkeletonController : MonoBehaviour {

	// movement control properties:
	public float spawnRadius = 30f;
	public float minDistApart = 8f;
	public float evadeDistance = 120f;
	public float playerDetection = 40f;
	public float combatDistance = 10f;
	public int removeBodyTime = 400;

	int deadTime;
	bool isAlive;
	Vector3 evadePoint;
	bool evading;

	// Components:
	GameObject player;
	NavMeshAgent agent;
	Animator anim;
	SkeletonGroupController group;
	SkeletonCombat combat;

	void Awake() {
		agent = GetComponent<NavMeshAgent> ();
		anim = GetComponent<Animator> ();
		combat = GetComponent<SkeletonCombat> ();
	}

	void Start() {
		evading = false;
		deadTime = 0;
		player = GameObject.FindGameObjectWithTag ("Player");
	}

	void Update () {
		if (!player) // StartMenu
			return;
		if (!isAlive) {
			deadTime++;
			if (deadTime > removeBodyTime) {
				GameObject.Destroy(gameObject);
			}
			return;
		};
		Debug.DrawLine (group.origin.transform.position, transform.position, Color.red);

		float distanceToPlayer = Vector3.Distance (transform.position, player.transform.position);
		float distanceFromEvading = Vector3.Distance (transform.position, evadePoint);
		if (distanceToPlayer < playerDetection && distanceFromEvading < evadeDistance && !evading) {
			if (distanceToPlayer <= combatDistance) {
				StopMoving();
				if (!combat.IsCombatEnabled ())
					combat.EnableCombat (true);
			} else {
				if (!anim.GetCurrentAnimatorStateInfo (0).IsName ("Attack")) {
					anim.SetBool ("Run", true);
					agent.SetDestination (player.transform.position);
					agent.Resume ();
				}
				if (combat.IsCombatEnabled ())
					combat.EnableCombat (false);
			}
		} else {
			// evade!
			if (combat.IsCombatEnabled())
				combat.EnableCombat (false);
			if (distanceFromEvading > spawnRadius) {
				agent.SetDestination (evadePoint);
				agent.Resume ();
				if (!evading) 
					evading = true;
			} else {
				StopMoving();
				evading = false;
			}
		}
	}

	public void SetGroupController(SkeletonGroupController group) {
		this.group = group;
		PlaceSkeleton ();
		if (group.agents.Capacity > 0) {
			foreach (GameObject other in group.agents) {
				while (Vector3.Distance(transform.position, other.transform.position) < minDistApart) {
					PlaceSkeleton();
				}
			}
		}
		isAlive = true;
	}

	public void RemoveFromGroup() {
		if (group != null) {
			isAlive = false;
			group.DestroyAgent (this);
		}
	}

	public bool IsEvading() {
		return evading;
	}

	void StopMoving() {
		agent.Stop ();
		agent.acceleration = 200;
		anim.SetBool ("Run", false);
	}
	
	void PlaceSkeleton() {
		Vector3 distance = new Vector3(Random.Range (-spawnRadius, spawnRadius), 0f, 0f);
		Vector3 position = group.origin.transform.position + distance;
		transform.position = position;
		transform.RotateAround (group.origin.transform.position, Vector3.up, Random.Range (0f, 360f));
		evadePoint = transform.position;
	}
}
