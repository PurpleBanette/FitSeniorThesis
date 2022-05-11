using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class robocapoJumpBehavior : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossAiRobocapoRemake bossReference = animator.GetComponent<bossAiRobocapoRemake>();
        bossReference.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        bossReference.GetComponent<Rigidbody>().isKinematic = false;
        bossReference.playerTracking = false;
        Vector3 horizontalDirection = bossReference.playerTarget.transform.position - bossReference.transform.position;
        bossReference.GetComponent<Rigidbody>().AddForce(horizontalDirection.normalized * 100, ForceMode.Impulse);
        bossReference.GetComponent<Rigidbody>().AddForce(0, 600, 0);
        bossReference.jumpTrail.SetActive(true);
        bossReference.bossIsAttacking = false;
        animator.ResetTrigger("jumpTimer");
    }
}
