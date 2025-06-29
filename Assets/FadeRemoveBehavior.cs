using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeRemoveBehavior : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}

    public float FadeTime = 0.5f;

    private float timeElapsesd = 0f;
    SpriteRenderer spriteRenderer;
    GameObject objToRemove;
    Color startColor;
    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        timeElapsesd = 0f;
        spriteRenderer = animator.GetComponent<SpriteRenderer>();
        startColor = spriteRenderer.color;
        objToRemove = animator.gameObject;

    }

    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        timeElapsesd += Time.deltaTime;
        float newAlpha =startColor.a * (1 -( timeElapsesd / FadeTime));
        spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);
        if (timeElapsesd > FadeTime)
        {
            Destroy(objToRemove);
        }



    }
}
