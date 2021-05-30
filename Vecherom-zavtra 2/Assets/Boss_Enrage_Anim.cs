using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Enrage_Anim : StateMachineBehaviour
{
    public float t = 0;
    private float minx = 1.5f;
    private float maxx = 1.7f;
    private float miny = 1.5f;
    private float maxy = 1.7f;
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        minx = animator.transform.localScale.x;
        maxx = animator.transform.localScale.x * 1.13f;

    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        // animate the position of the game object...
        animator.transform.localScale = new Vector3(Mathf.Lerp(minx, maxx, t), Mathf.Lerp(miny, maxy, t), Mathf.Lerp(miny, maxy, t));

        // .. and increase the t interpolater
        t += 0.5f * Time.deltaTime;


    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponentInParent<EnemyController2D>().moveSpeed = 5;
    }


}
