using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackScript : StateMachineBehaviour
{
    ModifiedTPC charctrl;
    bool nextAttack;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        charctrl = animator.GetComponent<ModifiedTPC>();
        charctrl.weapon.enabled = true;
        charctrl.canFall = false;

        charctrl.inAttack1 = true;
        
        //charctrl.comboSet = 1;
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    /*
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(charctrl.inputRecieved)
        {
            nextAttack = true;
            animator.SetTrigger("Attack2");
            
        }
        
    }
    */
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        charctrl.weapon.enabled = false;
        charctrl.canFall = true;
        charctrl.inAttack1 = false;
        
        //Uncomment the line below if the animator gets stuck on this
        //animator.ResetTrigger("Attack");
        
    }

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