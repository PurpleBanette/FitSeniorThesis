using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack2Behavior : StateMachineBehaviour
{
    ModifiedTPC charctrl;
    bool nextAttack;
    GameObject boss;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        charctrl = animator.GetComponent<ModifiedTPC>();
        boss = GameObject.FindGameObjectWithTag("Boss");
        //charctrl.weapon.enabled = true;
        charctrl.canFall = false;
        charctrl.inAttack2 = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        /*
            if (charctrl.inputRecieved)
            {
                animator.SetTrigger("Attack3");
            }
        */
        charctrl.transform.LookAt(new Vector3(boss.transform.position.x, charctrl.transform.position.y, boss.transform.position.z));
    }
    

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        charctrl.weapon.enabled = false;
        charctrl.canFall = true;
        charctrl.inAttack2 = false;
    }

}
