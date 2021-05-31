using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallDroid_Death_Anim : StateMachineBehaviour
{
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.GetComponentInParent<EnemyController2D>().enabled = false;
		animator.GetComponentInParent<Rigidbody2D>().simulated = false;
		animator.GetComponentInParent<SmallDroid>().healthBar.transform.parent.gameObject.SetActive(false);
		animator.GetComponentInParent<CircleCollider2D>().enabled = false;
	}
}
