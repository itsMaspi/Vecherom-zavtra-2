using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Death_anim : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponentInParent<EnemyController2D>().enabled = false;
        animator.GetComponentInParent<Rigidbody2D>().simulated = false;
        animator.GetComponentInParent<FireWorm>().enabled = false;
        animator.GetComponentInParent<FireWorm>().healthBar.enabled = false;
        var colliders = animator.GetComponentsInParent<CapsuleCollider2D>();
        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }
    }
}
