using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Death_Anim : StateMachineBehaviour
{
	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.GetComponentInParent<EnemyController2D>().enabled = false;
		animator.GetComponentInParent<Rigidbody2D>().simulated = false;
		animator.GetComponentInParent<Soldier>().healthBar.transform.parent.gameObject.SetActive(false);
		animator.GetComponentInParent<CircleCollider2D>().enabled = false;
	}



}
