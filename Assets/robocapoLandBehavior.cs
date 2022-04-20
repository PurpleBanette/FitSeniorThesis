using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class robocapoLandBehavior : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossAiRobocapo bossReference = animator.GetComponent<bossAiRobocapo>();
        bossReference.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
        bossReference.GetComponent<Rigidbody>().isKinematic = true;
        bossReference.bossNavAgent.speed = 0;
        /*bossReference.jumpAttack = false;*/
        bossReference.jumpTrail.SetActive(false);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossAiRobocapo bossReference = animator.GetComponent<bossAiRobocapo>();
        bossReference.bossNavAgent.speed = bossReference.bossMoveSpeedP1;
    }
}
