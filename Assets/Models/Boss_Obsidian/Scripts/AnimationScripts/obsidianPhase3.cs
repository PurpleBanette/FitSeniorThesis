using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obsidianPhase3 : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossAiObsidian bossReference = animator.GetComponent<bossAiObsidian>();
        bossReference.bossNavAgent.speed = 0;
        bossReference.phaseChanging = true;
        foreach (var hurtbox in bossReference.hurtboxes)
        {
            hurtbox.SetActive(false);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossAiObsidian bossReference = animator.GetComponent<bossAiObsidian>();
        bossReference.bossNavAgent.speed = 20;
        bossReference.phaseChanging = false;
        foreach (var hurtbox in bossReference.hurtboxes)
        {
            hurtbox.SetActive(true);
        }
        animator.ResetTrigger("gauntletAttack1");
        animator.ResetTrigger("gauntletAttack2");
        animator.ResetTrigger("gauntletAttack3");
        animator.ResetTrigger("gauntletAttack4");
        animator.ResetTrigger("gauntletAttack5");
        animator.ResetTrigger("gauntletAttack6");
    }
}
