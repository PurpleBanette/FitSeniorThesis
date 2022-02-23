using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obsidianGauntletAttack4 : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossAiObsidian bossReference = animator.GetComponent<bossAiObsidian>();
        //bossReference.bossNavAgent.isStopped = true;
        bossReference.bossNavAgent.speed = 0;
        bossReference.bossIsAttacking = true;
        foreach (var link in bossReference.jumpLinks)
        {
            link.SetActive(false);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossAiObsidian bossReference = animator.GetComponent<bossAiObsidian>();

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossAiObsidian bossReference = animator.GetComponent<bossAiObsidian>();
        //bossReference.bossNavAgent.isStopped = false;
        bossReference.bossNavAgent.speed = bossReference.bossMoveSpeedP3;
        bossReference.bossIsAttacking = false;
        //bossReference.walkPoint = bossReference.bossWaypoints[bossReference.bossWaypointIndex].transform.position;
        bossReference.bossWaypointIndex = Random.Range(bossReference.bossWaypointMin, bossReference.bossWaypointMax);
        bossReference.walkPoint = bossReference.bossWaypoints[bossReference.bossWaypointIndex].transform.position;
        animator.ResetTrigger("gauntletAttack4");
        foreach (var link in bossReference.jumpLinks)
        {
            link.SetActive(true);
        }

    }
}