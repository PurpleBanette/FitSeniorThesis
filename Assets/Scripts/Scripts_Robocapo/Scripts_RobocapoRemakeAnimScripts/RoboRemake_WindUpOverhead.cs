using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoboRemake_WindUpOverhead : StateMachineBehaviour
{
    float cachedSpeed;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        //bossAiRobocapoRemake.instance.ActivateGuard();
        cachedSpeed = bossAiRobocapoRemake.instance.bossNavAgent.speed;
        bossAiRobocapoRemake.instance.bossNavAgent.speed = 0;
        bossAiRobocapoRemake.instance.bossIsAttacking = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        bossAiRobocapoRemake.instance.bossNavAgent.speed = cachedSpeed;
        bossAiRobocapoRemake.instance.bossIsAttacking = false;
        animator.ResetTrigger("TripleStab");
        animator.ResetTrigger("DoubleWind");
        animator.ResetTrigger("3HitCombo");
        animator.ResetTrigger("WindUpOverhead");
        animator.ResetTrigger("StabDash");
        animator.ResetTrigger("Gunspin");
        animator.ResetTrigger("RangedShot");

        if (bossAiRobocapoRemake.instance.GuardUp)
        {
            bossAiRobocapoRemake.instance.DeActivateGuard();
        }
    }
}
