using UnityEngine;
using System.Collections;

public class SkeletonAttackScript : StateMachineBehaviour {

	public int start = 30; // time delay when attack animation can hurt the player
	public int finish = 40; // time until attack animation no longer hurts the player
	public bool attacking = false;
	public int timeSinceAttackStarted = 0;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (stateInfo.IsName ("Attack")) {
			timeSinceAttackStarted = 0;
			attacking = true;
		}
		
	}

	//OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (stateInfo.IsName ("Attack")) {
			timeSinceAttackStarted++;
			if (attacking) {
				Debug.Log ("ATTACKING!!!");
				SkeletonCombat sc = animator.gameObject.GetComponent<SkeletonCombat>();
				if (timeSinceAttackStarted > start) {
					sc.SetAttacking(true);
				} else if (timeSinceAttackStarted > finish) {
					sc.SetAttacking(false);
					attacking = false;
				}
			}
		}
	}
}
