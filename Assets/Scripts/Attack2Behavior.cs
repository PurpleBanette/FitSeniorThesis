using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack2Behavior : StateMachineBehaviour
{
    ModifiedTPC charctrl;
    bool nextAttack;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        charctrl = animator.GetComponent<ModifiedTPC>();
        charctrl.weapon.enabled = true;
        charctrl.canFall = false;
        charctrl.inAttack2 = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    
    /*
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (charctrl.inputRecieved)
        {
            animator.SetTrigger("Attack3");
        }
    }
    */

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        charctrl.weapon.enabled = false;
        charctrl.canFall = true;
        charctrl.inAttack2 = false;
    }

}
