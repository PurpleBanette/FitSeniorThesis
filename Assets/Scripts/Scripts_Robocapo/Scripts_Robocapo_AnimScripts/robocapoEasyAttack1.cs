using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class robocapoEasyAttack1 : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossAiRobocapo bossReference = animator.GetComponent<bossAiRobocapo>();
        bossReference.bossNavAgent.speed = 0;
        bossReference.bossIsAttacking = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossAiRobocapo bossReference = animator.GetComponent<bossAiRobocapo>();
        bossReference.bossNavAgent.speed = bossReference.bossMoveSpeedP1;
        bossReference.randAttack = 4;
        bossReference.bossIsAttacking = false;
        animator.ResetTrigger("attack1");
        animator.ResetTrigger("attack2");
        animator.ResetTrigger("attack3");
    }
}
