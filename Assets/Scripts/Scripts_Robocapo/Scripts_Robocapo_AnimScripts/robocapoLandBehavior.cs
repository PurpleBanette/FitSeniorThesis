using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class robocapoLandBehavior : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossAiRobocapoRemake bossReference = animator.GetComponent<bossAiRobocapoRemake>();
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
        bossAiRobocapoRemake bossReference = animator.GetComponent<bossAiRobocapoRemake>();
        if (bossReference.currentphase == 1)
        {
            bossReference.bossNavAgent.speed = bossReference.bossMoveSpeedP1;
            bossReference.randAttack = 6;
        }
        if (bossReference.currentphase == 2)
        {
            bossReference.bossNavAgent.speed = bossReference.bossMoveSpeedP2;
            bossReference.randAttack = 3;
        }
        if (bossReference.currentphase == 3)
        {
            bossReference.bossNavAgent.speed = bossReference.bossMoveSpeedP3;
            bossReference.randAttack = 4;
        }
        if (bossReference.currentphase == 4)
        {
            bossReference.bossNavAgent.speed = bossReference.bossMoveSpeedP4;
            bossReference.randAttack = 8;
        }
        bossReference.bossIsAttacking = false;
    }
}
