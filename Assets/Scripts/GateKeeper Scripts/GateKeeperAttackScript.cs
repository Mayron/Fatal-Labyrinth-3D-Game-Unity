using UnityEngine;
using System.Collections;

public class GateKeeperAttackScript : StateMachineBehaviour {

	public int start = 20; // time delay when attack animation can hurt the player
	public int finish = 30; // time until attack animation no longer hurts the player
	public bool attacking = false;
	public int timeSinceAttackStarted = 0;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (stateInfo.IsName ("Attack1") || stateInfo.IsName ("Attack2")) {
			timeSinceAttackStarted = 0;
			attacking = true;
		}
		
	}

	//OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (stateInfo.IsName ("Attack1") || stateInfo.IsName ("Attack2")) {
			timeSinceAttackStarted++;
			if (attacking) {
				GateKeeperCombat combat = animator.gameObject.GetComponent<GateKeeperCombat>();
				if (timeSinceAttackStarted > start) {
					combat.SetAttacking(true);
				} else if (timeSinceAttackStarted > finish) {
					combat.SetAttacking(false);
					attacking = false;
				}
			}
		}
	}
}
