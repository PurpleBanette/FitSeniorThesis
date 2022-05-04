using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack1 : StateMachineBehaviour
{

    GameObject boss;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ModifiedTPC.instance.inCombo = true;
        boss = GameObject.FindGameObjectWithTag("Boss");
        //charCtrl.inputRecieved = true;
    }

    
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        ModifiedTPC.instance.transform.LookAt(new Vector3(boss.transform.position.x, ModifiedTPC.instance.transform.position.y, boss.transform.position.z));
        /*
        if (charCtrl.inputRecieved)
        {
            animator.SetTrigger("Attack2");
            charCtrl.inputManager();
            charCtrl.inputRecieved = false;
        }
        */
    }

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
