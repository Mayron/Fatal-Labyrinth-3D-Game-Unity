using UnityEngine;
using System.Collections;

public class GateKeeperCombat : MonoBehaviour {

	GateKeeperController controller;
	
	public int attackCooldown = 180;	

	// Needed for equip Animation event!
	public Transform sword;
	public Transform shield;
	public Transform rightHand;
	public Transform leftHand;

	int timeSinceLastAttack = 0;
	bool combatEnabled;
	bool equipped;
	bool isAttacking;
	float currentHealth;
	Animator anim;
	PlayerController player;
	NavMeshAgent agent;
	
	void Awake () {
		controller = GetComponent<GateKeeperController> ();
		anim = GetComponent<Animator> ();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
		agent = GetComponent<NavMeshAgent> ();
	}

	void Start() {
		currentHealth = (float) Statistics.GetValue ("GateKeeper", "health");
		combatEnabled = false;
		equipped = false;
		isAttacking = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!combatEnabled)
			return;
		if (!equipped) {
			anim.SetBool ("Combat", true);
			anim.SetTrigger ("Equip");
			GetComponent<AudioSource>().Play();
			equipped = true;
		} else if (!anim.GetCurrentAnimatorStateInfo (0).IsName ("Equip")) {
			timeSinceLastAttack++;
			if (timeSinceLastAttack > attackCooldown){
				if (!controller.IsPlayerInSight ()) {
					FacePlayer();
				} else {
					timeSinceLastAttack = 0;
					int attackID = Random.Range (0, 2);
					if (attackID == 0) {
						anim.SetTrigger("Attack1");
					} else {
						anim.SetTrigger("Attack2");
					}
					sword.gameObject.GetComponent<AudioSource>().Play();
				}
			}
		}
	}

	public void SetAttacking(bool isAttacking) {
		this.isAttacking = isAttacking;
	}

	public void PlayAnimation(string animation) {
		anim.SetTrigger (animation);
	}

	public bool IsCombatEnabled() {
		return this.combatEnabled;
	}

	public bool IsAttacking() {
		return this.isAttacking;
	}

	public void EnableCombat(bool enabled) {
		anim.SetBool ("Run", false);
		anim.SetBool ("Walk", false);
		anim.SetBool ("CombatWalk", false);
		agent.speed = 0;
		agent.acceleration = 200;
		agent.Stop ();
		this.combatEnabled = enabled;
		if (!enabled) {
			anim.SetBool ("Combat", false);
		}
	}

	public void FacePlayer() {
		controller.StartMoving ("CombatWalk");
		transform.LookAt (player.transform.position);
	}

	public void equip() {
		sword.parent = rightHand;
		sword.position = rightHand.position;
		sword.rotation = rightHand.rotation;
		shield.parent = leftHand;
		shield.position = leftHand.position;
		shield.rotation = leftHand.rotation;
	}

	public void Die() {
		combatEnabled = false;
		controller.Die ();
		PlayAnimation ("Death");
		Camera.main.GetComponent<LootSpawner> ().SpawnLoot ("GateKeeper", transform.position);
		GameObject portalRoom = GameObject.FindGameObjectWithTag ("PortalRoom");
		portalRoom.GetComponent<GateController> ().Open ();
	}

	// TRIGGER EVENTS:
	
	void OnTriggerEnter(Collider other) {
		OnTriggerStay (other);
	}
	
	void OnTriggerStay(Collider other) {
		if (!controller.IsAlive ())
			return;
		if (player.IsAttacking() && other.gameObject.tag == "PlayerSword") {
			if (!anim.GetCurrentAnimatorStateInfo (0).IsName ("Knockback")) {			
				player.SetAttacking(false);
				float damage = (float) Statistics.CalculateDamage ("Player", "GateKeeper");
				currentHealth -= damage;
				if (currentHealth <= 0)
					Die ();
				else
					PlayAnimation("Knockback");
			}
		}
	}

}
