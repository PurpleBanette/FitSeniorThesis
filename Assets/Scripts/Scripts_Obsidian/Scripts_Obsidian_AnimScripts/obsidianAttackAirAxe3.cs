using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obsidianAttackAirAxe3 : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossAiObsidian bossReference = animator.GetComponent<bossAiObsidian>();
        bossReference.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
        bossReference.GetComponent<Rigidbody>().isKinematic = true;
        bossReference.bossNavAgent.speed = 0;
        /*bossReference.jumpAttack = false;*/
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossAiObsidian bossReference = animator.GetComponent<bossAiObsidian>();
        bossReference.bossNavAgent.speed = bossReference.bossMoveSpeedP1;
    }
}
