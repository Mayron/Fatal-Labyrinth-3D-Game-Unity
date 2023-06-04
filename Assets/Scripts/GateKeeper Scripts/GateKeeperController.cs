using UnityEngine;
using System.Collections;

public class GateKeeperController : MonoBehaviour {
	
	public float walkSpeed = 10f;
	public float runSpeed = 15f;
	public float combatWalkSpeed = 5f;
	public int removeBodyTime = 400;
	public float minChaseDistance = 80f; // the minimum distance towards the player before the enemy can chase
	public float maxChaseDistance = 10f; // how close the enemy can get to the player before entering combat
	public float combatDistance = 20f;
	public GameObject raySource; // source of ray casting on the model

	bool chasePlayer = false; // whether the enemy is chasing the player
	bool isAlive;
	int deadTime;
	
	Vector3 offset = new Vector3 (0.2f, 0, 0); // for positioning ray casts!

	// Components:
	GameObject player;
	Animator anim;
	GameObject dest; // current target destination to move to
	NavMeshAgent agent;	// controls the movement of the enemy on the map

	GateKeeperCombat combat;

	void Awake() {
		agent = gameObject.GetComponent<NavMeshAgent>();
		anim = GetComponent<Animator> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		combat = GetComponent<GateKeeperCombat> ();
		isAlive = true;
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
		if (IsPlayerInSight ()) { // enemy can see the player using raycasting
			SetDestination (player);
			chasePlayer = true;

			if (IsInRange (player, maxChaseDistance)) { // close enough to enter combat
				combat.EnableCombat (true);
			} else if (IsInRange (player, combatDistance)) { // close enough to enter combat
				StartMoving ("CombatWalk");
			} else {
				StartMoving ("Run");
			}
		} else if (IsInRange (player, minChaseDistance)) { // player is close by but cannot see them
			if (chasePlayer) { // enemy knows the player is close
				if (IsInRange (player, maxChaseDistance)) {
					combat.EnableCombat (true);
				} else {
					StartMoving ("Run");
				}
			} else {
				StartMoving ("Walk"); // is suspicious
			}
			SetDestination (player);
		} else { // enemy has lost the player = goes back to patrolling
			if (dest == null || dest.tag != "Spawner") {
				dest = GetRandomSpawnPoint();
			}
			if (IsInRange(dest, maxChaseDistance))  {
				dest = GetRandomSpawnPoint(dest);
			}
			SetDestination (dest);
			StartMoving("Walk");
		}
	}

	/* When the enemy is not chasing the player, it picks a random spawn point to move to
	 */
	GameObject GetRandomSpawnPoint() {
		GameObject[] spawners = GameObject.FindGameObjectsWithTag ("Spawner");
		float length = spawners.Length;
		int index = (int) Random.Range (0f, length);

		return spawners[index];
	}

	/* Gets a random spawn point that is not the current one
	 */
	GameObject GetRandomSpawnPoint(GameObject exclude) {
		GameObject spawner;
		do {
			spawner = GetRandomSpawnPoint ();
		} while (spawner == exclude);
		return spawner;
	}


	/* Uses the Nav agent to move to the selected location (spawn point or player)
	 */
	void SetDestination(GameObject dest) {		
		agent.SetDestination (dest.transform.position);
	}

	/* Returns whether in range of a target
	 */
	bool IsInRange(GameObject target, float range) {
		return (target.transform.position - transform.position).magnitude <= range;
	}

	/* Uses rays to scan for the player in front of hte enemy
	 */
	public bool IsPlayerInSight() {
		Ray forwardRay = new Ray (raySource.transform.position, transform.forward);
		Ray farLeftRay = new Ray (raySource.transform.position, transform.forward - offset);
		Ray farRightRay = new Ray (raySource.transform.position, transform.forward + offset);
		Ray leftRay = new Ray (raySource.transform.position, transform.forward - (offset / 2));
		Ray rightRay = new Ray (raySource.transform.position, transform.forward + (offset / 2));

		// Only visible with gizmo's turned on in play mode.
		Debug.DrawRay (raySource.transform.position, transform.forward * minChaseDistance);
		Debug.DrawRay (raySource.transform.position, (transform.forward - offset) * minChaseDistance);
		Debug.DrawRay (raySource.transform.position, (transform.forward + offset) * minChaseDistance);
		Debug.DrawRay (raySource.transform.position, (transform.forward - (offset / 2)) * minChaseDistance);
		Debug.DrawRay (raySource.transform.position, (transform.forward + (offset / 2)) * minChaseDistance);

		foreach (Ray ray in new Ray[]{forwardRay, farLeftRay, farRightRay, leftRay, rightRay}) {
			if (IsPlayerDetected(ray)) return true;
		}
		return false;
	}

	/* Returns whether the player has crossed one of the four enemy rays
	 */
	bool IsPlayerDetected(Ray ray) {
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, minChaseDistance)) {
			if (hit.collider.tag == "Player")
				return true;
		}
		return false;
	}

	public void Die() {
		this.isAlive = false;
	}

	public bool IsAlive() {
		return this.isAlive;
	}

	public void StartMoving(string movement) {
		anim.SetBool (movement, true);
		switch (movement) {
			case "Run":
			anim.SetBool ("Walk", false);
			anim.SetBool ("CombatWalk", false);
			agent.speed = runSpeed;
			break;
			case "Walk":
			anim.SetBool ("Run", false);
			anim.SetBool ("CombatWalk", false);
			agent.speed = walkSpeed;
			break;
			case "CombatWalk":
			anim.SetBool ("Run", false);
			anim.SetBool ("Walk", false);
			agent.speed = combatWalkSpeed;
			break;
		}
		agent.Resume ();
	}
}
