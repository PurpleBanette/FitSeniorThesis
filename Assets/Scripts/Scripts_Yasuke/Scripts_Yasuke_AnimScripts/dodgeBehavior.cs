using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dodgeBehavior : StateMachineBehaviour
{
    ModifiedTPC charCtrl;
    float dodgespeed = 3f;
     //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        charCtrl = animator.GetComponent<ModifiedTPC>();
        charCtrl.MoveSpeed = charCtrl.MoveSpeed * dodgespeed;
        charCtrl.isDashing = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        /*IEnumerator DashAfterImage()
        {
            ModifiedTPC.instance.dashPoolManager = yasukeDashPool.instance.GetPooledObjectManaged(yasukeDashPool.instance.pooledYasukes, ModifiedTPC.instance.dashPoolManager, null, ModifiedTPC.instance.transform, 0, null, 0);
            yield return new WaitForSeconds(0.1f);
        }
        DashAfterImage();*/
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        charCtrl.MoveSpeed = charCtrl.MoveSpeed / dodgespeed;
        charCtrl.isDashing = false;
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
