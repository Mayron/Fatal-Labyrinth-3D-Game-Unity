using UnityEngine;
using System.Collections;

public class SkeletonCombat : MonoBehaviour {
	
	public int attackCooldown = 60;
	
	int timeSinceLastAttack = 0;
	bool combatEnabled = false;
	bool isAttacking = false;
	float currentHealth;

	// Components:
	Animator anim;
	PlayerController player;
	SkeletonController controller;
	
	void Awake() {
		anim = GetComponent<Animator>();
		controller = GetComponent<SkeletonController>();
		GameObject playerObject = GameObject.FindGameObjectWithTag ("Player");
		if (playerObject != null) 
				player = playerObject.GetComponent<PlayerController>();
	}

	void Start() {
		currentHealth = (float) Statistics.GetValue ("Skeleton", "health");
	}
	
	void Update () {
		if (!combatEnabled)
			return;

		timeSinceLastAttack++;
		if (timeSinceLastAttack > attackCooldown) {
			PlayAnimation ("Attack");
			timeSinceLastAttack = 0;
		}
	}

	public bool IsCombatEnabled() {
		return combatEnabled;
	}
	
	public void PlayAnimation(string animation) {
		if (animation == "Death" || animation == "Damage") {
			player.SetAttacking(false);
		}
		this.SetAttacking(false);
		anim.SetTrigger(animation);
	}

	public void EnableCombat(bool enabled) { 
		timeSinceLastAttack = 10; // when entering combat, can attack slightly earlier
		this.isAttacking = false;
		this.combatEnabled = enabled; 
		anim.SetBool ("Combat", enabled);
	}

	public void SetAttacking(bool isAttacking) {
		this.isAttacking = isAttacking;
	}

	public bool IsAttacking() {
		return isAttacking;
	}

	public void Die() {
		combatEnabled = false;
		PlayAnimation ("Death");
		Camera.main.GetComponent<LootSpawner> ().SpawnLoot ("Skeleton", transform.position);
		controller.RemoveFromGroup ();
	}
	
	// TRIGGER EVENTS:

	void OnTriggerEnter(Collider other) {
		OnTriggerStay (other);
	}
	
	void OnTriggerStay(Collider other) {
		if (controller.IsEvading () || player == null)
			return;
		if (player.IsAttacking() && other.gameObject.tag == "PlayerSword") {
			if (!anim.GetCurrentAnimatorStateInfo (0).IsName ("Damage")) {						
				player.SetAttacking(false);
				float damage = Statistics.CalculateDamage ("Player", "Skeleton");
				currentHealth -= damage;
				if (currentHealth <= 0)
					Die ();
				else
					PlayAnimation("Damage");
			}
		}
	}
}
