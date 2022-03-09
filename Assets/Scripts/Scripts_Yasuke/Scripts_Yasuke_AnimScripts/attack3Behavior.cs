using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack3Behavior : StateMachineBehaviour
{
    ModifiedTPC charctrl;
    GameObject boss;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        charctrl = animator.GetComponent<ModifiedTPC>();
        boss = GameObject.FindGameObjectWithTag("Boss");
        //charctrl.weapon.enabled = true;
        charctrl.canFall = false;
        charctrl.inAttack3 = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        charctrl.transform.LookAt(new Vector3(boss.transform.position.x, charctrl.transform.position.y, boss.transform.position.z));
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        charctrl.inAttack3 = false;
        charctrl.canFall = true;
        charctrl.weapon.enabled = false;
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
