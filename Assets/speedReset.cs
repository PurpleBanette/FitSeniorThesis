using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speedReset : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(bossAiRobocapoRemake.instance.bossNavAgent.speed == 0)
        {
            if(bossAiRobocapoRemake.instance.currentphase == 1)
            {
                bossAiRobocapoRemake.instance.bossNavAgent.speed = bossAiRobocapoRemake.instance.bossMoveSpeedP1;
            }
            else if (bossAiRobocapoRemake.instance.currentphase == 3)
            {
                bossAiRobocapoRemake.instance.bossNavAgent.speed = bossAiRobocapoRemake.instance.bossMoveSpeedP3;
            }
            else if(bossAiRobocapoRemake.instance.currentphase == 4)
            {
                bossAiRobocapoRemake.instance.bossNavAgent.speed = bossAiRobocapoRemake.instance.bossMoveSpeedP4;
            }
        }    
    }

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
}
